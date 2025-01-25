using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ListExtensions
{
    public static bool IsValidIndex<T>(this List<T> self, int index)
    {
        if (0 <= index && index < self.Count)
        {
            return true;
        }
        return false;
    }

    public static T Random<T>(this List<T> self) where T : class
    {
        int num = self.Count;
        if (num == 0)
        {
            return null;
        }

        int index = UnityEngine.Random.Range(0, num - 1);
        return self[index];
    }

}