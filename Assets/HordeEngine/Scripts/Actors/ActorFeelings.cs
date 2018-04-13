using System.Linq;
using UnityEngine;

namespace HordeEngine
{
    /// <summary>
    /// OK.. so... How does a monster shoot while resting? I am modeling 
    /// 
    /// Desires. Evaluate independently. Highest desire get state. BUT, also criteria to allow state change. A default + custom for selected.
    /// Ex. DesireToFlee but suddenly CDs are ready. DesireToAttack might be better. After attack flee is better.
    /// 
    /// DesireToChase = age of loS + health + cd's
    /// 
    /// DesireToAttack = cd's, los, in range
    /// 
    /// DesireToFlee = health, los, cd's, incoming projectiles.
    /// 
    /// </summary>
    /// 
    public interface IFeelingChange
    {
        void FeelingActivated();
        void FeelingLost();
    }

    [RequireComponent(typeof(ActorPhysicsBody))]
    public class ActorFeelings : MonoBehaviour, IComponentUpdate
    {
        public FeelingData[] KnownFeelings;
        public FeelingEnum CurrentFeeling;

        float timeLastUpdate_;
        ActorPhysicsBody body_;

        static int IdCounter = AiBlackboard.ActorFeelingFrameSkipOffset;
        int myId_ = IdCounter++;

        void OnEnable() { Horde.ComponentUpdater.RegisterForUpdate(this, ComponentUpdatePass.Default); }
        void OnDisable() { Horde.ComponentUpdater.UnregisterForUpdate(this, ComponentUpdatePass.Default); }

        void Awake()
        {
            timeLastUpdate_ = Horde.Time.SlowableTime;
            body_ = GetComponent<ActorPhysicsBody>();
        }

        public bool TryUpdateFeeling(FeelingEnum feeling, float amount, float clampValue = 1.0f)
        {
            clampValue = Mathf.Min(1.0f, clampValue);

            var f = KnownFeelings.Where(fd => fd.Feeling == feeling).FirstOrDefault();
            if (f != null)
            {
                f.LinearValue = Mathf.Clamp(f.LinearValue + amount, 0.0f, clampValue);
                return true;
            }
            else
            {
                Debug.LogFormat("Feeling {0} not found", feeling); // This is not necessarily an error but for now I need the debug output
                return false;
            }
        }

        FeelingEnum GetDominantFeeling(FeelingEnum currentFeeling)
        {
            float dt = Horde.Time.SlowableTime - timeLastUpdate_;
            UpdateFeelings(dt);
            timeLastUpdate_ = Horde.Time.SlowableTime;

            FeelingEnum result = FeelingEnum.None;
            float highestScore = float.MinValue;

            foreach (var f in KnownFeelings)
            {
                bool isCurrentFeeling = f.Feeling == currentFeeling;
                f.UtilityValue = f.UtilityCurve.Evaluate(f.LinearValue) + (isCurrentFeeling ? f.InStateBonus : 0.0f);
                if (f.UtilityValue > highestScore)
                {
                    highestScore = f.UtilityValue;
                    result = f.Feeling;
                }
            }

            Debug.LogFormat("Dominant: {0} : {1}", result, highestScore);
            return result;
        }

        void UpdateFeelings(float dt)
        {
            foreach(var f in KnownFeelings)
                f.LinearValue = Mathf.Clamp01(f.LinearValue - (1.0f / f.DecayTime) * dt);
        }

        void OnDrawGizmos()
        {
            if (body_ == null)
                return;

            var pos = body_.Position;
            pos.y += 0.6f;
            for (int i = 0; i < KnownFeelings.Length; ++i)
            {
                var f = KnownFeelings[i];
                Vector3 size = new Vector3(1.2f, 0.2f, 0.1f);
                Gizmos.color = Color.black;
                Gizmos.DrawCube(pos, size);

                size.x *= f.UtilityValue;
                Gizmos.color = f.DebugColor;
                Gizmos.DrawCube(pos, size);
                pos.y += 0.25f;
            }
        }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            bool checkNow = (Time.frameCount + myId_) % AiBlackboard.ActorFeelingFrameSkip == 0;
            if (checkNow)
            {
                CurrentFeeling = GetDominantFeeling(CurrentFeeling);
            }
        }
    }
}
