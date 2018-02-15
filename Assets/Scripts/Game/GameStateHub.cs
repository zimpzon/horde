using System.Linq;

namespace HordeEngine
{
    public class GameStateHub : GameStateHandler
    {
        LogicalMap hubData_;

        void EnsureHubData()
        {
            if (hubData_ == null)
            {
                hubData_ = new LogicalMap();
                var hubRoom = Global.MapResources.Rooms.Where(r => r.Name == "hub").First();
                MapBuilderSingleRoom.Build(hubRoom, hubData_);
            }
        }

        public override bool TryEnterState()
        {
            EnsureHubData();

            Global.MapRenderer.SetMap(hubData_);
            return true;
        }

        public override void UpdateState(bool hasFocus)
        {
            Global.MapRenderer.DrawMap();
        }
    }
}
