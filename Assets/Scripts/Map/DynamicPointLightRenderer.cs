using HordeEngine;
using UnityEngine;

public class DynamicPointLightRenderer : MonoBehaviour, IComponentUpdate
{
    DynamicQuadMesh mesh_;

    void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Late); }
    void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Late); }

    void Update()
    {

    }

    public void ComponentUpdate(ComponentUpdatePass pass)
    {
        throw new System.NotImplementedException();
    }
}
