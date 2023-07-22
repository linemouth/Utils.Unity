using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Utils.Unity
{
	[System.Serializable]
	public class ResourceCache
	{
		public ResourceCache Parent
		{
			get => parent_;
			set
			{
				if(value != parent_)
				{
					ResourceCache cache = value;
					while(cache != null)
					{
						if(cache == this)
						{
							throw new ArgumentException("Tried to set parent in a recursive loop.");
						}
						cache = cache.Parent;
					}
					if(parent_ != null)
					{
						double stored = parent_.Request(Capacity * Fraction);
						if(stored == double.NaN)
						{
							throw new ArgumentException("Stored is NaN.");
						}
						stored_ = stored;
						parent_.AddChildCapacity(Capacity + ChildrenCapacity);
						parent_.children_.Remove(this);
					}
					parent_ = value;
					if(parent_ != null)
					{
						double toGive = stored_;
						parent_.children_.Add(this);
						parent_.AddChildCapacity(Capacity + ChildrenCapacity);
						Stored += toGive;
						if(Stored > ConnectedCapacity)
						{
							throw new OverflowException("ResourceCache overflow.");
						}
					}
				}
			}
		}
		public HashSet<ResourceCache> Children => children_.ToHashSet();
		public ResourceCache Root => Parent != null ? Parent.Root : this;
		public double ConnectedCapacity => (Root.capacity_ + Root.childrenCapacity_).Round(3);
		public double ChildrenCapacity => childrenCapacity_;
		public double Capacity
		{
			get => capacity_;
			set
			{
				value = value.Round(3);
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Tried to set ResourceCache.Capacity to negative value.");
				}
				if(value == double.NaN)
				{
					throw new ArgumentException("Capacity is NaN.");
				}
				if(Parent != null)
				{
					double difference = value - capacity_;
					Parent.AddChildCapacity(difference);
				}
				capacity_ = value;
				if(capacity_ < stored_)
				{
					// Truncate any amount beyond the local capacity.
					stored_ = capacity_;
				}
			}
		}
		public double Stored
		{
			get => Root.stored_;
			set
			{
				if(Parent != null)
				{
					Root.Stored = value;
				}
				else
				{
					value = value.Round(3);
					if(value < 0)
					{
						throw new ArgumentOutOfRangeException("Tried to set ResourceCache.Stored to negative value.");
					}
					else if(value > ConnectedCapacity)
					{
						throw new ArgumentOutOfRangeException("Tried to set ResourceCache.Stored to excessive value.");
					}
					if(value == double.NaN)
					{
						throw new ArgumentException("Stored is NaN.");
					}
					stored_ = value;
				}
			}
		}
		public double Available => Root.ConnectedCapacity - Root.Stored;
		public double Fraction
		{
			get
			{
				double capacity = Root.ConnectedCapacity;
				return capacity > 0 ? Root.Stored / capacity : 0;
			}
			set
			{
				value = (value * Root.ConnectedCapacity).Round(3);
				if(value < 0)
				{
					throw new ArgumentOutOfRangeException("Tried to set ResourceCache.Fraction to negative value.");
				}
				else if(value > 1)
				{
					throw new ArgumentOutOfRangeException("Tried to set ResourceCache.Fraction to excessive value.");
				}
				Root.Stored = value;
			}
		}
		public bool IsEmpty => Stored < double.Epsilon;
		public bool IsFull => Available < double.Epsilon;

		private ResourceCache parent_ = null;
		private readonly HashSet<ResourceCache> children_ = new HashSet<ResourceCache>();
		private double stored_ = 0;
		private double capacity_ = 0;
		private double childrenCapacity_ = 0;

		/// <summary>Constructs a new ResourceCache.</summary>
		/// <param name="capacity">The maximum amount which can be stored in the ResourceCache.</param>
		/// <param name="stored">The initial amount to store in the ResourceCache. If greater than</param>
		/// <param name="parent"></param>
		/// <exception cref="ArgumentOutOfRangeException">If stored is  is negative.</exception>
		public ResourceCache(double capacity = 0, double stored = 0, ResourceCache parent = null)
		{
			Capacity = capacity;
			Parent = parent;
			Give(stored);
		}
		/// <summary>Transfers up to some amount from one ResourceCache to another.</summary>
		/// <param name="from">The ResourceCache from which to take the transferred amount.</param>
		/// <param name="to">The ResourceCache to which to give the transferred amount.</param>
		/// <param name="maxAmount">The maximum amount that can be transferred.</param>
		/// <returns>The total amount actually transferred, which may be in the range [0, maxAmount].</returns>
		/// <exception cref="ArgumentOutOfRangeException">If maxAmount is negative.</exception>
		public static double Transfer(ResourceCache from, ResourceCache to, double maxAmount)
		{
			if(maxAmount < 0)
			{
				throw new ArgumentOutOfRangeException("Tried to transfer negative amount between ResourceCaches.");
			}
			double amount = Utils.Math.Min(maxAmount, from.Stored, to.Available).Round(3);
			from.Stored -= amount;
			to.Stored += amount;
			return amount;
		}
		/// <summary>Gives up to some amount to a ResourceCache.</summary>
		/// <param name="maxAmount">The maximum amount that can be given.</param>
		/// <returns>The amount actually accepted by the ResourceCache.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If maxAmount is negative.</exception>
		public double Offer(double maxAmount)
		{
			if(Parent != null)
			{
				return Root.Offer(maxAmount);
			}

			maxAmount = maxAmount.Round(3);
			if(maxAmount < 0)
			{
				throw new ArgumentOutOfRangeException("Tried to offer negative amount to ResourceCache.");
			}
			else if(maxAmount >= Available)
			{
				double transferred = Available;
				Stored = ConnectedCapacity;
				return transferred;
			}
			else
			{
				Stored += maxAmount;
				return maxAmount;
			}
		}
		/// <summary>Takes up to some amount from a ResourceCache.</summary>
		/// <param name="maxAmount">The maximum amount that can be taken.</param>
		/// <returns>The actual amount provided by the ResourceCache.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If maxAmount is negative.</exception>
		public double Request(double maxAmount)
		{
			if(Parent != null)
			{
				return Root.Request(maxAmount);
			}

			maxAmount = maxAmount.Round(3);
			if(maxAmount < 0)
			{
				throw new ArgumentOutOfRangeException("Tried to request negative amount from ResourceCache.");
			}
			else if(maxAmount >= Stored)
			{
				double transferred = Stored;
				Stored = 0;
				return transferred;
			}
			else
			{
				Stored -= maxAmount;
				return maxAmount;
			}
		}
		/// <summary>Gives all or none of some amount to a ResourceCache.</summary>
		/// <param name="amount">The amount to be offered to the ResourceCache.</param>
		/// <returns>`True` if the ResourceCache accepted the amount, otherwise `False`.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If amount is negative.</exception>
		public bool Give(double amount)
		{
			if(Parent != null)
			{
				return Root.Give(amount);
			}

			amount = amount.Round(3);
			if(amount < 0)
			{
				throw new ArgumentOutOfRangeException("Tried to give negative amount to ResourceCache.");
			}
			else if(amount > Available)
			{
				return false;
			}
			else
			{
				Stored += amount;
				return true;
			}
		}
		/// <summary>Takes all or none of some amount from a ResourceCache.</summary>
		/// <param name="amount">The amount to be requested from the ResourceCache.</param>
		/// <returns>`True` if the ResourceCache supplied the amount, otherwise `False`.</returns>
		/// <exception cref="ArgumentOutOfRangeException">If amount is negative.</exception>
		public bool Take(double amount)
		{
			if(Parent != null)
			{
				return Root.Take(amount);
			}

			amount = amount.Round(3);
			if(amount < 0)
			{
				throw new ArgumentOutOfRangeException("Tried to take negative amount from ResourceCache.");
			}
			else if(amount > Stored)
			{
				return false;
			}
			else
			{
				Stored -= amount;
				return true;
			}
		}
		public override string ToString() => $"{{ResourceCache: {Stored:0.000}/{ConnectedCapacity:0.000}}}";

		private void AddChildCapacity(double difference)
		{
			childrenCapacity_ = (childrenCapacity_ + difference).Round(3);
			if(Parent != null)
			{
				Parent.AddChildCapacity(difference);
			}
		}
	}
}
