using System;
using UnityEngine;

namespace HordeEngine
{
    public class GameStateBoot : GameStateHandler
    {
        public override void UpdateState(bool hasFocus)
        {
            try
            {
                LoadMapResources();
            }
            catch (Exception e)
            {
                Global.GameManager.FatalError(e.Message);
            }

            JumpToState(GameState.InHub);
        }

        void LoadMapResources()
        {
            var jsonTilemetaData = Resources.Load(@"TileMetadata\TileMetadata");
            Global.MapResources.TilemapMetaData = JsonUtility.FromJson<TileMapMetadata>(jsonTilemetaData.ToString());
            Global.MapResources.TilemapMetaData.CreatePropertyLookup();

            var jsonRooms = Resources.Load(@"Rooms\Rooms");
            var roomWrapper = JsonUtility.FromJson<RoomWrapper>(jsonRooms.ToString());
            Global.MapResources.Rooms = roomWrapper.Rooms;
        }
    }
}
