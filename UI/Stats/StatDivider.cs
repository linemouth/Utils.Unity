using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Utils.Unity
{
    public class StatDivider : IStat
    {
        public GameObject GameObject { get; private set; }
        public RectTransform RectTransform { get; private set; }

        private RawImage image;

        public StatDivider()
        {
            GameObject = new GameObject("Divider");
            RectTransform = GameObject.GetOrAddComponent<RectTransform>();
            RectTransform.sizeDelta = new Vector2(64, 3);
            image = GameObject.AddComponent<RawImage>();
            image.material = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf").material;
        }
        public void Update() { }
    }
}
