using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStat
{
    public GameObject GameObject { get; }
    public RectTransform RectTransform { get; }

    public void Update();
}
