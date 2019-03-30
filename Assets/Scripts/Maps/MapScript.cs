using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.ParticleSystem;

public class MapScript : MonoBehaviour
{
    public Tilemap FloorTileMap;
    public Tilemap WallTileMap;
    public Tilemap TopTileMap;
    public GameObject BackgroundQuad;

    public void ClearWallCircle(Vector3 worldPosition, float worldRadius)
    {
        int count = MapUtil.ClearCircle(WallTileMap, TopTileMap, worldPosition, worldRadius);
        for (int i = 0; i < count; ++i)
        {
            var tile = MapUtil.LatestResult[i];
            EmitParams param = new EmitParams();
            SceneGlobals.Instance.ParticleScript.WallDestructionParticles.Emit(tile.gameObject.transform.position, 20);
        }
        Debug.Log("Count: " + count);
    }

    void Awake()
    {
    }

    void Update()
    {
        
    }
}
