using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Utils.Unity
{
    public class StatGroup : IStat
    {
        public GameObject GameObject { get; private set; }
        public RectTransform RectTransform { get; private set; }
        private VerticalLayoutGroup layout;

        private List<IStat> stats = new List<IStat>();

        public StatGroup(string name = null)
        {
            GameObject = new GameObject(name);
            RectTransform = GameObject.GetOrAddComponent<RectTransform>();
            RectTransform.sizeDelta = new Vector2(50, 50);
            RectTransform.pivot = new Vector2(0.5f, 0);
            layout = GameObject.GetOrAddComponent<VerticalLayoutGroup>();
            layout.spacing = 0;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childAlignment = TextAnchor.UpperCenter;
            UpdateLayout();
        }
        public IStat Add(IStat stat)
        {
            stat.GameObject.transform.SetParent(RectTransform);
            stat.GameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            stat.GameObject.transform.localScale = Vector3.one;
            stats.Add(stat);
            UpdateLayout();
            return stat;
        }
        public void Remove(IStat stat)
        {
            stats.Remove(stat);
            UpdateLayout();
            UnityEngine.Object.Destroy(stat?.GameObject);
        }
        public void UpdateLayout()
        {
            if(layout != null)
            {
                layout.CalculateLayoutInputHorizontal();
                layout.CalculateLayoutInputVertical();
                RectTransform.sizeDelta = new Vector2(layout.preferredWidth, layout.preferredHeight);
            }
        }
        public void Update()
        {
            foreach(IStat stat in stats)
            {
                stat.Update();
            }
        }
    }
}
