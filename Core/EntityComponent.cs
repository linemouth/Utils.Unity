using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    public Entity Entity { get; private set; }
    public Faction Faction { get => Entity.Faction; set => Entity.Faction = value; }
    public StatBlock StatBlock => Entity.StatBlock;

    protected virtual void Awake()
    {
        Entity = gameObject.GetComponentInParent<Entity>();
        if(Entity == null)
        {
            Entity = gameObject.AddComponent<Entity>();
        }
    }
}