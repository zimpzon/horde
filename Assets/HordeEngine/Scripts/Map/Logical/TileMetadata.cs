using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileMetadata
{
    public static TileMetadata Default = new TileMetadata();
    public const int NoTile = -1;

    public int TileId;
    public bool Blocking;
    public float Height = 1.0f;
    public float ShadowLength;
}

[Serializable]
public class TileMapMetadata
{
    // The following names correspond to fields in json imported from Tiled.
    // They must match (case-sensitive) so don't change them.
    // Begin json field names ---->
    public int imagewidth;
    public int imageheight;
    public int tilecount;
    public int columns;
    public int tileheight;
    // <---- End json field names

    public List<TileMetadata> tileproperties = new List<TileMetadata>();
    public Dictionary<int, TileMetadata> tileLookup = new Dictionary<int, TileMetadata>();

    int tilesPerRow;
    int tilesPerCol;
    float tileUvSize;

    public Vector2 CalcUV(int tileId, int cornerX, int cornerY, float tileScale)
    {
        int tileX = tileId % columns;
        int tileY = tileId / columns;
        float tileUvBottom = 1.0f - ((tileY + 1) * tileUvSize);
        return new Vector2((tileX + cornerX) * tileUvSize, tileUvBottom + (cornerY * tileUvSize * 1.5f));
    }

    public void CreatePropertyLookup()
    {
        tilesPerRow = imagewidth / tileheight;
        tilesPerCol = imageheight / tileheight;
        tileUvSize = 1.0f / tilesPerRow;
        tileLookup.Clear();
        foreach(var tile in tileproperties)
        {
            tileLookup[tile.TileId] = tile;
        }
    }
}
