using UnityEngine;

namespace HordeEngine
{
    public static class MapBuilderSingleRoom
    {
        public static void Build(LogicalMapRoom room, LogicalMap mapDst)
        {
            const int Margin = 0;
            mapDst.Margin = Margin;
            mapDst.SetBounds(room.Width + Margin * 2, room.Height + Margin * 2);
            mapDst.EnsureAllocatedSizeFromBounds();
            mapDst.Clear();

            MapUtil.PlaceRoom(room, new Vector3Int(Margin, Margin, 0), mapDst);
            mapDst.UpdateCollisionMap();
        }
    }
}
