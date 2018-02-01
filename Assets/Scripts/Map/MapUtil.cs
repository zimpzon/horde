using UnityEngine;

namespace HordeEngine
{
    public static class MapUtil
    {
        public static bool RowIsEmpty(int[] tiles, int w, int h, int row)
        {
            int rowBegin = row * w;
            for (int i = 0; i < w; ++i)
            {
                if (tiles[rowBegin + i] != 0)
                    return false;
            }
            return true;
        }

        public static bool ColIsEmpty(int[] tiles, int w, int h, int col)
        {
            for (int i = 0; i < h; ++i)
            {
                if (tiles[col + i * w] != 0)
                    return false;
            }
            return true;
        }

        public static BoundsInt GetClampedBounds(int[] tiles, int w, int h)
        {
            var result = new BoundsInt(0, 0, 0, w, h, 0);
            for (int x = 0; x < w / 2; ++x)
            {
                if (ColIsEmpty(tiles, w, h, x))
                    result.xMin = x;

                if (ColIsEmpty(tiles, w, h, w - x - 1))
                    result.xMax = x;
            }

            for (int y = 0; y < h / 2; ++y)
            {
                if (RowIsEmpty(tiles, w, h, y))
                    result.yMin = y;

                if (RowIsEmpty(tiles, w, h, h - y - 1))
                    result.yMax = y;
            }
            return result;
        }
    }
}
