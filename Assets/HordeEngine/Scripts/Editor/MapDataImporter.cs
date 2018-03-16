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
#pragma warning disable 649
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
#pragma warning restore 649

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
        public List<LogicalMapRoom> Rooms = new List<LogicalMapRoom>();
    }

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
        var inputPath = "Assets/Maps/Tiled/tilesets/metadata";
        var outputPath = "Assets/Resources/TileMetadata";

        var files = Directory.GetFiles(inputPath, "*.json", SearchOption.TopDirectoryOnly);
        if (files.Length != 1)
            throw new Exception(string.Format("Expected exactly one metadata file in {0}, found {1} ", inputPath, files.Length));

        var file = files[0];
        Debug.Log("Parsing file (a single parse error is expected since Unity cannot read json maps): " + file);
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
                var tileMeta = new TileMetadata
                {
                    // New tile metadata fields must be added here:
                    TileId = key.AsInt,
                    Blocking = value["Blocking"].AsBool,
                    Height = value["Height"].AsFloat,
                    ShadowLength = value["ShadowLength"].AsFloat
                };

                tileMetadata.tileproperties.Add(tileMeta);
            }
        }

        Directory.CreateDirectory(outputPath);
        var outputFile = Path.Combine(outputPath, "TileMetadata.json");

        var jsonOutput = JsonUtility.ToJson(tileMetadata);
        File.WriteAllText(outputFile, jsonOutput);
        AssetDatabase.ImportAsset(outputFile);

        Debug.Log("Imported tile metadata.");
    }

    static void ImportRooms()
    {
        var inputPath = "Assets/Maps/Tiled/rooms";
        var outputPath = "Assets/Resources/Rooms";

        var files = Directory.GetFiles(inputPath, "*.json", SearchOption.AllDirectories);
        if (files.Length == 0)
            throw new Exception("No room files found in " + inputPath);

        List<LogicalMapRoom> rooms = new List<LogicalMapRoom>();
        foreach (string file in files)
        {
            Debug.Log("Parsing file: " + file);
            var json = File.ReadAllText(file);
            var map = JsonUtility.FromJson<JsonMap>(json);

            LogicalMapRoom room = GetRoom(map);
            if (room == null)
                continue;

            room.Name = Path.GetFileNameWithoutExtension(file).ToLower();
            rooms.Add(room);
        }

        var wrapper = new RoomWrapper
        {
            Rooms = rooms
        };

        var jsonRooms = JsonUtility.ToJson(wrapper, prettyPrint: false);
        Directory.CreateDirectory(outputPath);
        var outputFile = Path.Combine(outputPath, "rooms.json");
        File.WriteAllText(outputFile, jsonRooms);
        AssetDatabase.ImportAsset(outputFile, ImportAssetOptions.ForceUpdate);

        Debug.Log(string.Format("Imported {0} room(s).", rooms.Count));
    }

    static LogicalMapRoom GetRoom(JsonMap map)
    {
        var walls = map.GetLayer("Walls");
        var floor = map.GetLayer("Floor");
        var props = map.GetLayer("Props");
        var objects = map.GetLayer("Objects");

        // The Walls layer is used to determine the bounds of the room.
        var srcBounds = MapUtil.GetClampedBounds(walls.data, walls.width, walls.height, idEmpty: 0);
        Debug.Log("Room bounds: " + srcBounds);
        if (srcBounds.size.x * srcBounds.size.y == 0)
        {
            Debug.LogWarning("No room found, skipping.");
            return null;
        }

        var room = new LogicalMapRoom
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

    room.WallTiles = room.WallTiles.Select(tileId => FilterTileId(tileId)).ToArray();
        room.FloorTiles = room.FloorTiles.Select(tileId => FilterTileId(tileId)).ToArray();
        room.PropTiles = room.PropTiles.Select(tileId => FilterTileId(tileId)).ToArray();

        return room;
    }

    static int FilterTileId(int id)
    {
        // When exporting from Tiled 0 means empty and 1 is added to all tile ids.
        // Subtract 1 so ids match what we want: 0 is the first tile in the tile map
        // and -1 means empty.
        // Also remove flags for mirrored tiles, we don't support that. Tiled editor will
        // flag mirrored tiles when mirroring areas of a map, but that is not what we want.
        const uint FlagDiagonal = 0x20000000;
        const uint FlagVertical = 0x40000000;
        const uint FlagHorizontal = 0x80000000;
        uint uid = (uint)id;
        uid &= ~FlagDiagonal;
        uid &= ~FlagVertical;
        uid &= ~FlagHorizontal;
        id = (int)uid;
        return id - 1;
    }
}
