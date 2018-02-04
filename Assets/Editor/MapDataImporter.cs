using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using System;
using HordeEngine;
using System.Linq;
using SimpleJson;

/// <summary>
/// The json classes map directly to json exported by the Tiled editor.
/// (though fields in the json are omitted here if we have no use for them).
/// </summary>
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

    [Serializable]
    public class RoomWrapper
    {
        public List<Room> Rooms = new List<Room>();
    }

#pragma warning restore CS0649

    [MenuItem("Tools/Import Tiled Maps")]
    private static void DoImport()
    {
        Debug.Log("IMPORTING TILED MAP DATA");
        Debug.Log("----------------------------");

        ImportTileMetadata();
        ImportRooms();
    }

    static void ImportTileMetadata()
    {
        // Currently the input metadata file could just have been copied, but this way
        // syntax is verified and unneeded fields are stripped.
        var inputPath = "Assets/Maps/Tiled/tilesets/metadata";
        var outputPath = "Assets/Resources/TileMetadata";

        var files = Directory.GetFiles(inputPath, "*.json", SearchOption.TopDirectoryOnly);
        if (files.Length != 1)
            throw new Exception(string.Format("Expected exactly one metadata file in {0}, found {1} ", inputPath, files.Length));

        var file = files[0];
        Debug.Log("Parsing file: " + file);
        var json = File.ReadAllText(file);

        var tileMetadata = JsonUtility.FromJson<TileMapMetadata>(json);

        {
            // Unity JsonUtility does not yet support maps, so use SimpleJson to read the map data (*sigh*)
            var parsed = SimpleJSON.JSON.Parse(json);
            var tileProperties = parsed["tileproperties"];
            Debug.Log(string.Format("Found {0} tileProperties", tileProperties.Count));

            foreach(var key in tileProperties.Keys)
            {
                var value = tileProperties[key.Value];
                var tileMeta = new TileMetadata();

                // New tile metadata fields must be added here:
                tileMeta.TileId = key.AsInt;
                tileMeta.IsDoor = value["isDoor"].AsBool;
                tileMeta.LightMultiplier = value["LightMultiplier"].AsFloat;
                tileMeta.CollisionStr = value["Collision"];

                tileMetadata.tileproperties.Add(tileMeta);
            }
        }

        Directory.CreateDirectory(outputPath);
        var outputFile = Path.Combine(outputPath, "TileMetadata.json");

        var jsonOutput = JsonUtility.ToJson(tileMetadata);
        File.WriteAllText(outputFile, jsonOutput);

        Debug.Log("Imported tile metadata.");
    }

    static void ImportRooms()
    {
        var inputPath = "Assets/Maps/Tiled/rooms";
        var outputPath = "Assets/Resources/Rooms";

        var files = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);
        if (files.Length == 0)
            throw new Exception("No room files found in " + inputPath);

        List<Room> rooms = new List<Room>();
        foreach (string file in files)
        {
            Debug.Log("Parsing file: " + file);
            var json = File.ReadAllText(file);
            var map = JsonUtility.FromJson<JsonMap>(json);

            Room room = GetRoom(map);
            if (room == null)
                continue;

            rooms.Add(room);

            //MapUtil.TilesToPng(string.Format(@"d:\temp\HordeDebug\{0}_walls.png", Path.GetFileNameWithoutExtension(file)), room.WallTiles, room.Width, room.Height);
            //MapUtil.TilesToPng(string.Format(@"d:\temp\HordeDebug\{0}_floor.png", Path.GetFileNameWithoutExtension(file)), room.FloorTiles, room.Width, room.Height);
            //MapUtil.TilesToPng(string.Format(@"d:\temp\HordeDebug\{0}_props.png", Path.GetFileNameWithoutExtension(file)), room.PropTiles, room.Width, room.Height);
        }

        var wrapper = new RoomWrapper();
        wrapper.Rooms = rooms;

        var jsonRooms = JsonUtility.ToJson(wrapper, prettyPrint: false);
        Directory.CreateDirectory(outputPath);
        var outputFile = Path.Combine(outputPath, "rooms.json");
        File.WriteAllText(outputFile, jsonRooms);

        Debug.Log(string.Format("Imported {0} rooms.", rooms.Count));
    }

    static Room GetRoom(JsonMap map)
    {
        var walls = map.GetLayer("Walls");
        var floor = map.GetLayer("Floor");
        var props = map.GetLayer("Props");
        var objects = map.GetLayer("Objects");

        // The Walls layer is used to determine the bounds of the room.
        var srcBounds = MapUtil.GetClampedBounds(walls.data, walls.width, walls.height);
        Debug.Log("Room bounds: " + srcBounds);
        if (srcBounds.size.x * srcBounds.y == 0)
        {
            Debug.Log("ERROR: no room found, skipping.");
            return null;
        }

        var room = new Room
        {
            Width = srcBounds.size.x,
            Height = srcBounds.size.y
        };

        int tileCount = room.Width * room.Height;
        room.WallTiles = new int[tileCount];
        room.FloorTiles = new int[tileCount];
        room.PropTiles = new int[tileCount];

        var dstBounds = new BoundsInt(0, 0, 0, room.Width, room.Height, 0);
        MapUtil.CopyBlock(walls.data, room.WallTiles, srcBounds, dstBounds, walls.width, room.Width);
        MapUtil.CopyBlock(floor.data, room.FloorTiles, srcBounds, dstBounds, floor.width, room.Width);
        MapUtil.CopyBlock(props.data, room.PropTiles, srcBounds, dstBounds, props.width, room.Width);

        return room;
    }
}
