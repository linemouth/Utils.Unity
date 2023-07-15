using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using Utils;

public class OceanRenderer : MonoBehaviour
{
    public Vector2 size = new Vector2(100, 100);
    public float resolution = 0.5f;
    [Range(1, 255)]
    public int cellCount = 100;
    public Material material;

    private Mesh mesh = null;
    private Matrix4x4[] matrices; // Array to store instance transforms
    private Bounds bounds;
    private ComputeBuffer argsBuffer = null;
    private ComputeBuffer matricesBuffer = null;

    public float SampleHeight(Vector2 position) => SampleHeight(new Vector3(position.x, 0, position.y));
    public float SampleHeight(Vector3 position) => 0;

    private void Setup()
    {
        // Creates a plane centered at zero, with "cellCount" quads of "resolution" size in the X and Z directions.
        mesh = (Mesh)Geometry.GetPlane(BasisVectors.Up * resolution, Vector3.zero, cellCount);

        // Calculate the bounds of the instances.
        float meshSize = resolution * cellCount;
        bounds = new Bounds(Vector3.zero, new Vector3(meshSize, 5.0f, meshSize));

        // Calculate instance count.
        int xInstanceCount = Math.Max(1, Mathf.CeilToInt(size.x / meshSize));
        int zInstanceCount = Math.Max(1, Mathf.CeilToInt(size.y / meshSize));
        int instanceCount = xInstanceCount * zInstanceCount;
        matrices = new Matrix4x4[instanceCount];

        // Set up indirect arguments.
        argsBuffer?.Dispose();
        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(new uint[]
        {
            mesh.GetIndexCount(0),
            (uint)instanceCount,
            mesh.GetIndexStart(0),
            mesh.GetBaseVertex(0),
            0 // Start instance index
        });

        // Calculate instance transforms.
        Matrix4x4 worldMatrix = transform.parent?.localToWorldMatrix ?? Matrix4x4.identity;
        int index = 0;
        float x = (1 - xInstanceCount) * meshSize / 2;
        for(float xi = 0; xi < xInstanceCount; ++xi, x += meshSize)
        {
            float z = (1 - zInstanceCount) * meshSize / 2;
            for(float zi = 0; zi < xInstanceCount; ++zi, z += meshSize)
            {
                Vector3 position = new Vector3(x, 0, z);
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = Vector3.one;
                Matrix4x4 localMatrix = Matrix4x4.TRS(position, rotation, scale);
                matrices[index] = worldMatrix * localMatrix;
                ++index;
            }
        }
        matricesBuffer?.Dispose();
        matricesBuffer = new ComputeBuffer(instanceCount, sizeof(float) * 16);
        matricesBuffer.SetData(matrices);
        material.SetBuffer("InstanceTransform", matricesBuffer);
    }
    private void RenderInstances()
    {
        if(mesh == null || argsBuffer == null || matricesBuffer == null)
        {
            Setup();
        }

        // Render the instances of the mesh using "material".
        Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer);
    }
    private void Awake()
    {
        Setup();
    }
    private void Update()
    {
        RenderInstances();
    }
    private void OnDisable()
    {
        matricesBuffer?.Dispose();
        argsBuffer?.Dispose();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        RenderInstances();

        if(Selection.gameObjects.Contains(gameObject))
        {
            Handles.matrix = transform.localToWorldMatrix;
            Handles.DrawWireCube(transform.position, new Vector3(size.x, 0, size.y));
        }
    }
    private void OnValidate()
    {
        Setup();
        RenderInstances();
    }
#endif
}
