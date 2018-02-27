namespace HordeEngine
{
    public static class MapConstants
    {
        public static readonly float TileW = 1.0f;
        public static readonly float TileH = 1.0f;
        public static readonly int ChunkW = 8;
        public static readonly int ChunkH = 8;
        public static readonly int LightmapResolution = 2;

        public static readonly byte CollOutsideMap = 0;
        public static readonly byte CollBlocked = 255;
        public static readonly byte CollWalkable = 127;
    }
}
