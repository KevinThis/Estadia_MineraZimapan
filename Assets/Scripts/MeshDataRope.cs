using System.Collections.Generic;
using UnityEngine;

public class MeshDataRope
{
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Vector3[] bakedNormals;

    private int triangleIndex = 0;
    private bool useFlatShading;

    public MeshDataRope(int vertsPerCircle, int vertsAlongRope, bool useFlatShading)
    {
        this.useFlatShading = useFlatShading;

        vertices = new Vector3[vertsPerCircle * vertsAlongRope];
        uvs = new Vector2[vertices.Length];

        int numMainTriangles = (vertsPerCircle * (vertsAlongRope - 1)) * 2;
        triangles = new int[numMainTriangles * 3];
    }

    public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex)
    {
        vertices[vertexIndex] = vertexPosition;
        uvs[vertexIndex] = uv;
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex++] = a;
        triangles[triangleIndex++] = b;
        triangles[triangleIndex++] = c;
    }

    private Vector3[] CalculateNormals()
    {
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;

        for (int i = 0; i < triangleCount; i++)
        {
            int i0 = triangles[i * 3 + 0];
            int i1 = triangles[i * 3 + 1];
            int i2 = triangles[i * 3 + 2];

            Vector3 normal = SurfaceNormalFromIndices(i0, i1, i2);
            vertexNormals[i0] += normal;
            vertexNormals[i1] += normal;
            vertexNormals[i2] += normal;
        }

        for (int i = 0; i < vertexNormals.Length; i++)
        {
            vertexNormals[i].Normalize();
        }

        return vertexNormals;
    }

    private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
        Vector3 A = vertices[indexA];
        Vector3 B = vertices[indexB];
        Vector3 C = vertices[indexC];

        Vector3 sideAB = B - A;
        Vector3 sideAC = C - A;

        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public void ProcessMesh()
    {
        if (useFlatShading)
        {
            FlatShading();
        }
        else
        {
            bakedNormals = CalculateNormals();
        }
    }

    private void FlatShading()
    {
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUVs = new Vector2[triangles.Length];
        int[] flatShadedTriangles = new int[triangles.Length];

        for (int i = 0; i < triangles.Length; i++)
        {
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUVs[i] = uvs[triangles[i]];
            flatShadedTriangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUVs;
        triangles = flatShadedTriangles;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        if (useFlatShading)
        {
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.normals = bakedNormals;
        }

        return mesh;
    }

    public void ResetMesh(int vertsPerCircle, int vertsAlongRope, bool useFlatShading)
    {
        this.useFlatShading = useFlatShading;

        vertices = new Vector3[vertsPerCircle * vertsAlongRope];
        uvs = new Vector2[vertices.Length];

        int numMainTriangles = (vertsPerCircle * (vertsAlongRope - 1)) * 2;
        triangles = new int[numMainTriangles * 3];
        triangleIndex = 0;
    }

    public int[] GetTriangles()
    {
        return triangles;
    }
}
