using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SltUtil
{
    public static void AddToPosition(Transform transform, in Vector2 delta)
    {
        Vector3 pos = transform.position;
        pos.x += delta.x;
        pos.y += delta.y;
        transform.position = pos;
    }

    public static bool IsValidIndex<T>(List<T> list, int index)
    {
        if (list == null)
        {
            return false;
        }
        if (0 <= index && index < list.Count)
        {
            return true;
        }
        return false;
    }

    public static void ShiftListElement<T>(ref List<T> list, int delta)
    {
        if (delta > 0)
        {
            for (int index = list.Count - 1; index >= 0; index--)
            {
                if (list.IsValidIndex(index - delta))
                {
                    list[index] = list[index - delta];
                }
                else
                {
                    list[index] = default;
                }
            }
        }
        else if (delta < 0)
        {
            for (int index = 0; index < list.Count; index++)
            {
                if (list.IsValidIndex(index - delta))
                {
                    list[index] = list[index - delta];
                }
                else
                {
                    list[index] = default;
                }
            }
        }
    }

    public static void ResizeList<T>(ref List<T> list, int size)
    {
        if (list.Count > size)
        {
            list.RemoveRange(size, list.Count - size);
        }
        else if (list.Count < size)
        {
            list.AddRange(new T[size - list.Count]);
        }
    }

    public static bool IsPrefab(MonoBehaviour monoBehavior)
    {
        if (monoBehavior == null)
        {
            return false;
        }

        if (monoBehavior.gameObject.scene == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}