using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Unity
{
    public class StatBlock : MonoBehaviour
    {
        [Range(0, 5)]
        public float updatePeriod = 1;

        protected List<IStat> stats = new List<IStat>();
        protected RectTransform rectTransform;
        protected VerticalLayoutGroup layout;

        private float nextUpdateTime = 0;

        public T Add<T>(T stat) where T : IStat
        {
            stat.GameObject.transform.SetParent(transform);
            stat.GameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            stat.GameObject.transform.localScale = Vector3.one;
            stats.Add(stat);
            UpdateLayout();
            return stat;
        }
        public T Remove<T>(T stat) where T : IStat
        {
            stats.Remove(stat);
            UpdateLayout();
            return stat;
        }

        protected virtual void Awake()
        {
            nextUpdateTime = Time.time;
            rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(64, 64);
            //rectTransform.pivot = new Vector2(0.5f, 0);
            layout = gameObject.GetOrAddComponent<VerticalLayoutGroup>();
            layout.spacing = 0;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childAlignment = TextAnchor.UpperLeft;
            UpdateLayout();
        }
        protected virtual void Update()
        {
            if(Time.time >= nextUpdateTime)
            {
                nextUpdateTime += updatePeriod;
                foreach(IStat stat in stats)
                {
                    stat.Update();
                }
            }
        }

        private void UpdateLayout()
        {
            if(layout != null)
            {
                layout.CalculateLayoutInputHorizontal();
                layout.CalculateLayoutInputVertical();
            }
        }
    }
}
