using HordeEngine;
using UnityEngine;

public static class CollisionUtil
{
    public static byte[] CollisionMap;
    public static int Width;
    public static int Height;

    public static void SetCollisionMap(byte[] map, int w, int h)
    {
        CollisionMap = map;
        Width = w;
        Height = h;
        Global.GameManager.ShowDebug("CollisionMap", "w = {0}, h = {1}", w, h);
    }

    public static bool TryMove(ref Vector3 pos, float w, float h, float offsetY, Vector3 velocity)
    {
        var newPos = pos + velocity;
        pos += velocity;
        // A tile is 1 unit and there are 2x2 collision units per tile, so a collision unit is 0.5 units
        // Optimization: We don't need to check the full size, only the new cells entered. Might not be worth it though.
        return true;
    }

    public static bool GetCollisionValue(Vector3 pos)
    {
        int x = (int)(pos.x * 2);
        int y = (int)(pos.y * 2);
        int collIdx = y * Width + x;
        Global.GameManager.ShowDebug("Coll", "{0}, {1} = {2} {3}", x, y, "-", pos.ToString());
        var collision = CollisionMap[collIdx];
        Global.GameManager.ShowDebug("Coll", "{0}, {1} = {2} {3}", x, y, collision, pos.ToString());
        Global.SceneAccess.MiniMap.SetDebugPixel(collIdx, Color.red);

        return false;
    }

    public static bool IsColliding(Vector3 pos, float w, float h, float offsetY)
    {
        // A tile is 1 unit and there are 2x2 collision units per tile, so a collision unit is 0.5 units
        float halfW = w * 2 * 0.5f;
        float halfH = h * 2 * 0.5f;
        int x0 = (int)((pos.x - halfW) * 2);
        int x1 = (int)((pos.x + halfW) * 2);
        int y0 = -(int)((pos.y - halfH + offsetY) * 2);
        int y1 = -(int)((pos.y + halfH + offsetY) * 2);
        x1 = x0;
        y1 = y0;
        for (int y = y0; y <= y1; ++y)
        {
            for (int x = x0; x <= x1; ++x)
            {
                //if (x < 0 || y < 0 || x >= Width || y >= Height)
                //    continue;

                Global.GameManager.ShowDebug("Test", "x = {0}, y = {1}", x, y);
                int collIdx = y * Width + x;
                var collision = CollisionMap[collIdx];
                Global.GameManager.ShowDebug("Coll", collision.ToString());
                Global.SceneAccess.MiniMap.SetDebugPixel(collIdx, Color.red);
            }
        }

        return false;
    }
}
