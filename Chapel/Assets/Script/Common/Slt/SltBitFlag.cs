using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public struct TSltBitFlag<TEnum> where TEnum : Enum
{
    public TSltBitFlag(int initialFlag = 0)
    {
        _flag = initialFlag;

#if UNITY_EDITOR
        EnumDict = new Dictionary<TEnum, bool>();
#endif
    }

    public void Set(TEnum flag, bool bOn)
    {
        int index = Convert.ToInt32(flag);

        if (bOn)
        {
            _flag |= (1 << index);
        }
        else
        {
            _flag &= ~(1 << index);
        }

#if UNITY_EDITOR
        if (EnumDict == null)
        {
            EnumDict = new Dictionary<TEnum, bool>();
        }

        EnumDict[flag] = bOn;
#endif
    }

    public bool Get(TEnum flag)
    {
        int index = Convert.ToInt32(flag);
        return (_flag & 1 << index) != 0;
    }

    public bool Any()
    {
        return _flag != 0;
    }

    public void Clear()
    {
        _flag = 0;

#if UNITY_EDITOR
        EnumDict.Clear();
#endif
    }

    public string GetString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (TEnum key in Enum.GetValues(typeof(TEnum)))
        {
            String elemStr = new String($"{key}={Get(key)} ");
            sb.Append(elemStr);
        }
        return sb.ToString();
    }

    int _flag;

    // デバッグ表示用
#if UNITY_EDITOR
    Dictionary<TEnum, bool> EnumDict;
#endif
}