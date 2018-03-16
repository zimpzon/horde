using System;
using System.Collections.Generic;
using HordeEngine;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class QuadRenderer : MonoBehaviour
{
    public Material Material;
    [SerializeField, Layer] public LayerMask Layer;
    public Vector3 Offset = Vector3.zero;
    public int InitialCapacity = 100;

    [Header("Debug")]
    public int QuadCount;

    // texture + material => 1 renderer
    List<long> keys = new List<long>(10);
    List<DynamicQuadRenderer> renderers_ = new List<DynamicQuadRenderer>(10);

    [NonSerialized] public DynamicQuadMesh QuadMesh;
    public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Color color, Sprite sprite, Material material)
    {
        AddQuad(center, size, rotationDegrees, zSkew, Vector2.up, Vector2.one, color, sprite, material);
    }

    public void AddQuad(Vector3 center, Vector2 size, float rotationDegrees, float zSkew, Vector2 UVTopLeft, Vector2 uvSize, Color color, Sprite sprite, Material material)
    {
        var quadMesh = GetMesh(sprite, material);
    }

    private DynamicQuadMesh GetMesh(Sprite sprite, Material material)
    {
        long key = sprite.GetInstanceID() << 32 + material.GetInstanceID();
        int idx = keys.IndexOf(key);
        if (idx < 0)
            idx = AddQuadMesh(key);
        return null;
    }

    private int AddQuadMesh(long key)
    {
        var quadMesh = new DynamicQuadMesh(1);
            return 0;
    }

    void EnsureCreated()
    {
        if (QuadMesh == null)
            QuadMesh = new DynamicQuadMesh(InitialCapacity);
    }

    public void LateUpdate()
    {
        EnsureCreated();

        QuadMesh.ApplyChanges();
        QuadCount = QuadMesh.QuadCount();

        Matrix4x4 matrix = Matrix4x4.identity;
        matrix.SetTRS(Offset, Quaternion.identity, Vector3.one);
        Graphics.DrawMesh(QuadMesh.GetMesh(), matrix, Material, Layer.value);

        QuadMesh.Clear();
    }
}
