using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Rendering: gameobjects
// Collision: spatial hash with chunked lists from preallocated list.
// Script: NOT on all gameobjects. Systems.

// Pos, radius, layer, reference
// 
// Tightly packed for locality of data
// Free reset, then add all
// GetClosestX, GetX, Any
public class SpatialHash
{
//    Dictionary<int, List<>>
}
