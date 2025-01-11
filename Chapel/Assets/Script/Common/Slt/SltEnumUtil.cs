using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SltEnumUtil
{
    public static int GetNum<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T)).Length;
    }
    public static T GetNextEnumValue<T>(T value) where T : Enum
    {
        T[] values = (T[])Enum.GetValues(typeof(T));
        int currentIndex = Array.IndexOf(values, value);
        int nextIndex = (currentIndex + 1) % values.Length; // 次のインデックス（ループする）
        return values[nextIndex];
    }

    public static string ToString<T>(T value) where T : Enum
    {
        return value.ToString();
    }
    public static string ToString<T>(int index) where T : Enum
    {
        T value = GetValue<T>(index);
        return ToString(value);
    }

    public static void FillDictionary<TEnum, S>(ref Dictionary<TEnum, S> dict) where TEnum : Enum
    {
        //if (dict == null)
        //{
        //    dict = new Dictionary<TEnum, S>();
        //}

        //foreach (TEnum enumKey in Enum.GetValues(typeof())
        //{

        //}
    }

    public static T GetValue<T>(int index) where T : Enum
    {
        List<T> values = GetAllValues<T>();
        if (0 <= index && index < values.Count)
        {
            return values[index];
        }
        return default;
    }
    public static List<T> GetAllValues<T>() where T : Enum
    {
        return new List<T>((T[])Enum.GetValues(typeof(T)));
    }

}
