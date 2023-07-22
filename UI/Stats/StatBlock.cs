using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Unity
{
    public class StatBlock : MonoBehaviour
    {
        public Entity Entity
        {
            get => entity;
            set
            {
                entity = value;
                if(entity != null && localCanvas != null)
                {
                    localCanvas.transform.SetParent(entity.transform);
                    localCanvas.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    localCanvas.transform.localPosition = new Vector3(0, entity.Bounds.max.y * 1.3f + 2, 0);
                    if(Canvas == null)
                    {
                        transform.SetParent(localCanvas.transform);
                        transform.localScale = Vector3.one;
                        transform.localPosition = Vector3.zero;
                    }
                }
            }
        }
        public Canvas Canvas
        {
            get => canvas;
            set
            {
                if(value != null)
                {
                    canvas = value;
                    transform.parent = canvas.transform;
                    transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                    transform.localScale = Vector3.one;
                }
                else if(localCanvas != null)
                {
                    canvas = null;
                    transform.parent = localCanvas.transform;
                    transform.localScale = Vector3.one;
                    transform.localPosition = Vector3.zero;
                }
            }
        }

        public Entity entity;
        private List<IStat> stats = new List<IStat>();
        private RectTransform rectTransform;
        private RectTransform localRectTransform;
        private VerticalLayoutGroup layout;
        private Canvas canvas;
        private Canvas localCanvas;

        public IStat Add(IStat stat)
        {
            stat.GameObject.transform.SetParent(transform);
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
            Destroy(stat?.GameObject);
        }

        private void Start()
        {
            // Canvas = ScreenUI.Canvas;
            GameObject localUI = new GameObject("Local UI");
            localCanvas = localUI.GetOrAddComponent<Canvas>();
            localCanvas.enabled = true;
            localCanvas.worldCamera = Camera.main;
            localCanvas.renderMode = RenderMode.WorldSpace;
            localCanvas.scaleFactor = 4f;
            localRectTransform = gameObject.GetOrAddComponent<RectTransform>();
            localRectTransform.sizeDelta = new Vector2(50, 50);
            localRectTransform.pivot = new Vector2(0.5f, 0);

            Entity = entity; // This causes the property to trigger a resize of the localCanvas.
            rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            rectTransform.pivot = new Vector2(0.5f, 0);
            layout = gameObject.GetOrAddComponent<VerticalLayoutGroup>();
            layout.spacing = 0;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childAlignment = TextAnchor.LowerCenter;
            UpdateLayout();
        }
        private void Update()
        {
            foreach(IStat stat in stats)
            {
                stat.Update();
            }
        }
        private void OnGUI() => UpdatePosition();
        //private void OnWillRenderObject() => UpdatePosition();
        //private void OnDrawGizmos() => UpdatePosition();
        private void UpdateLayout()
        {
            if(layout != null)
            {
                layout.CalculateLayoutInputHorizontal();
                layout.CalculateLayoutInputVertical();
            }
        }
        private void OnDestroy()
        {
            foreach(var stat in stats)
            {
                Destroy(stat?.GameObject);
            }
        }
        private void UpdatePosition(bool updateWorldSpace = false)
        {
            if(entity != null)
            {
                // Draw the StatBlock on a screen canvas.
                if(canvas != null && canvas != localCanvas)
                {
                    // Get the stats block position above the entity in world space.
                    Vector3 position = entity.transform.position;
                    position.y += entity.Bounds.max.y * 1.2f + 1;

                    // Transform position from world space to screen space.
                    position = canvas.worldCamera.WorldToScreenPoint(position);
                    position.x -= canvas.pixelRect.width * 0.5f;
                    position.y -= canvas.pixelRect.height * 0.5f;
                    position.z = 0;

                    // Offset the stats up slightly in screen space.
                    transform.localPosition = position;
                }
                // Draw the StatBlock in world space.
                else
                {
                    // Rotate the StatBlock to face the camera.
                    Camera camera = Camera.current ?? Camera.main;
                    if(localCanvas != null && camera != null)
                    {
                        localCanvas.transform.rotation = Quaternion.LookRotation(camera.transform.forward);
                    }

                    // Scale the StatBlock to be 1:1 pixel-scale from the camera's perspective.
                    Vector3 relativePosition = camera.transform.InverseTransformPoint(transform.position);
                    float scale = 0.01f * relativePosition.z * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                    transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }
}
