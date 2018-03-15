using System.Collections.Generic;
using HordeEngine;
using UnityEngine;

public static class CollisionUtil
{
    public static byte[] CollisionMap;
    public static int Width;
    public static int Height;
    public static List<Vector2> TempList = new List<Vector2>(20);
    public const float MaxVelocityPerFrame = 0.75f;
    public const float MaxVelocitySqrPerFrame = MaxVelocityPerFrame * MaxVelocityPerFrame;

    public static void SetCollisionMap(byte[] map, int w, int h)
    {
        CollisionMap = map;
        Width = w;
        Height = h;
    }

    public static void AddCollisionPoints(List<Vector2> list, Vector2 lineCenter, float width, float depth, float granularity)
    {
        // TODO: This should be a rectangle
        AddCollisionPointsForLine(list, lineCenter + Vector2.up * depth * 0.5f, width, granularity, clearList: true);
        AddCollisionPointsForLine(list, lineCenter + Vector2.down * depth * 0.5f, width, granularity, clearList: false);
    }

    public static void AddCollisionPointsForLine(List<Vector2> list, Vector2 lineCenter, float width, float granularity, bool clearList)
    {
        // From left to right with at least a granularity of [granularity]
        int sliceCount = Mathf.CeilToInt(width / granularity);
        float stepX = width / sliceCount;
        Vector2 point = lineCenter;
        point.x -= width * 0.5f;

        int pointCount = sliceCount + 1;
        if (clearList)
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

        if (velocity.sqrMagnitude > MaxVelocitySqrPerFrame)
            velocity = velocity.normalized * MaxVelocityPerFrame;

        maxAllowedMove = velocity;
        bool allMoved = true;
        float shortestSqr = float.MaxValue;

        for (int i = 0; i < pointsToMove.Count; ++i)
        {
            Vector2 tempCollisionNormal;
            var point = pointsToMove[i];
            var p = point;
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

        if (!allMoved)
        {
            // There was a collision. If all points can move the shortest
            // distance found then allow the move. Else we stop right here.
            for (int i = 0; i < pointsToMove.Count; ++i)
            {
                var newPoint = pointsToMove[i] + maxAllowedMove;
                if (GetCollisionValue(newPoint) != MapConstants.CollWalkable)
                {
                    maxAllowedMove = Vector2.zero;
                    break;
                }
            }
        }

        return allMoved;
    }

    public static bool TryMovePoint(ref Vector2 pos, Vector2 velocity, out Vector2 collisionNormal, bool reactToCollision = true)
    {
        collisionNormal = Vector2.zero;

        if (velocity.sqrMagnitude > MaxVelocityPerFrame * MaxVelocityPerFrame)
            velocity = velocity.normalized * MaxVelocityPerFrame;

        var newPos = pos + velocity;
        if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
        {
            if (!reactToCollision)
                return false;

            // There is a collision, adjust movement.
            // Do x and y movement separate so we can glide against walls.
            newPos = pos;
            newPos.x += velocity.x;
            if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
            {
                newPos = ClampToCellX(pos, newPos);
                collisionNormal += velocity.x < 0.0f ? Vector2.right : Vector2.left;
            }

            newPos.y += velocity.y;
            if (GetCollisionValue(newPos) != MapConstants.CollWalkable)
            {
                newPos = ClampToCellY(pos, newPos);
                collisionNormal += velocity.y < 0.0f ? Vector2.up: Vector2.down;
            }

            pos = newPos;
            return false;
        }

        pos = newPos;
        return true;
    }

    // Will not clamp if pos and newPos are in the same X cell.
    private static Vector2 ClampToCellX(Vector2 pos, Vector2 newPos)
    {
        Vector2 result = newPos;
        int oldX = (int)pos.x;
        int newX = (int)newPos.x;

        const float Nudge = 0.01f; // When clamping to a cell border move a tiny bit away from the cell
        if (newX < oldX)
        {
            // Moving left, clamp x to right side of cell
            result.x = newX + 1 + Nudge;
        }
        else if (newX > oldX)
        {
            // Moving right, clamp x to left side of cell
            result.x = newX - Nudge;
        }

        return result;
    }

    // Will not clamp if pos and newPos are in the same Y cell.
    private static Vector2 ClampToCellY(Vector2 pos, Vector2 newPos)
    {
        Vector2 result = newPos;
        int oldY = (int)-pos.y;
        int newY = (int)-newPos.y;

        const float Nudge = 0.01f; // When clamping to a cell border move a tiny bit away from the cell
        if (newY < oldY)
        {
            // Moving up, clamp y to bottom side of cell
            result.y = -newY - 1 - Nudge;
        }
        else if (newY > oldY)
        {
            // Moving down, clamp y to top side of cell
            result.y = -newY + Nudge;
        }

        return result;
    }

    public static bool IsCircleColliding(Vector2 pos, float size)
    {
        float halfSize = size * 0.5f;
        Vector2 testPos = Vector2.zero;

        // Top
        testPos.x = pos.x;
        testPos.y = pos.y + halfSize;
        int count = GetCollisionValue(testPos);

        // Bottom
        testPos.x = pos.x;
        testPos.y = pos.y - halfSize;
        count += GetCollisionValue(testPos);

        // Left
        testPos.x = pos.x - halfSize;
        testPos.y = pos.y;
        count += GetCollisionValue(testPos);

        // Right
        testPos.x = pos.x + halfSize;
        testPos.y = pos.y;
        count += GetCollisionValue(testPos);

        return count != MapConstants.CollWalkable * 4;
    }

    public static int GetCollisionValue(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = -(int)pos.y;
        int collIdx = y * Width + x;
        var collision = CollisionMap[collIdx];
        return collision;
    }
}
