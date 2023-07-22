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
        public StatBlock StatBlock
        {
            get
            {
                if(statBlock == null)
                {
                    GameObject go = new GameObject("Stats Block for " + gameObject.name);
                    statBlock = go.GetOrAddComponent<StatBlock>();
                    statBlock.entity = this;
                    statBlock.Add(new StatLabel(() => name, "Name"));
                    //StatBlock.AddStat(new StatBar(() => 1, new Color(0.0f, 0.6f, 1.0f), Color.black, new Vector2(50, 3), "Shield"));
                    //StatBlock.AddStat(new StatBar(() => 1, new Color(1.0f, 0.8f, 0.2f), Color.black, new Vector2(50, 3), "Armor"));
                    //StatBlock.AddStat(new StatBar(() => 1, new Color(0.0f, 1.0f, 0.0f), Color.black, new Vector2(50, 3), "Health"));
                }
                return statBlock;
            }
        }
        public event Action<Bounds> BoundsChanged;
        public string Name { get => name; set => name = value; }

        private Faction faction;
        private StatBlock statBlock;

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
            if(StatBlock != null)
            {
                StatBlock.Entity = this;
            }
        }

        private void Start()
        {
            UpdateBounds();
        }
        private void OnDestroy()
        {
            if(StatBlock != null)
            {
                Destroy(StatBlock.gameObject);
            }
        }
    }
}
