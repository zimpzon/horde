using TMPro;
using UnityEngine;

namespace HordeEngine
{
    public class SceneAccessScript : MonoBehaviour
    {
        public TextMeshProUGUI DebugText;
        public MapRenderer MapRenderer;
        public MiniMapScript MiniMap;
        public CameraShake CameraShake;
        public CameraTarget CameraTarget;

        void Awake()
        {
            Global.SceneAccess = this;
        }
    }
}
