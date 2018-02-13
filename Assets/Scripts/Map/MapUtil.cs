using UnityEngine;
using System.IO;

namespace HordeEngine
{
    public static class MapUtil
    {
        public static void PlaceRoom(LogicalMapRoom room, Vector3Int pos, LogicalMap mapDst)
        {
            for (int y = 0; y < room.Height; ++y)
            {
                for (int x = 0; x < room.Width; ++x)
                {
                    int dstIdx = (pos.y + y) * mapDst.Stride + pos.x + x;
                    int srcIdx = y * room.Width + x;

                    mapDst.walls[dstIdx] = room.WallTiles[srcIdx];
                    mapDst.floor[dstIdx] = room.FloorTiles[srcIdx];
                    mapDst.props[dstIdx] = room.PropTiles[srcIdx];
                }
            }
        }

        public static void CopyBlock(int[] src, int[] dst, BoundsInt srcBounds, BoundsInt dstBounds, int srcStride, int dstStride)
        {
            int topLeftSrc = srcBounds.y * srcStride + srcBounds.x;
            int topLeftDst = dstBounds.y * dstStride + dstBounds.x;

            for (int y = 0; y < dstBounds.size.y; ++y)
            {
                for (int x = 0; x < dstBounds.size.x; ++x)
                {
                    int idxSrc = topLeftSrc + y * srcStride + x;
                    int idxDst = topLeftDst + y * dstStride + x;
                    dst[idxDst] = src[idxSrc];
                }
            }
        }

        public static void ArrayToPng(string path, int[] tiles, int w, int h, int idEmpty)
        {
            ArrayToPng(path, tiles, new BoundsInt(0, 0, 0, w, h, 0), w, idEmpty);
        }

        public static void ArrayToPng(string path, int[] tiles, BoundsInt bounds, int stride, int idEmpty)
        {
            int w = bounds.size.x;
            int h = bounds.size.y;
            Color[] pixels = new Color[w * h];
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    byte value = (byte)(tiles[y * stride + x] == idEmpty ? 0 : 255);
                    pixels[y * w + x] = new Color32(value, value, value, 255);
                }
            }

            tex.SetPixels(pixels);
            var bytes = tex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }

        public static bool RowIsEmpty(int[] tiles, int w, int h, int row, int idEmpty)
        {
            int rowBegin = row * w;
            for (int i = 0; i < w; ++i)
            {
                if (tiles[rowBegin + i] != idEmpty)
                    return false;
            }
            return true;
        }

        public static bool ColIsEmpty(int[] tiles, int w, int h, int col, int idEmpty)
        {
            for (int i = 0; i < h; ++i)
            {
                if (tiles[col + i * w] != idEmpty)
                    return false;
            }
            return true;
        }

        public static BoundsInt GetClampedBounds(int[] tiles, int w, int h, int idEmpty)
        {
            var result = new BoundsInt(0, 0, 0, 0, 0, 0);

            // Clamp left
            for (int x = 0; x < w; ++x)
            {
                if (!ColIsEmpty(tiles, w, h, x, idEmpty))
                {
                    result.xMin = x;
                    break;
                }
            }

            // Clamp right
            for (int x = w - 1; x >= 0; --x)
            {
                if (!ColIsEmpty(tiles, w, h, x, idEmpty))
                {
                    result.xMax = x + 1;
                    break;
                }
            }

            // Clamp top
            for (int y = 0; y < h; ++y)
            {
                if (!RowIsEmpty(tiles, w, h, y, idEmpty))
                {
                    result.yMin = y;
                    break;
                }
            }

            // Clamp bottom
            for (int y = h - 1; y >= 0; --y)
            {
                if (!RowIsEmpty(tiles, w, h, y, idEmpty))
                {
                    result.yMax = y + 1;
                    break;
                }
            }

            return result;
        }
    }
}
