using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Geometry
{
    public static Vector2 Normalized(Vector2 v)
    {
        float sqrMag = v.sqrMagnitude;
        if(sqrMag != 1)
        {
            v /= sqrMag;
        }
        return v;
    }
    public static Vector3 Normalized(Vector3 v)
    {
        float sqrMag = v.sqrMagnitude;
        if(sqrMag != 1)
        {
            v /= sqrMag;
        }
        return v;
    }
    public static double GetLongitude(Vector3 position) => Math.Atan2(position.z, position.x) * Mathf.Rad2Deg;
    public static double GetLatitude(Vector3 position) => Math.Atan2(position.y, Math.Sqrt(position.x * position.x + position.z * position.z)) * Mathf.Rad2Deg;
    public static MeshData GetQuad(BasisVectors basisVectors, Vector3 center = default)
    {
        center -= 0.5f * (basisVectors.x + basisVectors.y);
        Vector3[] vertices = new Vector3[4] { center, center + basisVectors.x, center + basisVectors.x + basisVectors.y, center + basisVectors.y };
        Vector3[] normals = new Vector3[4] { basisVectors.normal, basisVectors.normal, basisVectors.normal, basisVectors.normal };
        Vector2[] uv1 = new Vector2[4] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        int[] indices = new int[] { 0, 2, 1, 0, 3, 2 };
        return new MeshData("Quad", vertices, indices, normals, null, uv1);
    }
    public static MeshData GetPlane(BasisVectors basisVectors, Vector3 center, int quadCount) => GetPlane(basisVectors, center, quadCount, quadCount);
    public static MeshData GetPlane(BasisVectors basisVectors, Vector3 center, int xQuadCount, int yQuadCount)
    {
        int xPointCount = 1 + xQuadCount;
        int yPointCount = 1 + yQuadCount;
        int pointCount = xPointCount * yPointCount;
        Vector3[] vertices = new Vector3[pointCount];
        Vector3[] normals = new Vector3[pointCount];
        Vector2[] uv1 = new Vector2[pointCount];

        int index = 0;
        Vector3 y = (1 - yPointCount) * basisVectors.y / 2;
        for(int yi = 0; yi < yPointCount; ++yi, y += basisVectors.y)
        {
            Vector3 x = (1 - xPointCount) * basisVectors.x / 2;
            for(int xi = 0; xi < xPointCount; ++xi, x += basisVectors.x)
            {
                vertices[index] = center + x + y;
                normals[index] = basisVectors.normal;
                uv1[index] = new Vector2((float)xi / xPointCount, (float)yi / yPointCount);
                ++index;
            }
        }
        return new MeshData("Plane", vertices, GetPlaneIndices(xQuadCount, yQuadCount), normals, null, uv1);
    }
    // This will make a cube which looks smooth.
    // To make a cube with flat faces, it will need 4 vertices for each face.
    public static MeshData GetCube(Vector3 center, Vector3 scale)
    {
        Vector3[] vertices = new Vector3[24];
        Vector3[] normals = new Vector3[24];
        Vector2[] uv1 = new Vector2[24];
        int[] indices = new int[36];
        Vector3 dx = new Vector3(0.5f * scale.x, 0, 0);
        Vector3 dy = new Vector3(0, 0.5f * scale.y, 0);
        Vector3 dz = new Vector3(0, 0, 0.5f * scale.z);

        CalculateCubeFace(vertices, normals, uv1, indices, 0,  dx, -dz, -dy); // Bottom
        CalculateCubeFace(vertices, normals, uv1, indices, 1,  dx,  dy, -dz); // Back
        CalculateCubeFace(vertices, normals, uv1, indices, 2,  dz,  dy,  dx); // Right
        CalculateCubeFace(vertices, normals, uv1, indices, 3, -dx,  dy,  dz); // Front
        CalculateCubeFace(vertices, normals, uv1, indices, 4, -dz,  dy, -dx); // Left
        CalculateCubeFace(vertices, normals, uv1, indices, 5,  dx,  dz,  dy); // Top

        return new MeshData("Plane", vertices, indices, normals, null, uv1);
    }
    public static MeshData GetIcosahedron()
    {
        // The following numbers represent 1 and the golden ratio normalized about their orthogonal length. (m^2 + g^2 = 1)
        float m = 0.525731112f; // Normalized 1
        float g = 0.850650808f; // Normalized golden ratio
        Vector3[] vertices = new Vector3[]
        {
            new Vector3( 0,  g,  m),
            new Vector3( 0,  g, -m),
            new Vector3( 0, -g,  m),
            new Vector3( 0, -g, -m),
            new Vector3( m,  0,  g),
            new Vector3(-m,  0,  g),
            new Vector3( m,  0, -g),
            new Vector3(-m,  0, -g),
            new Vector3( g,  m,  0),
            new Vector3( g, -m,  0),
            new Vector3(-g,  m,  0),
            new Vector3(-g, -m,  0),
        };
        int[] triangles = new int[60]
        {
             0,  1,  8,
             0, 10,  1,
            10,  7,  1,
             1,  7,  6,
             6,  8,  1,
             6,  9,  8,
             8,  9,  4,
             8,  4,  0,
             4,  5,  0,
             5, 10,  0,
            10,  5, 11,
            11,  7, 10,
             7, 11,  3,
             7,  3,  6,
             9,  6,  3,
             9,  3,  2,
             4,  9,  2,
             5,  4,  2,
            11,  5,  2,
             3, 11,  2,
        };

        Vector2[] uv1 = new Vector2[12];
        for(int i = 0; i < vertices.Length; ++i)
        {
            uv1[i].x = (float)(GetLongitude(vertices[i]) / 360 + 0.5);
            uv1[i].y = (float)(GetLatitude(vertices[i]) / 180 + 0.5);
        }
        return new MeshData("Icosahedron", vertices, triangles, (Vector3[])vertices.Clone(), null, uv1);
    }
    /// <summary>
    /// Divides a triangle into equal smaller triangles. Use 2 - 360 for 16-bit vertex meshes.
    /// </summary>
    /// <param name="corners">Position of original triangle corners</param>
    /// <param name="divisions">Divisor for each edge of the triangle</param>
    public static MeshData DivideTriangle(Vector3[] corners, int divisions)
    {
        Vector3 a = corners[0];
        Vector3 b = corners[1];
        Vector3 c = corners[2];
        Vector3 x = (b - a) / divisions;
        Vector3 y = (c - b) / divisions;

        // Calculate vertices
        MeshData meshData = new MeshData("Triangle", (divisions + 1) * (divisions + 2) / 2, divisions * divisions * 3);
        for(int i = 0; i <= divisions; ++i)
        {
            int rowStartingIndex = i * (i + 1) / 2;
            for(int j = 0; j <= i; ++j)
            {
                meshData.vertices[rowStartingIndex + j] = a + i * x + j * y;
            }
        }
        GetTriangleIndices(meshData.triangles);
        return meshData;
    }
    public static MeshData DivideTriangleUnitSphere(Vector3[] corners, int powerOfTwoDivisions)
    {
        int divisions = (int)Math.Pow(2, powerOfTwoDivisions);
        MeshData meshData = new MeshData("Triangle Sphere Section", (divisions + 1) * (divisions + 2) / 2, divisions * divisions);
        {
            int lastRowIndex = divisions * (divisions + 1) / 2;
            meshData.vertices[0] = corners[0];
            meshData.vertices[lastRowIndex] = corners[1];
            meshData.vertices[lastRowIndex + divisions] = corners[2];
        }

        for(int currentDivisions = 1; currentDivisions < divisions; currentDivisions *= 2)
        {
            int stride = divisions / currentDivisions;
            int halfStride = stride / 2;

            for(int i = 0; i < divisions; i += stride)
            {
                int upperRowStartingIndex = i * (i + 1) / 2;
                int middleRowStartingIndex = (i + halfStride) * (i + halfStride + 1) / 2;
                int lowerRowStartingIndex = (i + stride) * (i + stride + 1) / 2;
                for(int j = 0; j <= i; j += stride)
                {
                    int indexA = upperRowStartingIndex + j;
                    int indexB = lowerRowStartingIndex + j;
                    int indexC = lowerRowStartingIndex + j + stride;
                    int indexAB = middleRowStartingIndex + j;
                    int indexAC = middleRowStartingIndex + j + halfStride;
                    int indexBC = lowerRowStartingIndex + j + halfStride;

                    Vector3 a = meshData.vertices[indexA];
                    Vector3 b = meshData.vertices[indexB];
                    Vector3 c = meshData.vertices[indexC];
                    Vector3 x = (b - a) / 2f;
                    Vector3 y = (c - b) / 2f;
                    meshData.vertices[indexAB] = (a + x).normalized;
                    meshData.vertices[indexAC] = (a + x + y).normalized;
                    meshData.vertices[indexBC] = (b + y).normalized;
                }
            }
        }

        GetTriangleIndices(meshData.triangles);
        return meshData;
    }
    public static void CalculateCubeFace(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int[] indices, int faceIndex, Vector3 dx, Vector3 dy, Vector3 dz)
    {
        int vertexOffset = faceIndex * 4;
        int indexOffset = faceIndex * 6;
        Vector3 normal = dz.normalized;

        // Vertices
        vertices[vertexOffset + 0] = dz - dx - dy;
        vertices[vertexOffset + 1] = dz + dx - dy;
        vertices[vertexOffset + 2] = dz + dx + dy;
        vertices[vertexOffset + 3] = dz - dx + dy;

        // Normals
        normals[vertexOffset + 0] = normal;
        normals[vertexOffset + 1] = normal;
        normals[vertexOffset + 2] = normal;
        normals[vertexOffset + 3] = normal;

        // UVs
        uvs[vertexOffset + 0] = new Vector2(0, 0);
        uvs[vertexOffset + 1] = new Vector2(1, 0);
        uvs[vertexOffset + 2] = new Vector2(1, 1);
        uvs[vertexOffset + 3] = new Vector2(0, 1);

        // Indices
        indices[vertexOffset + 0] = indexOffset + 0;
        indices[vertexOffset + 1] = indexOffset + 2;
        indices[vertexOffset + 2] = indexOffset + 3;
        indices[vertexOffset + 3] = indexOffset + 0;
        indices[vertexOffset + 4] = indexOffset + 1;
        indices[vertexOffset + 5] = indexOffset + 2;
    }
    public static bool IsInsideConvexPolygon(IEnumerable<Vector2> vertices, Vector2 point)
    {
        Vector2 previousVertex = vertices.Last();
        int netCount = 0;

        foreach(Vector2 vertex in vertices)
        {
            Vector2 edge = vertex - previousVertex;
            Vector2 toPoint = point - previousVertex;
            float cross = edge.Cross(toPoint);
            int sign = Math.Sign(cross);
            netCount += sign;
            previousVertex = vertex;
        }

        return Math.Abs(netCount) == vertices.Count();
    }
    public static Vector2 ToPlanarPoint(Vector3 point, BasisVectors basisVectors) => new Vector2(Vector3.Dot(point, basisVectors.x), Vector3.Dot(point, basisVectors.y));
    public static Vector3 FromPlanarPoint(Vector2 point, BasisVectors basisVectors) => (point.x * basisVectors.x) + (point.y * basisVectors.y);

    private static void GetTriangleIndices(int[] indices)
    {
        // Assign triangles
        int triangleIndex = 0;
        int divisions = Mathf.RoundToInt(Mathf.Sqrt(indices.Length / 3));
        for(int i = 0; i < divisions; ++i)
        {
            int upperRowStartingIndex = i * (i + 1) / 2;
            int lowerRowStartingIndex = (i + 1) * (i + 2) / 2;
            for(int j = 0; j <= i; ++j)
            {
                //Debug.Log($"u:{upperRowStartingIndex}, l:{lowerRowStartingIndex}, ji:{j}/{i}, t:{triangleIndex}/{indices.Length}");
                indices[triangleIndex + 0] = upperRowStartingIndex + j;
                indices[triangleIndex + 1] = lowerRowStartingIndex + j;
                indices[triangleIndex + 2] = lowerRowStartingIndex + j + 1;
                triangleIndex += 3;
                if(j < i)
                {
                    indices[triangleIndex + 0] = upperRowStartingIndex + j;
                    indices[triangleIndex + 1] = lowerRowStartingIndex + j + 1;
                    indices[triangleIndex + 2] = upperRowStartingIndex + j + 1;
                    triangleIndex += 3;
                }
            }
        }
    }
    private static int[] GetPlaneIndices(int xQuadCount, int yQuadCount)
    {
        int xPointCount = 1 + xQuadCount;
        int yPointCount = 1 + yQuadCount;
        int[] indices = new int[xQuadCount * yQuadCount * 6];
        int index = 0;
        for(int y = 0; y < yQuadCount; ++y)
        {
            int row1 = xPointCount * y;
            int row2 = xPointCount * (y + 1);
            for(int x = 0; x < xQuadCount; ++x)
            {
                indices[index    ] = row1;
                indices[index + 1] = row2 + 1;
                indices[index + 2] = row1 + 1;
                indices[index + 3] = row1;
                indices[index + 4] = row2;
                indices[index + 5] = row2 + 1;

                row1 += 1;
                row2 += 1;
                index += 6;
            }
        }
        return indices;
    }
}
