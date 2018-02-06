using UnityEngine;
using System.Linq;

namespace HordeEngine
{
    public class GameStateHub : GameStateHandler
    {
        MapData hubData_;

        void EnsureHubData()
        {
            if (hubData_ == null)
            {
                hubData_ = new MapData();
                var hubRoom = Global.MapResources.Rooms.Where(r => r.Name == "hub").First();
                MapBuilderSingleRoom.Build(hubRoom, hubData_);
            }
        }

        public override bool TryEnterState()
        {
            return true;
        }
    }
}
