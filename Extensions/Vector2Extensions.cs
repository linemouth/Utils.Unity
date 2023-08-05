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
        /// <summary>Rotates a direction vector by some angle.</summary>
        /// <param name="v">A Vector2 to rotate.</param>
        /// <param name="angle">Angle by which to rotate, in degrees.</param>
        public static Vector2 Rotate(this Vector2 v, float angle)
        {
            float radians = (float)Math.DegToRad * angle;
            float sin = Math.Sin(radians);
            float cos = Math.Cos(radians);
            return new Vector2(
                cos * v.x - sin * v.y,
                sin * v.x + cos * v.y
            );
        }
    }
}
