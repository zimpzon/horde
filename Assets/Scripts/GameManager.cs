using HordeEngine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    TileMapMetadata tilemapMetaData_;
    RoomWrapper roomWrapper_;

    public void Start()
    {
        LoadMapResources();
    }

    public void LoadMapResources()
    {
        var jsonTilemetaData = Resources.Load(@"TileMetadata\TileMetadata");
        var jsonRooms = Resources.Load(@"Rooms\Rooms");
    }
}
