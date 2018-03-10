using HordeEngine;
using UnityEngine;

[RequireComponent(typeof(CameraPositioner))]
public class CameraController_PlayerAndCrosshair : MonoBehaviour
{
    CameraPositioner positioner_;

    private void Awake()
    {
        positioner_ = GetComponent<CameraPositioner>();
    }

    void LateUpdate()
    {
        var playerPos = Global.SceneAccess.PlayerTransform.localPosition;
        var crosshairPos = Global.Crosshair.GetWorldPosition();
        positioner_.SetTarget(Vector3.Lerp(playerPos, crosshairPos, 0.15f));
    }
}
