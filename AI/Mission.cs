using System;
using System.Collections;
using UnityEngine;

namespace Utils.Unity
{
    public abstract class Mission
    {
        public string Name { get; private set; }
        public float Priority { get; private set; }
        public float LastTimeAcquiredPriority { get; private set; }
        public float LastTimeLostPriority { get; private set; }
        public bool HasPriority
        {
            get => hasPriority;
            set
            {
                if(value != hasPriority)
                {
                    hasPriority = value;
                    if(hasPriority)
                    {
                        LastTimeAcquiredPriority = Time.time;
                        OnAcquiredPriority();
                    }
                    else
                    {
                        LastTimeLostPriority = Time.time;
                        OnLostPriority();
                    }
                }
            }
        }
        public event Action<Mission> OnAborted;

        private bool hasPriority = false;

        public Mission(string name)
        {
            Name = name;
        }
        public override string ToString() => $"{Name}: {GetPriority():0.00}";
        public abstract float GetPriority();
        public virtual void Update() { }
        public virtual void Update1() { }
        public virtual void OnAcquiredPriority() { }
        public virtual void OnLostPriority() { }

        protected void Abort()
        {
            HasPriority = false;
            OnAborted?.Invoke(this);
        }
    }
}