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
        public CameraPositioner CameraTarget;
        public Transform PlayerTransform;
        public Camera LightingCam;
        public Camera GameplayCam;
        public Camera OverlayCam;

        void Awake()
        {
            Global.SceneAccess = this;
        }
    }
}
