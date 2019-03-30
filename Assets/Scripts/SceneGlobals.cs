using UnityEngine;

public class SceneGlobals : MonoBehaviour
{
    public static SceneGlobals Instance;

    public PlayerScript PlayerScript;
    public CameraPositioner CameraPositioner;
    public CameraShake CameraShake;
    public MapScript MapScript;
    public SoundManager SoundManager;
    public ParticleScript ParticleScript;

    void Awake()
    {
        PlayerScript = FindObjectOfType<PlayerScript>();
        CameraPositioner = FindObjectOfType<CameraPositioner>();
        CameraShake = FindObjectOfType<CameraShake>();
        SoundManager = FindObjectOfType<SoundManager>();
        MapScript = FindObjectOfType<MapScript>();
        ParticleScript = FindObjectOfType<ParticleScript>();

        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
