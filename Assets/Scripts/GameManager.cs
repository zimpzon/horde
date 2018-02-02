using HordeEngine;
using System.Collections;
using UnityEngine;

enum GameState { Starting, StartScreen, InHub };

public class GameManager : MonoBehaviour
{
    TileMapMetadata tilemapMetaData_;
    RoomWrapper roomWrapper_;
    MapData mapData_;

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
        LoadMapResources();

        while (true)
        {
            yield break;
        }
    }
}
