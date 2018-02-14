using System.IO;

namespace HordeEngine
{
    public static class Global
    {
        public static GameManager GameManager;
        public static MapRenderer MapManager;
        public static TimeManager TimeManager;
        public static MapResources MapResources;

        // On/off toggle for verbose debug logging
        public static bool DebugLogging = true;

        // Enable this to output debug png files of map layers and collision layer. Set path in DebugPngFilePath.
        public static bool WriteDebugPngFiles = true;
        public static string DebugPngFilePath = @"c:\HordeEngineDebug";
        public static void WriteDebugPng(string filename, int[] array, int w, int h, int idEmpty)
        {
            Directory.CreateDirectory(DebugPngFilePath);
            MapUtil.ArrayToPng(Path.Combine(DebugPngFilePath, filename) + ".png", array, w, h, idEmpty);
        }
    }
}
