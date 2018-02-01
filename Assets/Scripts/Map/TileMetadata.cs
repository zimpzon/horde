using System;
using System.Collections.Generic;

#pragma warning disable CS0649

[Serializable]
public class TileMetadata
{
    public int TileId;
    public string CollisionStr;
    // Maybe normal can be calculated from collision map? Collision blocks are steep if neighbor is empty?
    public float LightMultiplier;
    public bool IsDoor;
}

[Serializable]
public class TileMapMetadata
{
    public int imagewidth;
    public int imageheight;
    public int tilecount;
    public int columns;
    public int tileheight;
    public List<TileMetadata> tileproperties = new List<TileMetadata>();
    public Dictionary<int, TileMetadata> tileLookup = new Dictionary<int, TileMetadata>();

    public void UpdateTileLookup()
    {
        tileLookup.Clear();
        foreach(var tile in tileproperties)
        {
            tileLookup[tile.TileId] = tile;
        }
    }
}

#pragma warning restore CS0649
