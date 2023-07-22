using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if(!go.TryGetComponent<T>(out T result))
            {
                result = go.AddComponent<T>();
            }
            return result;
        }
        public static bool TryGetComponentInChildren<T>(this GameObject go, out T result) where T : Component
        {
            result = go.GetComponentInChildren<T>();
            return result != null;
        }
        public static bool TryGetComponentInParent<T>(this GameObject go, out T result) where T : Component
        {
            result = go.GetComponentInParent<T>();
            return result != null;
        }
    }
}
