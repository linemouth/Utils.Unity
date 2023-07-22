using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if(!component.gameObject.TryGetComponent<T>(out T result))
            {
                result = component.gameObject.AddComponent<T>();
            }
            return result;
        }
        public static bool TryGetComponentInChildren<T>(this Component component, out T result) where T : Component
        {
            result = component.GetComponentInChildren<T>();
            return result != null;
        }
        public static bool TryGetComponentInParent<T>(this Component component, out T result) where T : Component
        {
            result = component.GetComponentInParent<T>();
            return result != null;
        }
    }
}
