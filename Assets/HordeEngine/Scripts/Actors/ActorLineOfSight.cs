using System;
using UnityEngine;

namespace HordeEngine
{
    [RequireComponent(typeof(ActorPhysicsBody))]
    public class ActorLineOfSight : MonoBehaviour, IComponentUpdate
    {
        public ActorPhysicsBody Target;
        public float RequiredWidth = 0.1f;
        [NonSerialized] public bool HasLineOfSight;
        [NonSerialized] public Vector2 LatestLineOfSightPosition;
        [NonSerialized] public float LatestLineOfSightTime;

        static int IdCounter = 0;
        int myId_ = IdCounter++;

        ActorPhysicsBody body_;

        void Awake()
        {
            body_ = GetComponent<ActorPhysicsBody>();
        }

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            bool checkNow = (Time.frameCount + myId_) % AiBlackboard.LineOfSightCheckFrameSkipMod == 0;
            if (checkNow)
            {
                var p0 = body_.Position;
                var p1 = Target.Position;

                bool hasLoS = CollisionUtil.CircleCast(p0, p1, RequiredWidth, allowPartial: true);
                if (hasLoS)
                {
                    LatestLineOfSightPosition = p1;
                    LatestLineOfSightTime = Horde.Time.Time;
                    Debug.DrawLine(p0, p1, Color.green, 0.32f);
                }
                if (!hasLoS)
                    Debug.DrawLine(p0, LatestLineOfSightPosition, Color.yellow, 0.32f);
            }
        }
    }
}
