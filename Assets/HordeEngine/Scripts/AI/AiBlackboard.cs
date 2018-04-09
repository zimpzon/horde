using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    public class AiBlackboard
    {
        public const int LineOfSightCheckFrameSkipMod = 20; // How often to check line of sight. Every X frames.

        public Vector2 PlayerPos;
        public List<ActorPhysicsBody> HasPlayerLoS;
    }
}
