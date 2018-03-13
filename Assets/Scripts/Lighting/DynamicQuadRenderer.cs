using HordeEngine;
using UnityEngine;

public class DynamicQuadRenderer : MonoBehaviour
{
    public Material Material;
    [SerializeField, Layer] public LayerMask Layer;
    public int InitialCapacity = 100;

    [Header("Debug")]
    public int LatestQuadsRendered;

    [System.NonSerialized] public DynamicQuadMesh Mesh;

    int layerNumber_;

    void Awake()
    {
        Mesh = new DynamicQuadMesh(InitialCapacity);
        layerNumber_ = Layer.value;
    }

    public void LateUpdate()
    {
        Mesh.ApplyChanges();
        LatestQuadsRendered = Mesh.QuadCount();

        Matrix4x4 matrix = Matrix4x4.identity;
        matrix.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Graphics.DrawMesh(Mesh.GetMesh(), matrix, Material, layerNumber_);

        Mesh.Clear();
    }
}
