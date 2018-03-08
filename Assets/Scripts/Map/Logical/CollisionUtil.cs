using System;
using System.Collections.Generic;
using HordeEngine;
using UnityEngine;

public static class CollisionUtil
{
    public static byte[] CollisionMap;
    public static int Width;
    public static int Height;
    public static List<Vector3> PointsToMove = new List<Vector3>(20);

    public static void SetCollisionMap(byte[] map, int w, int h)
    {
        CollisionMap = map;
        Width = w;
        Height = h;
        Global.GameManager.ShowDebug("CollisionMap", "w = {0}, h = {1}", w, h);
    }

    public static bool TryMovePoints(List<Vector3> pointsToMove, Vector3 velocity, out Vector3 shortestMove, bool adjustOnCollision = true)
    {
        shortestMove = velocity;
        bool allMoved = true;
        float shortestSqr = float.MaxValue;

        for (int i = 0; i < pointsToMove.Count; ++i)
        {
            var point = pointsToMove[i];
            bool couldMove = TryMovePoint(ref point, velocity, adjustOnCollision);
            if (!couldMove)
            {
                allMoved = false;
                var movementVec = point - pointsToMove[i];
                Global.GameManager.ShowDebug(i.ToString(), movementVec.magnitude);
                if (point.sqrMagnitude < shortestSqr)
                {
                    shortestMove = movementVec;
                    shortestSqr = movementVec.sqrMagnitude;
                }
            }
        }
        return allMoved;
    }

    public static bool TryMovePoint(ref Vector3 pos, Vector3 velocity, bool adjustOnCollision = true)
    {
        var newPos = pos + velocity;
        if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
        {
            if (!adjustOnCollision)
                return false;

            // There is a collision, adjust movement
            // Do x and y movement separate so we can glide against walls
            newPos = pos;
            newPos.x += velocity.x;
            if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
            {
                int x = (int)(newPos.x * 2);
                newPos = ClampToCellX(pos, newPos, x);
            }

            newPos.y += velocity.y;
            if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
            {
                int y = -(int)(newPos.y * 2);
                newPos = ClampToCellY(pos, newPos, y);
            }

            pos = newPos;
            return false;
        }

        pos = newPos;
        return true;
    }

    // Will not clamp if pos and newPos are in the same X cell.
    private static Vector3 ClampToCellX(Vector3 pos, Vector3 newPos, int x)
    {
        Vector3 result = newPos;
        int oldX = (int)(pos.x * 2);

        const float Nudge = 0.01f; // When clamping to a cell border move a tiny bit away from the cell
        if (x < oldX)
        {
            // Moving left, clamp x to right side of cell
            result.x = (x + 1) * 0.5f + Nudge;
        }
        else if (x > oldX)
        {
            // Moving right, clamp x to left side of cell
            result.x = x * 0.5f - Nudge;
        }
        return result;
    }

    // Will not clamp if pos and newPos are in the same Y cell.
    private static Vector3 ClampToCellY(Vector3 pos, Vector3 newPos, int y)
    {
        Vector3 result = newPos;
        int oldY = -(int)(pos.y * 2);

        const float Nudge = 0.01f; // When clamping to a cell border move a tiny bit away from the cell
        if (y < oldY)
        {
            // Moving up, clamp y to bottom side of cell
            result.y = -((y + 1) * 0.5f) - Nudge;
        }
        else if (y > oldY)
        {
            // Moving down, clamp y to top side of cell
            result.y = -(y * 0.5f) + Nudge;
        }
        return result;
    }

    public static int GetCollisionValue(Vector3 pos)
    {
        int x = (int)(pos.x * 2);
        int y = -(int)(pos.y * 2);
        int collIdx = y * Width + x;
        var collision = CollisionMap[collIdx];
        if (Time.frameCount % 100 == 0)
            Global.SceneAccess.MiniMap.SetDebugPixel(collIdx, collision == MapConstants.CollWalkable ? Color.green : Color.red);
        return collision;
    }
}
