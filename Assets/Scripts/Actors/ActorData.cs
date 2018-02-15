using UnityEngine;

/*

Looping through data
------------------------------------------

Controllers not running every frame.
CheapScheduler class holding timestamp and some identification.
	Are we then yet? Cyclic buffer, only checking top?

Movement towards moving target (chase player, is that all?)
	Does not have to update every frame

Physics, force + friction + possibly gravity if we are cool.

Movement in static direction (very, very common, even with AI. Dumb frames)
    Pos, vel

Static collision (very, very common. Might be able to skip some frames if not moving too fast. Will I hit a wall in next step?)
    Circle and/or box.
    Ray casting static colliders (can I see player? Can I hide over there?)
    Avoiding corners when chasing?
    CircleOverlap, get all in circle
    ConeOverlap (partial circle), melee attack?
    In-front-of, interaction with items, doors, etc.

Dynamic collision (very, very common. Projectile, enemy, player, allied, props). Layers?

Dynamic meshes (all dynamic sprites. Projectiles, enemies, players, dropshadows, weapons, props, lights, items, chests, doors)

Dynamic map updates?

Specials: health regen (update rate?)

Summary:

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
