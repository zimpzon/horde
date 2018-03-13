using HordeEngine;
using UnityEngine;

[RequireComponent(typeof(CameraPositioner))]
public class CameraController_PlayerAndCrosshair : MonoBehaviour, IComponentUpdate
{
    CameraPositioner positioner_;

    private void Awake()
    {
        positioner_ = GetComponent<CameraPositioner>();
    }

    void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late); }
    void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late); }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        var playerPos = Global.SceneAccess.PlayerTransform.localPosition;
        var crosshairPos = Global.Crosshair.GetWorldPosition();
        positioner_.SetTarget(Vector3.Lerp(playerPos, crosshairPos, 0.15f));
    }
}
