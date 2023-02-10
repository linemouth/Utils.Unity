using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatDivider : IStat
{
    public GameObject GameObject { get; private set; }
    public RectTransform RectTransform { get; private set; }

    private RawImage image;

    public StatDivider()
    {
        GameObject = new GameObject("Divider");
        RectTransform = GameObject.GetOrAddComponent<RectTransform>();
        RectTransform.sizeDelta = new Vector2(50, 2);
        image = GameObject.AddComponent<RawImage>();
        image.material = Resources.GetBuiltinResource<Font>("Arial.ttf").material;
    }
    public void Update() { }
}
