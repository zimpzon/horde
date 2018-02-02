using UnityEngine;

namespace HordeEngine
{
    public static class MapUtil
    {
        public static void CopyBlock(int[] src, int[] dst, BoundsInt srcBounds, BoundsInt dstBounds)
        {
            int topLeftSrc = srcBounds.size.x * srcBounds.y + srcBounds.x;
            int topLeftDst = dstBounds.size.x * dstBounds.y + dstBounds.x;

            for (int y = 0; y < dstBounds.size.y; ++y)
            {
                for (int x = 0; x < dstBounds.size.x; ++x)
                {
                    int idxSrc = topLeftSrc + y * srcBounds.size.x + x;
                    int idxDst = topLeftDst + y * dstBounds.size.x + x;
                    dst[idxDst] = src[idxSrc];
                }
            }
        }

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
            var result = new BoundsInt(0, 0, 0, 0, 0, 0);

            // Clamp left
            for (int x = 0; x < w; ++x)
            {
                if (!ColIsEmpty(tiles, w, h, x))
                {
                    result.xMin = x;
                    break;
                }
            }

            // Clamp right
            for (int x = w - 1; x >= 0; --x)
            {
                if (!ColIsEmpty(tiles, w, h, x))
                {
                    result.xMax = x;
                    break;
                }
            }

            // Clamp top
            for (int y = 0; y < h; ++y)
            {
                if (!RowIsEmpty(tiles, w, h, y))
                {
                    result.yMin = y;
                    break;
                }
            }

            // Clamp bottom
            for (int y = h - 1; y >= 0; --y)
            {
                if (!RowIsEmpty(tiles, w, h, y))
                {
                    result.yMax = y;
                    break;
                }
            }

            return result;
        }
    }
}
