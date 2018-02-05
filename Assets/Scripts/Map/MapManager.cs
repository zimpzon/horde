using HordeEngine;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    const int ChunkW = 16;
    const int ChunkH = 16;

    VirtualMap map_ = new VirtualMap(Constants.ChunkW, Constants.ChunkH, Constants.TileW, Constants.TileH, Constants.LightmapResolution);

    public void SetMap(MapData mapData)
    {
        map_.SetMap(mapData);
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
