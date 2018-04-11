using System.Linq;
using UnityEngine;

namespace HordeEngine
{
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

        /// <summary>
        /// Gradual influence like getting tired when running
        /// </summary>
        /// <param name="feeling"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryAddToFeeling(FeelingEnum feeling, float amount, float clampValue = 1.0f)
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

            FeelingEnum result = FeelingEnum.None;
            float highestScore = float.MinValue;

            foreach (var f in KnownFeelings)
            {
                f.LinearValue = Mathf.Clamp01(f.LinearValue - f.DecayTime * dt);

                bool isCurrentFeeling = f.Feeling == currentFeeling;
                f.UtilityValue = f.UtilityCurve.Evaluate(f.LinearValue) + (isCurrentFeeling ? f.InStateBonus : 0.0f);
                if (f.UtilityValue > highestScore)
                {
                    highestScore = f.UtilityValue;
                    result = f.Feeling;
                }
            }

            return result;
        }

        void UpdateFeelings(float dt)
        {
            foreach(var feeling in KnownFeelings)
            {
                feeling.LinearValue = Mathf.Clamp01(feeling.LinearValue - feeling.DecayTime * dt);
            }
        }

        void ShowDebug()
        {
            var pos = body_.Position;
            pos.y += 1.5f;
            for (int i = 0; i < KnownFeelings.Length; ++i)
            {
                var f = KnownFeelings[i];
                Debug.DrawRay(pos, Vector3.right * 0.5f, Color.black);
                Debug.DrawRay(pos, Vector3.right * 0.5f, f.DebugColor);
            }
        }

        public void ComponentUpdate(ComponentUpdatePass pass)
        {
            ShowDebug();

            bool checkNow = (Time.frameCount + myId_) % AiBlackboard.ActorFeelingFrameSkip == 0;
            if (checkNow)
            {
                CurrentFeeling = GetDominantFeeling(CurrentFeeling);
            }
        }
    }
}
