using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Utils.Unity;
using UnityEngine.Assertions;

namespace Utils.Unity
{
    public class EntityUI : StatBlock
    {
        public Entity Entity
        {
            get => entity;
            set
            {
                Assert.IsNull(entity, "Tried to set entity of EntityUI twice.");
                entity = value;
                transform.SetParent(entity.transform, false);
                UpdatePosition(entity.Bounds);
            }
        }

        private Entity entity = null;
        private Canvas canvas;
        
        protected override void Awake()
        {
            base.Awake();

            // Create canvas.
            canvas = gameObject.GetOrAddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
        }

        private void OnGUI() => UpdateOrientation();
        //private void OnWillRenderObject() => UpdatePosition();
        //private void OnDrawGizmos() => UpdatePosition();
        private void UpdatePosition(Bounds bounds)
        {
            transform.localPosition = new Vector3(0, bounds.max.y * 1.2f + 5, 0);
        }
        private void UpdateOrientation()
        {
            Assert.IsNotNull(Entity, "EntityUI doesn't have an entity.");

            // Rotate the StatBlock to face the camera.
            Camera camera = Camera.current ?? Camera.main;
            if(camera != null)
            {
                transform.rotation = Quaternion.LookRotation(camera.transform.forward);
            }

            // Scale to be 1:1 pixel-scale from the camera's perspective.
            Vector3 relativePosition = camera.transform.InverseTransformPoint(transform.position);
            float scale = 0.005f * relativePosition.z * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            transform.localScale = new Vector3(scale, scale, scale);
        }
        /*private void OnDestroy()
        {
            foreach(var stat in stats)
            {
                Destroy(stat?.GameObject);
            }
        }*/
    }
}
