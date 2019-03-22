using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HordeEngine
{
    /// <summary>
    /// GameManager will find this and set it in Global.SceneAccess.
    /// </summary>
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
        public RawImage LightDebugView;
        public ProjectileManager ProjectileManager;
        public ProjectileBlueprints ProjectileBlueprints;
        public LightingImageEffect LightingImageEffect;
    }
}
