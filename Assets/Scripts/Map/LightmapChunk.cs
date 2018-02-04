using UnityEngine;

namespace HordeEngine
{
    // Light rendering
    // Max value or curve (1d tex lookup?)
    class LightmapChunk
    {
        int[] staticLightData = new int[0]; // x * y * 4 (RGBA)
        int[] dynamicLightData = new int[0]; // x * y * 4 (RGBA)

        Texture2D staticLightTexture; // Dynamic objects will sample this to get environment color
        Texture2D dynamicLightTexture; // Dynamic objects will sample this to get environment color

        public LightmapChunk(int w, int h, float tileW, float tileH, int resolution)
        {

        }
    }
}
