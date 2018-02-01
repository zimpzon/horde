
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HordeEngine
{
    public class JsonToMap
    {
        public static VirtualMap Convert()
        {
            var result = new VirtualMap(Vector2Int.one * 16, Vector2Int.zero);
            return result;
        }
    }
}
