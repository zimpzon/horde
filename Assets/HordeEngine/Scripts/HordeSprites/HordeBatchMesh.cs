using System;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// Fixed size mesh. Triangles that are not in use will be rendered as degenerate (empty) by setting all vertices to 0.
    /// </summary>
    [Serializable]
    public class HordeBatchMesh
    {
        public Mesh Mesh;

        public int Capacity;
        public int ActiveQuadCount;

        Vector3[] vertices_;
        Vector2[] UV_;
        int[] indices_;
        Color32[] colors_;

        int idxAlreadyZeroed_;

        const int VerticesPerQuad = 4;
        const int IndicesPerQuad = 6;

        public HordeBatchMesh(int capacity)
        {
            Capacity = capacity;

            Mesh = new Mesh();
            Mesh.MarkDynamic();
            Mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000);

            vertices_ = new Vector3[capacity * VerticesPerQuad];
            UV_ = new Vector2[capacity * VerticesPerQuad];
            colors_ = new Color32[capacity * VerticesPerQuad];
            indices_ = new int[capacity * IndicesPerQuad];
            InitializeIndices();

            // All vertices are zeroed to begin with
            idxAlreadyZeroed_ = 0;

            ApplyChanges();

            // This happens only once
            Mesh.SetTriangles(indices_, 0);
//            Mesh.triangles = indices_;
            Mesh.UploadMeshData(false);
        }

        void InitializeIndices()
        {
            for (int i = 0; i < indices_.Length; i += IndicesPerQuad)
            {
                indices_[i + 0] = 0;
                indices_[i + 1] = 1;
                indices_[i + 2] = 3;

                indices_[i + 3] = 1;
                indices_[i + 4] = 2;
                indices_[i + 5] = 3;
            }
        }

        public void Clear()
        {
            ActiveQuadCount = 0;
        }

        public void ApplyChanges()
        {
            ZeroVertices(ActiveQuadCount, idxAlreadyZeroed_ - 1);
            idxAlreadyZeroed_ = ActiveQuadCount;

            Mesh.vertices = vertices_;
            Mesh.uv = UV_;
            Mesh.colors32 = colors_;
            Mesh.UploadMeshData(false);
        }

        void ZeroVertices(int first, int last)
        {
            for (int i = first; i <= last; ++i)
            {
                vertices_[i].x = 0.0f;
                vertices_[i].y = 0.0f;
                vertices_[i].z = 0.0f;
            }
        }

        public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Color color)
        {
            AddQuad(center, size, rotationDegrees, zSkew, Vector2.up, Vector2.one, color);
        }

        public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Vector2 UVTopLeft, Vector2 uvSize, Color color)
        {
            zSkew = 0;
            // 0---1
            // | / | = [0, 1, 3] and [1, 2, 3]
            // 3---2

            float halfW = size.x * 0.5f;
            float halfH = size.y * 0.5f;
            float halfSkew = zSkew * 0.5f;

            int vert0 = ActiveQuadCount * 4;
            vertices_[vert0 + 0].x = center.x - halfW;
            vertices_[vert0 + 1].x = center.x + halfW;
            vertices_[vert0 + 2].x = center.x + halfW;
            vertices_[vert0 + 3].x = center.x - halfW;

            vertices_[vert0 + 0].y = center.y + halfH;
            vertices_[vert0 + 1].y = center.y + halfH;
            vertices_[vert0 + 2].y = center.y - halfH;
            vertices_[vert0 + 3].y = center.y - halfH;

            vertices_[vert0 + 0].z = center.z - halfSkew;
            vertices_[vert0 + 1].z = center.z - halfSkew;
            vertices_[vert0 + 2].z = center.z + halfSkew;
            vertices_[vert0 + 3].z = center.z + halfSkew;

            float duration = 0;
            Debug.DrawLine(vertices_[vert0 + 0], vertices_[vert0 + 1], Color.green, duration);
            Debug.DrawLine(vertices_[vert0 + 1], vertices_[vert0 + 3], Color.green, duration);
            Debug.DrawLine(vertices_[vert0 + 3], vertices_[vert0 + 0], Color.green, duration);
            Debug.DrawLine(vertices_[vert0 + 1], vertices_[vert0 + 2], Color.green, duration);
            Debug.DrawLine(vertices_[vert0 + 2], vertices_[vert0 + 3], Color.green, duration);

            UV_[vert0 + 0].x = UVTopLeft.x;
            UV_[vert0 + 0].y = UVTopLeft.y;

            UV_[vert0 + 1].x = UVTopLeft.x + uvSize.x;
            UV_[vert0 + 1].y = UVTopLeft.y;

            UV_[vert0 + 2].x = UVTopLeft.x + uvSize.x;
            UV_[vert0 + 2].y = UVTopLeft.y - uvSize.y;

            UV_[vert0 + 3].x = UVTopLeft.x;
            UV_[vert0 + 3].y = UVTopLeft.y - uvSize.y;

            colors_[vert0 + 0] = color;
            colors_[vert0 + 1] = color;
            colors_[vert0 + 2] = color;
            colors_[vert0 + 3] = color;

            ActiveQuadCount++;
        }
    }
}
