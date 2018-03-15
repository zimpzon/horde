using System.Linq;

namespace HordeEngine
{
    public class GameStateHub : GameStateHandler
    {
        LogicalMap hubMap_;

        void EnsureHubMapCreated()
        {
            if (hubMap_ == null)
            {
                hubMap_ = new LogicalMap();
                var hubRoom = Global.MapResources.Rooms.Where(r => r.Name == "hub").First();
                MapBuilderSingleRoom.Build(hubRoom, hubMap_);
            }
        }

        public override bool TryEnterState()
        {
            EnsureHubMapCreated();

            Global.SceneAccess.MiniMap.SetMap(hubMap_);
            Global.SetMap(hubMap_);
            return true;
        }
        
        public override void UpdateState(bool hasFocus)
        {
        }
    }
}
