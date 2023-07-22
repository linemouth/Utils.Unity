using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public class MeshData
    {
        public string name;
        public Vector3[] vertices;
        public int[] triangles;
        public Vector3[] normals;
        public Vector4[] tangents;
        public Vector2[] uv1;
        public Vector2[] uv2;
        public Vector2[] uv3;
        public Vector2[] uv4;
        public Vector2[] uv5;
        public Vector2[] uv6;
        public Vector2[] uv7;
        public Vector2[] uv8;

        public MeshData(string name, Vector3[] vertices, int[] triangles, Vector3[] normals = null, Vector4[] tangents = null, Vector2[] uv1 = null, Vector2[] uv2 = null, Vector2[] uv3 = null, Vector2[] uv4 = null, Vector2[] uv5 = null, Vector2[] uv6 = null, Vector2[] uv7 = null, Vector2[] uv8 = null)
        {
            this.name = name;
            this.vertices = vertices;
            this.triangles = triangles;
            this.normals = normals;
            this.tangents = tangents;
            this.uv1 = uv1;
            this.uv2 = uv2;
            this.uv3 = uv3;
            this.uv4 = uv4;
            this.uv5 = uv5;
            this.uv6 = uv6;
            this.uv7 = uv7;
            this.uv8 = uv8;
        }
        public MeshData(string name, int vertexCount, int triangleCount, bool hasTangents = false, bool hasUv1 = false, bool hasUv2 = false, bool hasUv3 = false, bool hasUv4 = false, bool hasUv5 = false, bool hasUv6 = false, bool hasUv7 = false, bool hasUv8 = false)
        {
            this.name = name;
            vertices = new Vector3[vertexCount];
            triangles = new int[triangleCount * 3];
            normals = new Vector3[vertexCount];
            tangents = hasTangents ? new Vector4[vertexCount] : null;
            uv1 = hasUv1 ? new Vector2[vertexCount] : null;
            uv2 = hasUv2 ? new Vector2[vertexCount] : null;
            uv3 = hasUv3 ? new Vector2[vertexCount] : null;
            uv4 = hasUv4 ? new Vector2[vertexCount] : null;
            uv5 = hasUv5 ? new Vector2[vertexCount] : null;
            uv6 = hasUv6 ? new Vector2[vertexCount] : null;
            uv7 = hasUv7 ? new Vector2[vertexCount] : null;
            uv8 = hasUv8 ? new Vector2[vertexCount] : null;
        }
        public static implicit operator bool(MeshData meshData) => meshData.vertices.Length > 0;
        public static explicit operator Mesh(MeshData meshData)
        {
            Mesh mesh = new Mesh();
            meshData.ApplyToMesh(mesh);
            return mesh;
        }
        public MeshData Clone() => new MeshData(name + "_copy", (Vector3[])vertices.Clone(), (int[])triangles.Clone(), (Vector3[])normals.Clone(), (Vector4[])tangents?.Clone(), (Vector2[])uv1?.Clone(), (Vector2[])uv2?.Clone(), (Vector2[])uv3?.Clone(), (Vector2[])uv4?.Clone(), (Vector2[])uv5?.Clone(), (Vector2[])uv6?.Clone(), (Vector2[])uv7?.Clone(), (Vector2[])uv8?.Clone());
        public void ApplyToMesh(Mesh mesh)
        {
            mesh.name = name;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.tangents = tangents;
            mesh.uv = uv1;
            mesh.uv2 = uv2;
            mesh.uv3 = uv3;
            mesh.uv4 = uv4;
            mesh.uv5 = uv5;
            mesh.uv6 = uv6;
            mesh.uv7 = uv7;
            mesh.uv8 = uv8;
        }
        public MeshData Append(MeshData other)
        {
            int count = vertices.Length;
            int otherCount = other.vertices.Length;
            return new MeshData(name,
                MergeArrays(vertices, other.vertices, count, otherCount),
                triangles.Concat(other.triangles.Select(index => index + count)).ToArray(),
                MergeArrays(normals, other.normals, count, otherCount),
                MergeArrays(tangents, other.tangents, count, otherCount),
                MergeArrays(uv1, other.uv1, count, otherCount),
                MergeArrays(uv2, other.uv2, count, otherCount),
                MergeArrays(uv3, other.uv3, count, otherCount),
                MergeArrays(uv4, other.uv4, count, otherCount),
                MergeArrays(uv5, other.uv5, count, otherCount),
                MergeArrays(uv6, other.uv6, count, otherCount),
                MergeArrays(uv7, other.uv7, count, otherCount),
                MergeArrays(uv8, other.uv8, count, otherCount)
            );
        }
        public override string ToString() => $"{{MeshData: \"{name}\", vertices: {vertices?.Length ?? 0}, triangles: {triangles?.Length / 3 ?? 0}}}";

        private static T[] MergeArrays<T>(T[] a, T[] b, int countA, int countB)
        {
            if(a == null && b != null)
            {
                a = new T[countA];
            }
            else if(a != null && b == null)
            {
                b = new T[countB];
            }
            return (a == null || b == null) ? null : a.Concat(b).ToArray();
        }
    }
}
