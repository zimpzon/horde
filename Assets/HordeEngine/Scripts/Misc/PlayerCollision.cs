using UnityEngine;

namespace HordeEngine
{
    public static class PlayerCollision
    {
        public static Vector2 PlayerPos;
        public static float PlayerSize;
        public static ActorPhysicsBody PlayerBody;

        public delegate void Callback(ref Projectile p, Vector2 velocity);
        public static Callback OnPlayerCollision;
    }
}
