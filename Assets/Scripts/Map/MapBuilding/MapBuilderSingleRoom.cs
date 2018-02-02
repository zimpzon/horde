using UnityEngine;

namespace HordeEngine
{
    public class MapBuilderSingleRoom : MapBuilderBase
    {
        public void Build(Room room, MapData mapDst)
        {
            const int Margin = 2;
            mapDst.margin = Margin;
            mapDst.mapBounds = new BoundsInt(0, 0, 0, room.Width + Margin * 2, room.Height + Margin * 2, 0);
            mapDst.stride = mapDst.mapBounds.size.x;
            mapDst.EnsureSizeFromBounds();

            PlaceRoom(room, new Vector3Int(Margin, Margin, 0), mapDst);
        }
    }
}
