using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public string Name;
    public Color Color;
    //public List<Player> Players = new List<Player>();
    public List<Entity> Entities = new List<Entity>();
    public Dictionary<string, ResourceCache> Resources = new Dictionary<string, ResourceCache>
    {
        { "Wood", new ResourceCache(float.PositiveInfinity) },
        { "Food", new ResourceCache(float.PositiveInfinity) },
        { "Stone", new ResourceCache(float.PositiveInfinity) },
        { "Metal", new ResourceCache(float.PositiveInfinity) }
    };

    public Faction(string name = "", Color color = new Color())
    {
        Name = name;
        Color = color;
    }
    public override string ToString() => $"{{Faction: \"{Name}\"}}";
}
