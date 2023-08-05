using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils.Unity
{
    public static class Vector2Extensions
    {
        public static float Cross(this Vector2 a, Vector2 b) => (a.x * b.y) - a.y * b.x;
        public static float TaxiDistance(this Vector2 v) => Mathf.Abs(v.x) + Mathf.Abs(v.y);
    }
}
