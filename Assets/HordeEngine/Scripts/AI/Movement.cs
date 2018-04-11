using UnityEngine;

class FeelingData
{
}

public class Movement : MonoBehaviour
{
    Vector2 desiredPosition;
    Vector2 steeringVector;
    Vector2 currentPosition;

    // if Can see Excitor: Angry  += 1
    // if Has Recent Player Pos: Agry = max(current, PlayerRecentHate)

    // if not resting Tired = min(current, 

    // Social = 

    // Angry: decay 20 sec, curve: 
    // FeelingsEnum { Angry, Scared, Bored, Social, Curious, tired } have an associated position and decay rate and value curve
    // Have a state
    // Utility may change state
    // Components:
    //  ChaseStateComponent (Angry = 1, Curious = 1
    //  IdleStateComponent
    //  ExploreStateComponent
    //  SocialStateComponent

    // StateSelectorComponent, lists possible states.

    // pos -> target

    // If obstacle ahead
    //   Scan left/right until free passage
    //   If no free route possible give up

    // Update()
    //   if angle != target angle Scan towards target angle
    //     if free change angle
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
