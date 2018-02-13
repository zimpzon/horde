using HordeEngine;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public Material MapMaterial;

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
        displayMap_.DrawMap(MapMaterial, trans_.position.z);
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
