using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Unity
{
    public class Faction
    {
        public string Name;
        public UnityEngine.Color Color;
        public List<Entity> Entities = new List<Entity>();

        public Faction(string name = "", UnityEngine.Color color = new UnityEngine.Color())
        {
            Name = name;
            Color = color;
        }
        public override string ToString() => $"{{Faction: \"{Name}\"}}";
    }
}
