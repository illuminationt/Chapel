using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public static class CpDebugParam
{
    public static void Reset()
    {
        bEnableHellTest = false;
        TestHellParamScriptableObject = null;
        HellTestObjectNormalizedPosition = Vector2.zero;
    }
    public static bool bEnableHellTest = false;
    public static CpHellParamScriptableObject TestHellParamScriptableObject = null;
    public static Vector2 HellTestObjectNormalizedPosition;
}

#endif