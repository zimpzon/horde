using System.Collections.Generic;

public static class ListExtensions
{
    /// <summary>
    /// Remove item by replacing it with the last item in the last
    /// </summary>
    public static void ReplaceRemove<T>(this IList<T> list, T item)
    {
        int idxToRemove = list.IndexOf(item);
        list[idxToRemove] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }
}
