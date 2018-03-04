using TMPro;
using UnityEngine;

namespace HordeEngine
{
    public class SceneAccessScript : MonoBehaviour
    {
        public TextMeshProUGUI DebugText;
        public MapRenderer MapRenderer;
        public MiniMapScript MiniMap;

        void Awake()
        {
            Global.SceneAccess = this;
        }
    }
}
