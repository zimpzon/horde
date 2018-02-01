using System.Collections.Generic;

namespace HordeEngine
{
    public class TileInfo
    {
        public int id;
        public byte[] collision = new byte[4];
    }

    public enum TileTypes { WallHoriz, WallVertLeft, WallVertRight, WallCornerUpLeft, WallCornerUpRight };

    // All data for a single room
    public class Room
    {
        List<int> tiles_ = new List<int>();
        public int width_;
        public int height_;
    }
}
