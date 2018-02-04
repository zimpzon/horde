using UnityEngine;

namespace HordeEngine
{
    public static class MapBuilderSingleRoom
    {
        public static void Build(Room room, MapData mapDst)
        {
            const int Margin = 1;
            mapDst.Margin = Margin;
            mapDst.SetBounds(room.Width + Margin * 2, room.Height + Margin * 2, stride: room.Width + Margin * 2);
            mapDst.EnsureAllocatedSizeFromBounds();

            MapUtil.PlaceRoom(room, new Vector3Int(Margin, Margin, 0), mapDst);
        }
    }
}
