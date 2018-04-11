using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    public class AiBlackboard
    {
        // FrameSkip = how often to check line of sight. Every X frames.
        // FrameSkipOffset = offset the categories so its even more spread out
        public const int LineOfSightFrameSkip = 20;
        public const int LineOfSightFrameSkipOffset = 0;

        public const int ActorFeelingFrameSkip = 20;
        public const int ActorFeelingFrameSkipOffset = 10;

        public Vector2 PlayerPos;
        public List<ActorPhysicsBody> HasPlayerLoS;
    }
}
