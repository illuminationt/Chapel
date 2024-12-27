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

    public static String ToString<T>(T value) where T : Enum
    {
        return value.ToString();
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


}
