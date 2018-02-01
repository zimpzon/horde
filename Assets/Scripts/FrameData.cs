using Unity.Collections;
using UnityEngine;

namespace HordeEngine
{
    // Data that will be updated at the beginning of every rendering frame.
    public class FrameData
    {
        public static float Time;
        public static float DeltaTime;

        public static void UpdateTime()
        {
            Time = UnityEngine.Time.time;
            DeltaTime = UnityEngine.Time.deltaTime;
        }
    }
}
