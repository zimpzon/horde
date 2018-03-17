using HordeEngine;
using UnityEngine;

public class MapRenderer : MonoBehaviour, IComponentUpdate
{
    public Material FloorMaterial;
    public Material WallMaterial;
    public float OffsetY;
    Transform trans_;

    DisplayMap displayMap_ = new DisplayMap(MapConstants.ChunkW, MapConstants.ChunkH, MapConstants.TileW, MapConstants.TileH);

    void Awake()
    {
        trans_ = this.transform;
    }

    public void SetMap(LogicalMap mapData)
    {
        displayMap_.SetMap(mapData);
    }

    void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late); }
    void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late); }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        displayMap_.DrawMap(FloorMaterial, WallMaterial, trans_.position.z, OffsetY);
    }
}
