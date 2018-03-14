using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    // Optimization:
    // It might be worth it to manage own arrays since this is called so often.
    public class DynamicQuadMesh
    {
        Mesh mesh_ = new Mesh();

        List<Vector3> vertices_;
        List<Vector2> UV_;
        List<int> indices_;
        List<Color> colors_;

        public DynamicQuadMesh(int initialCapacity)
        {
            const int VerticesPerQuad = 4;
            const int IndicesPerQuad = 6;
            vertices_ = new List<Vector3>(initialCapacity * VerticesPerQuad);
            UV_ = new List<Vector2>(initialCapacity * VerticesPerQuad);
            colors_ = new List<Color>(initialCapacity * VerticesPerQuad);
            indices_ = new List<int>(initialCapacity * IndicesPerQuad);
        }

        public int QuadCount()
        {
            return vertices_.Count / 4;
        }

        public void Clear()
        {
            vertices_.Clear();
            UV_.Clear();
            colors_.Clear();
            indices_.Clear();
        }

        public void ApplyChanges()
        {
            mesh_.Clear();
            mesh_.SetVertices(vertices_);
            mesh_.SetUVs(0, UV_);
            mesh_.SetColors(colors_);
            mesh_.SetTriangles(indices_, 0);
        }

        public Mesh GetMesh()
        {
            return mesh_;
        }

        public void AddQuad(Vector3 center, float w, float h, float rotationDegrees, float topZSkew, Color color)
        {
            AddQuad(center, w, h, rotationDegrees, topZSkew, Vector2.up, 1.0f, 1.0f, color);
        }

        public void AddQuad(Vector3 center, float w, float h, float rotationDegrees, float topZSkew, Vector2 UVTopLeft, float uvW, float uvH, Color color)
        {
            // 0---1
            // | / | = [0, 1, 3] and [1, 2, 3]
            // 3---2

            // TODO: Rotation. Remember z skewing. SKEWING MIGHT also be doable in vertex shader? Just need to know base of object.
            float halfW = w * 0.5f;
            float halfH = h * 0.5f;
            vertices_.Add(new Vector3(center.x - halfW, center.y + halfH, center.z + topZSkew));
            vertices_.Add(new Vector3(center.x + halfW, center.y + halfH, center.z + topZSkew));
            vertices_.Add(new Vector3(center.x + halfW, center.y - halfH, center.z));
            vertices_.Add(new Vector3(center.x - halfW, center.y - halfH, center.z));

            UV_.Add(UVTopLeft);
            UV_.Add(new Vector2(UVTopLeft.x + uvW, UVTopLeft.y));
            UV_.Add(new Vector2(UVTopLeft.x + uvW, UVTopLeft.y - uvH));
            UV_.Add(new Vector2(UVTopLeft.x, UVTopLeft.y - uvH));

            colors_.Add(color);
            colors_.Add(color);
            colors_.Add(color);
            colors_.Add(color);

            int idx0 = vertices_.Count - 4;
            indices_.Add(idx0 + 0);
            indices_.Add(idx0 + 1);
            indices_.Add(idx0 + 3);

            indices_.Add(idx0 + 1);
            indices_.Add(idx0 + 2);
            indices_.Add(idx0 + 3);
        }
    }
}
