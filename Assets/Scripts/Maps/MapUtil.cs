using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapUtil
{
    // Reuse the same list for all returned results. Assumes single-threaded.
    public static List<Tile> LatestResult = new List<Tile>(50);

    /// <summary>
    /// Clears cells in tilemap in a circle at worldPosition within worldRadius.
    /// Returns number of cleared cells. LatestResult contains the cells.
    /// </summary>
    public static int ClearCircle(Tilemap wallTiles, Tilemap topTiles, Vector3 worldPosition, float worldRadius)
    {
        LatestResult.Clear();

        var cellPosition = wallTiles.WorldToCell(worldPosition);
        int cellRadius = (int)(worldRadius / wallTiles.cellSize.x); // Assuming square tiles
        int y0 = cellPosition.y - cellRadius;
        int y1 = cellPosition.y + cellRadius;
        int x0 = cellPosition.x - cellRadius;
        int x1 = cellPosition.x - cellRadius;
        for (int y = y0; y <= y1; ++y)
        {
            for (int x = x0; x <= x1; ++x)
            {
                cellPosition.x = x;
                cellPosition.y = y;
                var tile = wallTiles.GetTile<Tile>(cellPosition);
                if (tile != null)
                {
                    wallTiles.SetTile(cellPosition, null);

                    cellPosition.y += 1;
                    topTiles.SetTile(cellPosition, null);

                    LatestResult.Add(tile);
                }
            }
        }

        return LatestResult.Count;
    }
}
