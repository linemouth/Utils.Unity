using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Unity
{
    public class Entity : MonoBehaviour
    {
        public Faction Faction
        {
            get => faction;
            set
            {
                if(faction != null)
                {
                    Faction.Entities.Remove(this);
                }
                faction = value;
                if(faction != null)
                {
                    Faction.Entities.Add(this);
                }
            }
        }
        public Bounds Bounds { get; private set; }
        public EntityUI UI
        {
            get
            {
                if(ui == null)
                {
                    GameObject go = new GameObject("Stats Block for " + gameObject.name);
                    ui = go.GetOrAddComponent<EntityUI>();
                    ui.Entity = this;
                    ui.Add(new StatLabel(() => name, "Name"));
                }
                return ui;
            }
        }
        public event Action<Bounds> BoundsChanged;
        public string Name { get => name; set => name = value; }

        private Faction faction;
        private EntityUI ui = null;

        public void Kill() { }
        public void UpdateBounds()
        {
            Bounds bounds = new Bounds(transform.position, Vector3.zero);
            foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            };
            Bounds = bounds;
            BoundsChanged?.Invoke(Bounds);
        }

        private void Start()
        {
            UpdateBounds();
        }
    }
}
