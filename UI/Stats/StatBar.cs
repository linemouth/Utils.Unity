using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Color = UnityEngine.Color;

public class StatBar : Stat<float>
{
    private RawImage image;
    private Material material;

    public StatBar(Func<float> getValue, Color barColor, Color backgroundColor, Vector2 size, string name = null) : base(getValue, size, name == null ? "Bar" : $"{name} Bar")
    {
        material = UnityEngine.Object.Instantiate(Resources.Load<Material>("Materials/UI/Simple Bar"));
        material.SetColor("_Color", barColor);
        material.SetColor("_Background", backgroundColor);
        image = GameObject.GetOrAddComponent<RawImage>();
        image.material = material;
        image.raycastTarget = false;
        image.maskable = false;
        Update();
    }

    protected override void Refresh()
    {
        material.SetFloat("_Value", Value);
    }
}
