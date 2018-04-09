using UnityEngine;

public struct GradientRange
{
    // Gradient (r0 -> r1)
    // Sustain  (r1 -> r2)
    // Gradient (r2 -> r3)
    public float r0;
    public float r1;
    public float r2;
    public float r3;

    public static GradientRange AnyRange()
    {
        return new GradientRange { r0 = float.MaxValue, r1 = float.MaxValue, r2 = 0.0f, r3 = 0.0f };
    }
}

// Actions
// ActorChaseTarget (Target, LastSeenPos, LastSeenTime, DesiredRange) - NB target can be friend if we have nothing and they are agitated
// ActorIdle (Friends nearby, move around)
public class PerceptionBase<T>
{
    // Decrease over time
    float desireFlee;
    float desireChase;

    // Increase over time
    float desireSocial;
    float desireExplore;
    float desireRest;

    // Add every update update (* dt) or at events.
    // Normalized [0..1]
    // Curves where applicable
    float inputSocial; // Friends nearby, ex. 0 = 0, 3 = 1
    float inputDanger; // Incoming bullets + player distance
    float inputAggro; // Recent player LoS normalized, ex. 1 = LoS, 0.5 = 5 sec since LoS, 0 = 10 sec since LoS
    float inputCuriosity; // Static low input
    float inputHealth; // % * curve
    float inputFear; // nearby friend death
    float life = 0;
    float offensiveAbility;
    float fear = 0;
    float aggro = 0;
    float curious = 0;
}
