﻿using System;
using System.Collections.Generic;

namespace HordeEngine
{
    public enum DoorFacing { Top, Right, Down, Left };

    public class RoomWrapper
    {
        public List<Room> Rooms = new List<Room>();
    }

    /// <summary>
    /// Position and facing of a door in a room.
    /// </summary>
    [Serializable]
    public struct Door
    {
        public DoorFacing Facing;
        public int RoomX;
        public int RoomY;
    }

    /// <summary>
    /// Immutable (not enforced) prototype data for a single non-connected room.
    /// </summary>
    [Serializable]
    public class Room
    {
        public string Name;
        public int[] FloorTiles;
        public int[] WallTiles;
        public int[] PropTiles;
        public object[] Objects;
        public int[] Doors;

        public int Width;
        public int Height;

        public static Door[] FindDoors(Room room)
        {
            return null;
        }
    }
}
