using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

#if CP_DEBUG
using ImGuiNET;
#endif

[System.Serializable]
public struct TSltBitFlag<TEnum> where TEnum : Enum
{
    public TSltBitFlag(int initialFlag = 0)
    {
        _flag = initialFlag;

#if CP_EDITOR
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

#if CP_EDITOR
        if (EnumDict == null)
        {
            EnumDict = new Dictionary<TEnum, bool>();
        }

        EnumDict[flag] = bOn;
#endif
    }

    public void Fill(bool bFlag)
    {
        if (bFlag)
        {
            _flag = int.MaxValue;
        }
        else
        {
            _flag = 0;
        }
    }

    public bool Get(TEnum flag)
    {
        int index = Convert.ToInt32(flag);
        return Get(index);
    }
    bool Get(int index)
    {
        return (_flag & 1 << index) != 0;
    }

    public bool EqualsFlag(TEnum flag, bool bValue)
    {
        return Get(flag) == bValue;
    }

    public bool Any()
    {
        return _flag != 0;
    }

    public void Clear()
    {
        _flag = 0;

#if CP_EDITOR
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

    [SerializeField]
    int _flag;


    // デバッグ表示用
#if CP_EDITOR
    Dictionary<TEnum, bool> EnumDict;
#endif

#if CP_DEBUG
    public void DrawImGui(string label)
    {
        ImGui.Text(label);

        List<TEnum> enums = SltEnumUtil.GetAllValues<TEnum>();
        for (int index = 0; index < enums.Count; index++)
        {
            bool bValue = Get(index);
            string valueName = SltEnumUtil.ToString(enums[index]);
            ImGui.Checkbox(valueName, ref bValue);
            Set(enums[index], bValue);
        }
    }
#endif
}