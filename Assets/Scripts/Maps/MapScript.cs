using UnityEngine;
using UnityEngine.Tilemaps;

public class MapScript : MonoBehaviour
{
    public Tilemap FloorTileMap;
    public Tilemap WallTileMap;
    public Tilemap TopTileMap;
    public GameObject BackgroundQuad;

    public void ClearWallCircle(Vector3 worldPosition, float worldRadius)
    {
        int count = MapUtil.ClearCircle(WallTileMap, TopTileMap, worldPosition, worldRadius);
        Debug.Log("Count: " + count);
    }

    void Awake()
    {
    }

    void Update()
    {
        
    }
}
