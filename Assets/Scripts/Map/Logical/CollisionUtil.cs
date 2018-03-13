using System.Collections.Generic;
using HordeEngine;
using UnityEngine;

// TODO: Can walk right through walls on low frame rates. Do stepping.
public static class CollisionUtil
{
    public static byte[] CollisionMap;
    public static int Width;
    public static int Height;
    public static List<Vector2> TempList = new List<Vector2>(20);
    const float MaxVelocity = 0.49f; // A collision tile is 0.5. Limit speed to just below that.

    public static void SetCollisionMap(byte[] map, int w, int h)
    {
        CollisionMap = map;
        Width = w;
        Height = h;
        Global.GameManager.ShowDebug("CollisionMap", "w = {0}, h = {1}", w, h);
    }

    public static void AddPointsForLine(List<Vector2> list, Vector2 lineCenter, float lineWidth, float granularity)
    {
        // From left to right with at least a granularity of [granularity]
        int sliceCount = Mathf.CeilToInt(lineWidth / granularity);
        float stepX = lineWidth / sliceCount;
        Vector2 point = lineCenter;
        point.x -= lineWidth * 0.5f;

        int pointCount = sliceCount + 1;
        list.Clear();
        for (int i = 0; i < pointCount; ++i)
        {
            list.Add(point);
            point.x += stepX;
        }
    }

    public static bool TryMovePoints(List<Vector2> pointsToMove, Vector2 velocity, out Vector2 maxAllowedMove, out Vector2 collisionNormal, bool reactToCollision = true)
    {
        collisionNormal = Vector2.zero;

        if (velocity.sqrMagnitude > MaxVelocity * MaxVelocity)
            velocity = velocity.normalized * MaxVelocity;

        maxAllowedMove = velocity;
        bool allMoved = true;
        float shortestSqr = float.MaxValue;

        for (int i = 0; i < pointsToMove.Count; ++i)
        {
            Vector2 tempCollisionNormal;
            var point = pointsToMove[i];
            bool couldMove = TryMovePoint(ref point, velocity, out tempCollisionNormal, reactToCollision);
            if (!couldMove)
            {
                allMoved = false;
                var movementVec = point - pointsToMove[i];
                if (movementVec.sqrMagnitude < shortestSqr)
                {
                    maxAllowedMove = movementVec;
                    shortestSqr = movementVec.sqrMagnitude;
                    collisionNormal = tempCollisionNormal;
                }
            }

            if (Application.isEditor)
            {
                if (GetCollisionValue(point) != MapConstants.CollWalkable)
                {
                    // After resolving collision this point is still inside a collider
                    Debug.LogErrorFormat("Error resolving collision, newPos = {0}, velocity = {1}, normal = {2}", point.ToString("0.00000000"), velocity.ToString("0.00000000"), collisionNormal.ToString("0.00000000"));
                }
            }
        }

        return allMoved;
    }

    public static bool TryMovePoint(ref Vector2 pos, Vector2 velocity, out Vector2 collisionNormal, bool reactToCollision = true)
    {
        collisionNormal = Vector2.zero;

        if (velocity.sqrMagnitude > MaxVelocity * MaxVelocity)
            velocity = velocity.normalized * MaxVelocity;

        var newPos = pos + velocity;
        if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
        {
            if (!reactToCollision)
                return false;

            // There is a collision, adjust movement
            // Do x and y movement separate so we can glide against walls
            newPos = pos;
            newPos.x += velocity.x;
            if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
            {
                int x = (int)(newPos.x * 2);
                newPos = ClampToCellX(pos, newPos, x);
                collisionNormal += velocity.x < 0.0f ? Vector2.right : Vector2.left;
            }

            newPos.y += velocity.y;
            if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
            {
                int y = -(int)(newPos.y * 2);
                newPos = ClampToCellY(pos, newPos, y);
                collisionNormal += velocity.y < 0.0f ? Vector2.up: Vector2.down;
            }

            pos = newPos;
            return false;
        }

        pos = newPos;
        return true;
    }

    // Will not clamp if pos and newPos are in the same X cell.
    private static Vector2 ClampToCellX(Vector2 pos, Vector2 newPos, int x)
    {
        Vector2 result = newPos;
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
    private static Vector2 ClampToCellY(Vector2 pos, Vector2 newPos, int y)
    {
        Vector2 result = newPos;
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

    public static int GetCollisionValue(Vector2 pos)
    {
        int x = (int)(pos.x * 2);
        int y = -(int)(pos.y * 2);
        int collIdx = y * Width + x;
        var collision = CollisionMap[collIdx];
        return collision;
    }
}
