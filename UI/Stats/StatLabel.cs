using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatLabel : Stat<string>
{
    private Text text;

    public StatLabel(Func<string> getValue, string name = null) : base(getValue, new Vector2(50, 14), name == null ? "Label" : $"{name} Label")
    {
        RectTransform rectTransform = GameObject.GetOrAddComponent<RectTransform>();
        text = GameObject.GetOrAddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 12;
        text.supportRichText = false;
        text.alignment = TextAnchor.UpperLeft;
        text.alignByGeometry = true;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Truncate;
        text.color = Color.white;
        text.raycastTarget = false;
        text.maskable = false;
        Update();
    }

    protected override void Refresh()
    {
        text.text = Value;
    }
}
