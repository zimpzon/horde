using System.IO;
using UnityEngine;

namespace HordeEngine
{
    public static class Global
    {
        static Global()
        {
            // When in edit mode we cannot be sure of execution order so initialize these if missing
            if (TimeManager == null)
            {
                Debug.Log("Creating TimeManager for edit mode");
                TimeManager = new TimeManager();
            }

            if (ComponentUpdater == null)
            {
                Debug.Log("Creating ComponentUpdater for edit mode");
                ComponentUpdater = new ComponentUpdater();
            }
        }

        // TODO: This does NOT belong here. Probably GameManager but that will make it crazy big.
        public static void SetMap(LogicalMap map)
        {
            CurrentMap = map;
            SceneAccess.MapRenderer.SetMap(map);
            CollisionUtil.SetCollisionMap(map.CollisionMap, map.CollisionWidth, map.CollisionHeight);
        }

        public static GameManager GameManager;
        public static SceneAccessScript SceneAccess;
        public static TimeManager TimeManager;
        public static ComponentUpdater ComponentUpdater;
        public static MapResources MapResources;
        public static LogicalMap CurrentMap;
        public static CrosshairController Crosshair;

        // On/off toggle for verbose debug logging
        public static bool DebugLogging = true;

        // On/off toggle for expensive debug drawing
        public static bool DebugDrawing = false;

        // Enable this to output debug png files of map layers and collision layer. Set path in DebugPngFilePath.
        public static bool WriteDebugPngFiles = true;
        public static string DebugPngFilePath = @"c:\HordeEngineDebug";
        public static void WriteDebugPng(string filename, int[] array, int w, int h, int idEmpty)
        {
            Directory.CreateDirectory(DebugPngFilePath);
            MapUtil.ArrayToPng(Path.Combine(DebugPngFilePath, filename) + ".png", array, w, h, idEmpty);
        }

        public static void WriteDebugPng(string filename, Texture2D tex)
        {
            Directory.CreateDirectory(DebugPngFilePath);
            MapUtil.TextureToPng(Path.Combine(DebugPngFilePath, filename) + ".png", tex);
        }
    }
}
