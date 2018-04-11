using System;
using UnityEngine;

namespace HordeEngine
{
    public enum FeelingEnum { None, Fight, Flight, Tired, Social, Curious };

    [CreateAssetMenu(fileName = "new FeelingData.asset", menuName = "HordeEngine/AI/Feeling Data", order = 0)]
    public class FeelingData : ScriptableObject
    {
        public FeelingEnum Feeling;
        public float DecayTime;
        public AnimationCurve UtilityCurve;
        public float InStateBonus;
        public Color DebugColor;
        [NonSerialized] public float LinearValue;
        [NonSerialized] public float UtilityValue;
    }
}
