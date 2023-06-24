using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public string Name;
    public Color Color;
    public List<Entity> Entities = new List<Entity>();

    public Faction(string name = "", Color color = new Color())
    {
        Name = name;
        Color = color;
    }
    public override string ToString() => $"{{Faction: \"{Name}\"}}";
}
