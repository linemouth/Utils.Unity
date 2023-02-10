using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        if(!go.TryGetComponent<T>(out T component))
        {
            component = go.AddComponent<T>();
        }
        return component;
    }
    public static T GetOrAddComponent<T>(this Component c) where T : Component
    {
        if(!c.gameObject.TryGetComponent<T>(out T component))
        {
            component = c.gameObject.AddComponent<T>();
        }
        return component;
    }
}

