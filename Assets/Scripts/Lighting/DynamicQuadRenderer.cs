using HordeEngine;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicQuadRenderer : MonoBehaviour
{
    public Material Material;
    [SerializeField, Layer] public LayerMask Layer;
    public int InitialCapacity = 100;

    [Header("Debug")]
    public int QuadCount;

    public DynamicQuadMesh QuadMesh { get { return GetQuadMesh(); } }

    DynamicQuadMesh quadMesh_;

    DynamicQuadMesh GetQuadMesh()
    {
        EnsureInit();
        return quadMesh_;
    }

    void EnsureInit()
    {
        if (quadMesh_ == null)
            quadMesh_ = new DynamicQuadMesh(InitialCapacity);
    }

    public void LateUpdate()
    {
        EnsureInit();

        quadMesh_.ApplyChanges();
        QuadCount = quadMesh_.QuadCount();

        Matrix4x4 matrix = Matrix4x4.identity;
        matrix.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Graphics.DrawMesh(QuadMesh.GetMesh(), matrix, Material, Layer.value);

        QuadMesh.Clear();
    }
}
