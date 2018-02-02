using UnityEngine;

namespace HordeEngine
{
    public class MapData
    {
        public int[] walls;
        public int[] floor;
        public int[] props;
        public int stride;
        public BoundsInt mapBounds = new BoundsInt();
        public int margin = 2;

        public VirtualMap virtualMap;

        public void EnsureSizeFromBounds()
        {
            int size = mapBounds.size.x * mapBounds.size.y;
            if (walls == null || walls.Length < size)
            {
                walls = new int[size];
                floor = new int[size];
                props = new int[size];
            }
        }
    }
}
