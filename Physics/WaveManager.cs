using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class WaveManager : MonoBehaviour
{
    public Vector3 direction;
    public float amplitude = 1;
    public float length = 2;
    public float period = 1;
    public float offset = 0;
    public Mesh mesh;
    public Vector2 size;

    public float SampleHeight(Vector3 position)
    {
        float phase = Vector3.Dot(direction, position);
        float cycle = (float)Math.Repeat(Time.timeAsDouble, period) / period;
        return amplitude * Mathf.Sin(cycle / length + offset);
    }

    public void OnStart()
    {
        MeshRenderer meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh = (Mesh)Geometry.GetPlane(new BasisVectors(Direction.Up, size), Vector3.zero, 50);
    }
}
