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

                    mapDst.Walls[dstIdx] = room.WallTiles[srcIdx];
                    mapDst.Floor[dstIdx] = room.FloorTiles[srcIdx];
                    mapDst.Props[dstIdx] = room.PropTiles[srcIdx];
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

        public static void ArrayToPng(string path, int[] array, int w, int h, int idEmpty)
        {
            ArrayToPng(path, array, new BoundsInt(0, 0, 0, w, h, 0), w, idEmpty);
        }

        public static void ArrayToPng(string path, int[] array, BoundsInt bounds, int stride, int idEmpty)
        {
            int w = bounds.size.x;
            int h = bounds.size.y;
            Color[] pixels = new Color[w * h];
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int arrayValue = array[y * stride + x];
                    if (arrayValue > 255)
                        arrayValue = 255;

                    byte pixelValue = (byte)arrayValue;// (arrayValue == idEmpty ? 0 : (arrayValue >> 1) + 128); // Scale value to be between 128 and 255
                    pixels[(h - y - 1) * w + x] = new Color32(pixelValue, pixelValue, pixelValue, 255);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            var bytes = tex.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }

        public static void TextureToPng(string path, Texture2D tex)
        {
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
