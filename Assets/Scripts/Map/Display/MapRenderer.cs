using HordeEngine;
using UnityEngine;

public class MapRenderer : MonoBehaviour, IComponentUpdate
{
    public Material FloorMaterial;
    public Material WallMaterial;
    public Material AmbientOcclusionMaterial;
    public bool EnableAmbientOcclusion = true;

    Transform trans_;

    DisplayMap displayMap_ = new DisplayMap(MapConstants.ChunkW, MapConstants.ChunkH, MapConstants.TileW, MapConstants.TileH, MapConstants.LightmapResolution);

    void Awake()
    {
        trans_ = this.transform;
    }

    public void SetMap(LogicalMap mapData)
    {
        displayMap_.SetMap(mapData);
    }

    public void DrawMap()
    {
        displayMap_.DrawMap(FloorMaterial, WallMaterial, AmbientOcclusionMaterial, EnableAmbientOcclusion, trans_.position.z);
    }

    void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late); }
    void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late); }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        DrawMap();
    }
}
