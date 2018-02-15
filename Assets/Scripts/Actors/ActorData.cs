using UnityEngine;

/*
    Data-orientated
    Dooldown/timers: keep a sorted list and just check top every frame.
    Health regen: collection of damaged entities. No checking when full health.
    Possibly build list of items to be processed, then process them all.
        Better cache than checking flag then process?


    props are decals?
*/

// Player
// Enemy (from simplest to complex boss)
// NPC
// Player ghosts
// Bullet
// pickup item
// Signs?
// Text on walls

// Door
// Chest
// Dymamic obstacle? Barrel?
// Destructable object (chairs etc.)
// Turret/flame thrower/laser/timed spikes
// Trap
// Dynamic stationary light (destructable, flashing)
// Entrance (like stairs down, portal, etc.)


// Test if struct works out

// Multiple linked sprites. Body, shadow, gun, hands, head, possibly many more.
struct ActorRenderData
{
    Matrix4x4 Matrix;
    public Color Color;
    public Vector2 Size;
}
