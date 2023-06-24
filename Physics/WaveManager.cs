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
    public float gridSize;

    protected MeshRenderer meshRenderer;
    protected MeshFilter meshFilter;

    public float SampleHeight(Vector3 position)
    {
        float phase = Vector3.Dot(direction, position);
        float cycle = (float)Math.Repeat(Time.timeAsDouble, period) / period;
        return amplitude * Mathf.Sin(cycle / length + offset + phase);
    }

    public void Awake()
    {
        meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        if(mesh != null)
        {
            meshFilter.sharedMesh = mesh;
        }
    }
#if UNITY_EDITOR
    public void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall -= RecalculateMesh;
        UnityEditor.EditorApplication.delayCall += RecalculateMesh;
    }

    private void RecalculateMesh()
    {
        UnityEditor.EditorApplication.delayCall -= RecalculateMesh;
        mesh = (Mesh)Geometry.GetPlane(new BasisVectors(Direction.Up, size), Vector3.zero, Math.RoundToInt(size.x / gridSize), Math.RoundToInt(size.y / gridSize));
        if(meshFilter != null)
        {
            meshFilter.sharedMesh = mesh;
        }
    }
#endif
}
