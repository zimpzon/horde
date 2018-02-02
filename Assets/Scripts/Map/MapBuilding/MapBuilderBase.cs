using UnityEngine;

namespace HordeEngine
{
    // 1) Do logical layout
    // 2) Define physical bounds
    // 3) Write tiles
    // 4) Create meshes
    public class MapBuilderBase
    {
        protected void PlaceRoom(Room room, Vector3Int pos, MapData mapDst)
        {
            for (int y = 0; y < room.Height; ++y)
            {
                for (int x = 0; x < room.Width; ++x)
                {
                    int dstIdx = pos.y * mapDst.stride + pos.x;
                    int srcIdx = y * room.Width + x;
                    mapDst.walls[dstIdx] = room.WallTiles[srcIdx];
                    mapDst.floor[dstIdx] = room.FloorTiles[srcIdx];
                    mapDst.props[dstIdx] = room.PropTiles[srcIdx];
                }
            }
        }
    }
}
