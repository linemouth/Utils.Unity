using System;
using UnityEngine;

namespace Utils.Unity
{
    public abstract class Stat<T> : IStat where T : IEquatable<T>
    {
        public GameObject GameObject { get; private set; }
        public RectTransform RectTransform { get; private set; }
        public T Value
        {
            get => value;
            set
            {
                if(!Equals(this.value, value))
                {
                    this.value = value;
                    valueChanged = true;
                }
            }
        }

        private Func<T> getValue;
        private bool valueChanged;
        private T value = default;

        public Stat(Vector2 size, string name) : this(null, size, name) { }
        public Stat(Func<T> getValue, Vector2 size, string name)
        {
            this.getValue = getValue;
            GameObject = new GameObject(name);
            RectTransform = GameObject.GetOrAddComponent<RectTransform>();
            RectTransform.sizeDelta = size;
        }
        public void Update()
        {
            if(getValue != null)
            {
                Value = getValue();
            }
            
            if(valueChanged)
            {
                Refresh();
            }
        }

        protected abstract void Refresh();
    }
}
