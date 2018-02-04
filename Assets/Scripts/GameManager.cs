using HordeEngine;
using System.Collections;
using UnityEngine;

enum GameState { Starting, StartScreen, InHub };

// 1) Do logical layout
// 2) Define physical bounds
// 3) Write tiles
// 4) Create meshes

public class GameManager : MonoBehaviour
{
    public MapRenderer MapRenderer;

    TileMapMetadata tilemapMetaData_;
    RoomWrapper roomWrapper_;
    MapData mapData_ = new MapData();
    VirtualMap currentMap_;

    public void Start()
    {
        StartCoroutine(GameStateLoop());
    }

    public void LoadMapResources()
    {
        var jsonTilemetaData = Resources.Load(@"TileMetadata\TileMetadata");
        tilemapMetaData_ = JsonUtility.FromJson<TileMapMetadata>(jsonTilemetaData.ToString());
        tilemapMetaData_.UpdateInferredValues();

        var jsonRooms = Resources.Load(@"Rooms\Rooms");
        roomWrapper_ = JsonUtility.FromJson<RoomWrapper>(jsonRooms.ToString());
    }

    IEnumerator GameStateLoop()
    {
        // One-time startup
        LoadMapResources();

        MapBuilderSingleRoom.Build(roomWrapper_.Rooms[0], mapData_);
        MapRenderer.SetMap(mapData_);

        //        MapUtil.TilesToPng(@"d:\temp\HordeDebug\walls.png", mapData_.walls, mapData_.mapBounds, mapData_.stride);

        while (true)
        {
            yield return null;
        }
    }
}
