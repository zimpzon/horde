using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(Camera))]
    public class LightingCamera : MonoBehaviour
    {
        public LayerMask LightingLayer;
        public RenderTextureFormat LightingTextureFormat;
        public Camera ParentCamera;

        Camera lightingCam_;
        LightingImageEffect lightingImageEffect_;

        private void Awake()
        {
            lightingCam_ = GetComponent<Camera>();
            lightingImageEffect_ = ParentCamera.GetComponent<LightingImageEffect>();
        }

        private void OnEnable()
        {
            AlignWithParentCamera(ParentCamera);

            lightingCam_.targetTexture = RenderTexture.GetTemporary(ParentCamera.pixelWidth, ParentCamera.pixelHeight, 0, LightingTextureFormat);
            lightingImageEffect_.LightingTexture = lightingCam_.targetTexture;
        }

        private void OnDisable()
        {
            RenderTexture.ReleaseTemporary(lightingCam_.targetTexture);
            lightingCam_.targetTexture = null;
            lightingImageEffect_.LightingTexture = null;
        }

        void AlignWithParentCamera(Camera parent)
        {
            // Render before main camera
            lightingCam_.depth = parent.depth - 1;

            // Align size with main camera
            lightingCam_.orthographicSize = parent.orthographicSize;

            // Make sure main camera does not render lighting
            lightingCam_.cullingMask = LightingLayer.value;
            if ((parent.cullingMask & LightingLayer.value) != 0)
            {
                Debug.Log("Disabling lighting layer in main camera");
                parent.cullingMask = parent.cullingMask & ~LightingLayer.value;
            }
        }
    }
}
