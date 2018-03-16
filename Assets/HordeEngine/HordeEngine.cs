using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// Shortcut class
    /// </summary>
    public static class Horde
    {
        public static ComponentUpdater ComponentUpdater = new ComponentUpdater();
        public static TimeManager Time = new TimeManager();
        public static HordeSpriteManager Sprites;
    }

    /// <summary>
    /// HordeEngine root
    /// </summary>
    public class HordeEngine : MonoBehaviour
    {
    }
}
