using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    /*
        1) Critical path: spatial Actor/Actor queries
            query types: circle, cone, line?
            CRITICAL PATH could be handled by storing id, layer and bounding circle in spatial hash table.

    spatial collision one array
    pos
    bounds
    layer


    rendering one array
    sprite/uv
    color
    rotation
    scale


    Item
    ----
    Icon
    Position
    in chest, on player


       props are decals?
    */

    class SpatialHash
    {
        public void Reset()
        {

        }

        public void AddActor(int actorId)
        {

        }
    }

    class RenderSystem
    {
        // Material (same for all?), color, sprite/uv, TRS?
    }
//  
struct ComponentPositionSize
{

}

// Multiple linked sprites. Body, shadow, gun, hands, head, possibly many more.
class Actors
{
    // Likely multiple hits per frame
    struct Hot
    {
        public int ActorId; // Implicit?
        public Vector2 Position;
        public Vector2 Size;
        public int Layer;
    }

    // Likely one hit per frame
    struct Common
    {
        // id is implicitly position in list
        float Rotation;
        float Z;
        Color Color;
        int SpriteId; // need sprite list (idle, running) and T (0..1 inside list)
    }

    ComponentPositionSize[] HotData; //
    Common[] CommonData;
    //Dictionary<int, >

     //   type?
     //   rot
     //z
     //velocity
     //color
     //spriteId
    }
}
