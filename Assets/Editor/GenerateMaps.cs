using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using System;
using HordeEngine;
using System.Linq;

public class MenuItems
{
#pragma warning disable CS0649

    [Serializable]
    class JsonObject
    {
        public int id;
        public string name;
        public float x;
        public float y;
    }

    [Serializable]
    class JsonLayer
    {
        public string name;
        public string type;
        public int width;
        public int height;
        public int x;
        public int y;
        public int[] data;
        public JsonObject[] objects;
    }

    [Serializable]
    class JsonMap
    {
        public JsonLayer GetLayer(string name)
        {
            return layers.Where(l => l.name == name).First();
        }

        public List<JsonLayer> layers = new List<JsonLayer>();
    }
#pragma warning restore CS0649

    [MenuItem("Tools/Import Tiled Maps")]
    private static void MenuCode()
    {
        var files = Directory.GetFiles("Assets/Maps/Tiled/rooms", "*.json", SearchOption.AllDirectories);
        if (files.Length == 0)
            throw new Exception("No files found");

        int roomCount = 0;
        foreach (string file in files)
        {
            Debug.Log("Found file: " + file);
            var json = File.ReadAllText(file);
            var map = JsonUtility.FromJson<JsonMap>(json);

            Room room = GetRoom(map);
            roomCount++;
        }

        Debug.Log(string.Format("Found {0} rooms", roomCount));
    }

    static Room GetRoom(JsonMap map)
    {
        var result = new Room();
        var walls = map.GetLayer("Walls");
        var floor = map.GetLayer("Floor");
        var props = map.GetLayer("Props");
        var objects = map.GetLayer("Objects");

        var bounds = MapUtil.GetClampedBounds(walls.data, walls.width, walls.height);
        Debug.Log(bounds);
        return result;
    }
}