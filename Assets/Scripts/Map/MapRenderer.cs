using HordeEngine;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    public Material MapMaterial;

    DisplayMap displayMap_ = new DisplayMap(MapConstants.ChunkW, MapConstants.ChunkH, MapConstants.TileW, MapConstants.TileH, MapConstants.LightmapResolution);

    public void SetMap(LogicalMap mapData)
    {
        displayMap_.SetMap(mapData);
    }

    public void DrawMap()
    {
        displayMap_.DrawMap(MapMaterial);
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
