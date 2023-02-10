using System;
using UnityEngine;

public abstract class Stat<T> : IStat where T : IEquatable<T>
{
    public GameObject GameObject { get; private set; }
    public RectTransform RectTransform { get; private set; }
    protected T Value { get; private set; } = default;

    private Func<T> getValue;

    public Stat(Func<T> getValue, Vector2 size, string name)
    {
        this.getValue = getValue;
        GameObject = new GameObject(name);
        RectTransform = GameObject.GetOrAddComponent<RectTransform>();
        RectTransform.sizeDelta = size;
    }
    public void Update()
    {
        T newValue = getValue();
        if(newValue == null ? (Value != null) : (!newValue.Equals(Value)))
        {
            Value = newValue;
            Refresh();
        }
    }

    protected abstract void Refresh();
}
