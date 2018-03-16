using System.Collections.Generic;
using UnityEngine;

namespace HordeEngine
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get layer number from mask ([0..31]). Only one layer must be present in mask.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static int GetSingleLayerNumber(this LayerMask mask)
        {
            int bitmask = mask.value;
            int result = bitmask > 0 ? 0 : 31;
            while (bitmask > 1)
            {
                bitmask = bitmask >> 1;
                result++;
            }
            return result;
        }

        /// <summary>
        /// Remove item from list by replacing it with the last item in the last
        /// </summary>
        public static void ReplaceRemove<T>(this IList<T> list, T item)
        {
            int idxToRemove = list.IndexOf(item);
            list[idxToRemove] = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
        }
    }
}
