using System;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

[Serializable]
public class TileMetadata
{
    public int TileId;
    public string CollisionStr;
    // TODO: Maybe normal can be calculated from collision map? Collision blocks are steep if neighbor is empty?
    public float LightMultiplier;
    public bool IsDoor;
    public Vector2 UV;

    public void UpdateInferredValues(TileMapMetadata meta)
    {
        int tileX = TileId % meta.columns;
        int tileY = TileId / meta.columns;
        UV = new Vector2(tileX * meta.tileheight, tileY * meta.tileheight);
    }
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

    /// <summary>
    /// After initial creation a selection of inferred values will be calculated (UV, etc)
    /// </summary>
    public void UpdateInferredValues()
    {
        for (int i = 0; i < tileproperties.Count; ++i)
            tileproperties[i].UpdateInferredValues(this);

        tileLookup.Clear();
        foreach(var tile in tileproperties)
        {
            tileLookup[tile.TileId] = tile;
        }
    }
}

#pragma warning restore CS0649
