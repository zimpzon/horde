using UnityEngine;

namespace HordeEngine
{
    public class ActorAbility_EmitLight : MonoBehaviour, IComponentUpdate
    {
        public Color Color;
        Vector2 Scale;
        Vector2 Offset;
        public bool Enabled;

        Transform trans_;

        void OnEnable() { Global.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Global.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            throw new System.NotImplementedException();
        }
    }
}
