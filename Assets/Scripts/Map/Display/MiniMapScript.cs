using System;
using UnityEngine;
using UnityEngine.UI;

namespace HordeEngine
{
    [RequireComponent(typeof(RawImage))]
    public class MiniMapScript : MonoBehaviour
    {
        public Color32 BlockedColor = Color.white;
        public Color32 WalkableColor = Color.blue;

        Texture2D texture_;
        Color32[] pixels_;
        Color32[] pixelsCopy_;

        const float PixelWorldSize = 0.1f;
        RawImage image_;

        public void CenterAt(Vector3 worldPos)
        {

        }

        private void Awake()
        {
            image_ = GetComponent<RawImage>();
        }

        private void LateUpdate()
        {
            if (pixels_ != null)
                Array.Copy(pixelsCopy_, pixels_, pixels_.Length);
        }

        public void SetDebugPixel(int idx, Color col)
        {
            // NB NB Requires margin of 0 atm
            int w = texture_.width;
            int h = texture_.height;
            int y = idx / w;
            int x = idx % w;
            pixels_[(h - y - 1) * w + x] = col;
            //texture_ = new Texture2D(texture_.width, texture_.height, TextureFormat.RGBA32, false)
            //{
            //    filterMode = FilterMode.Point
            //};
            texture_.SetPixels32(pixels_);
            texture_.Apply();
//            image_.texture = texture_;
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

            pixels_ = new Color32[pixelW * pixelH];
            texture_ = new Texture2D(pixelW, pixelH, TextureFormat.RGBA32, false);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int arrayValue = map.CollisionMap[y * stride + x];
                    Color32 col = new Color32(0, 0, 0, 0);
                    switch (arrayValue)
                    {
                        case MapConstants.CollBlocked: col = BlockedColor; break;
                        case MapConstants.CollWalkable: col = WalkableColor; break;
                    }
                    pixels_[(h - y - 1 + PixelMargin) * PixelStride + x + PixelMargin] = col;
                }
            }

            texture_.filterMode = FilterMode.Point;
            texture_.SetPixels32(pixels_);
            texture_.Apply();
            image_.texture = texture_;

            pixelsCopy_ = (Color32[])pixels_.Clone();

            if (Global.WriteDebugPngFiles)
            {
                Global.WriteDebugPng("mini_in", Array.ConvertAll(map.CollisionMap, item => (int)item), map.Width * 2, map.Height * 2, 0);
                Global.WriteDebugPng("mini_out", texture_);
            }
        }
    }
}
