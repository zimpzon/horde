using UnityEngine;

public class SceneGlobals : MonoBehaviour
{
    public static SceneGlobals Instance;

    public PlayerScript PlayerScript;
    public CameraPositioner CameraPositioner;
    public CameraShake CameraShake;
    public MapScript MapScript;
    public SoundManager SoundManager;

    void Awake()
    {
        PlayerScript = FindObjectOfType<PlayerScript>();
        CameraPositioner = FindObjectOfType<CameraPositioner>();
        CameraShake = FindObjectOfType<CameraShake>();
        SoundManager = FindObjectOfType<SoundManager>();
        MapScript = FindObjectOfType<MapScript>();

        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
