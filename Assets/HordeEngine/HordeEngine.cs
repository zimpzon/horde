using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// Shortcut class
    /// </summary>
    public static class Horde
    {
        public static HordeEngine Engine;
        public static ComponentUpdater ComponentUpdater = new ComponentUpdater();
        public static TimeManager Time = new TimeManager();
        public static HordeSpriteManager Sprites;
    }

    /// <summary>
    /// HordeEngine root
    /// </summary>
    [ExecuteInEditMode]
    public class HordeEngine : MonoBehaviour
    {
        public float SlowableTimeScale = 1.0f;

        void Awake()
        {
            Horde.Engine = this;
        }

        public void SetDebugTexture(Texture tex)
        {
            Global.SceneAccess.LightDebugView.texture = tex;
        }

        void Update()
        {
            // This is the first Update() to be called in every frame
            Horde.Time.SlowableTimeScale = SlowableTimeScale;
            Horde.Time.UpdateTime(Time.deltaTime);

            Horde.ComponentUpdater.DoUpdate();

        }

        void LateUpdate()
        {
            // This is the first LateUpdate() to be called in every frame
            Horde.ComponentUpdater.DoLateUpdate();
        }
    }
}
