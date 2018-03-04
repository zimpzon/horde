using System;
using UnityEngine;
using UnityEngine.UI;

namespace HordeEngine
{
    [RequireComponent(typeof(RawImage))]
    public class MiniMapScript : MonoBehaviour
    {
        public Texture2D Texture;
        Color[] pixels_;

        RawImage image_;

        private void Awake()
        {
            image_ = GetComponent<RawImage>();    
        }

        public void SetDebugPixel(int idx, Color col)
        {
            // NB NB Requires margin of 0 atm
            int w = Texture.width;
            int h = Texture.height;
            int y = idx / w;
            int x = idx % w;
            pixels_[(h - y - 1) * w + x] = col;
            Texture = new Texture2D(Texture.width, Texture.height, TextureFormat.RGB24, false);
            Texture.filterMode = FilterMode.Point;
            Texture.SetPixels(pixels_);
            image_.texture = Texture;
        }

        public void SetMap(LogicalMap map)
        {
            int w = map.Width * 2;
            int h = map.Height * 2;
            int stride = map.CollisionWidth;
            const int PixelMargin = 0;
            const int PixelMargin2 = PixelMargin * 2;
            int PixelStride = w + PixelMargin2;
            int pixelW = w + PixelMargin2;
            int pixelH = h + PixelMargin2;

            pixels_ = new Color[pixelW * pixelH];
            Texture = new Texture2D(pixelW, pixelH, TextureFormat.RGB24, false);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int arrayValue = map.CollisionMap[y * stride + x];
                    byte pixelValue = (byte)arrayValue;
                    pixels_[(h - y - 1 + PixelMargin) * PixelStride + x + PixelMargin] = new Color32(pixelValue, pixelValue, pixelValue, 255);
                }
            }

            pixels_[400] = Color.red;
            Texture.SetPixels(pixels_);
            Texture.filterMode = FilterMode.Point;
            image_.texture = Texture;

            if (Global.WriteDebugPngFiles)
            {
                Global.WriteDebugPng("mini_in", Array.ConvertAll(map.CollisionMap, item => (int)item), map.Width * 2, map.Height * 2, 0);
                Global.WriteDebugPng("mini_out", Texture);
            }
        }
    }
}
