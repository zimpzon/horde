using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// A GameManager enabling minimal functionality when in edit mode
    /// </summary>
    [ExecuteInEditMode]
    public class EditorGameManager : MonoBehaviour
    {
        void Update()
        {
            if (!Application.isPlaying)
            {
                Global.TimeManager.UpdateTime(Time.deltaTime);
                Global.ComponentUpdater.DoUpdate();
            }
        }

        void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                Global.ComponentUpdater.DoLateUpdate();
            }
        }
    }
}
