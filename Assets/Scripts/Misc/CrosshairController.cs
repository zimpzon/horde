using HordeEngine;
using UnityEngine;

public class CrosshairController
{
    public Vector2 GetDirectionVector(Vector2 from, bool normalize = false)
    {
        var result = GetWorldPosition() - from;
        return normalize ? result.normalized : result;
    }

    public Vector2 GetWorldPosition()
    {
        return Global.SceneAccess.GameplayCam.ScreenToWorldPoint(Input.mousePosition);
    }
}
