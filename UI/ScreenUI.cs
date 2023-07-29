using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Unity
{
    public class ScreenUI : MonoBehaviour
    {
        public static Canvas Canvas => instance.canvas;
        public static Transform Transform => instance.transform;
        public static StatBlock LeftSidebar => instance.leftSidebar;
        public static StatBlock RightSidebar => instance.rightSidebar;

        private static ScreenUI instance = null;
        private Canvas canvas;
        private StatBlock leftSidebar;
        private StatBlock rightSidebar;
        private RectTransform leftSidebarTransform;
        private RectTransform rightSidebarTransform;
        private VerticalLayoutGroup leftLayout;
        private VerticalLayoutGroup rightLayout;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                canvas = gameObject.GetOrAddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = Camera.main;
                //canvas.pixelPerfect = true;
                canvas.enabled = true;

                GameObject go = new GameObject("Left Sidebar");
                leftSidebar = go.AddComponent<StatBlock>();
                leftSidebarTransform = leftSidebar.GetOrAddComponent<RectTransform>();
                leftSidebarTransform.SetParent(transform);
                leftSidebarTransform.anchorMin = new Vector2(0, 0);
                leftSidebarTransform.anchorMax = new Vector2(0, 1);
                leftSidebarTransform.pivot = new Vector2(0, 0.5f);
                leftSidebarTransform.sizeDelta = new Vector2(160, 0);
                leftSidebarTransform.anchoredPosition = new Vector2(0, 0);
                leftLayout = leftSidebar.GetOrAddComponent<VerticalLayoutGroup>();
                leftLayout.childAlignment = TextAnchor.UpperLeft;
                leftLayout.childControlHeight = false;
                leftLayout.childControlWidth = true;
                leftLayout.childForceExpandHeight = false;
                leftLayout.childForceExpandWidth = true;
                leftLayout.childScaleHeight = false;
                leftLayout.childScaleWidth = false;
                leftLayout.padding = new RectOffset(4, 0, 4, 0);
                leftLayout.spacing = 4;

                go = new GameObject("Right Sidebar");
                rightSidebar = go.AddComponent<StatBlock>();
                rightSidebarTransform = rightSidebar.GetOrAddComponent<RectTransform>();
                rightSidebarTransform.SetParent(transform);
                rightSidebarTransform.anchorMin = new Vector2(1, 0);
                rightSidebarTransform.anchorMax = new Vector2(1, 1);
                rightSidebarTransform.pivot = new Vector2(1, 0.5f);
                rightSidebarTransform.sizeDelta = new Vector2(160, 0);
                rightSidebarTransform.anchoredPosition = new Vector2(0, 0);
                rightLayout = rightSidebar.GetOrAddComponent<VerticalLayoutGroup>();
                rightLayout.childAlignment = TextAnchor.UpperRight;
                rightLayout.childControlHeight = false;
                rightLayout.childControlWidth = true;
                rightLayout.childForceExpandHeight = false;
                rightLayout.childForceExpandWidth = true;
                rightLayout.childScaleHeight = false;
                rightLayout.childScaleWidth = false;
                rightLayout.padding = new RectOffset(4, 0, 4, 0);
                rightLayout.spacing = 4;
            }
            else
            {
                throw new InvalidOperationException("Cannot create second ScreenUI Canvas.");
            }
        }
    }
}
