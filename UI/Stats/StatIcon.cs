using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatIcon : Stat<string>
{
    private RawImage image;

    public StatIcon(Func<string> getValue, Vector2 size, string name = null) : base(getValue, size, name == null ? "Icon" : $"{name} Icon")
    {
        image = GameObject.GetOrAddComponent<RawImage>();
        image.raycastTarget = false;
        image.maskable = false;
        Update();
    }

    protected override void Refresh()
    {
        // Set material to an icon using the string as the path to an image.
        Material material = UnityEngine.Object.Instantiate(Resources.Load<Material>(Value));
        image.material = material;
    }
}
