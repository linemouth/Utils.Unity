using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Unity
{
    public class StatBar : Stat<float>
    {
        private RawImage image;
        private Material material;

        public StatBar(UnityEngine.Color barColor, UnityEngine.Color backgroundColor, Vector2 size, string name = null) : this(null, barColor, backgroundColor, size, name == null ? "Bar" : $"{name} Bar") { }
        public StatBar(Func<float> getValue, UnityEngine.Color barColor, UnityEngine.Color backgroundColor, Vector2 size, string name = null) : base(getValue, size, name == null ? "Bar" : $"{name} Bar")
        {
            material = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/Fractional Bar"));
            material.SetColor("_Foreground", barColor);
            material.SetColor("_Background", backgroundColor);
            image = GameObject.GetOrAddComponent<RawImage>();
            image.material = material;
            image.raycastTarget = false;
            image.maskable = false;
            Update();
        }

        protected override void Refresh()
        {
            material.SetFloat("_Fraction", Value);
        }
    }
}
