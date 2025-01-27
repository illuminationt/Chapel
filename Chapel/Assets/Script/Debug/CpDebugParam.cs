using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if CP_EDITOR

public static class CpDebugParam
{
    public static void Reset()
    {
        bEnableHellTest = false;
        TestHellParamScriptableObject = null;
        TestHellListScriptableObject = null;

        HellTestObjectNormalizedPosition = Vector2.zero;


        bEnableEnemyTest = false;
        TestEnemyScriptableObject = null;
    }

    // �e���̃e�X�g
    // CpHellParamScriptableObject
    public static bool bEnableHellTest = false;
    public static CpHellParamScriptableObject TestHellParamScriptableObject = null;
    public static CpHellParamListScriptableObject TestHellListScriptableObject = null;
    public static Vector2 HellTestObjectNormalizedPosition;
    // CpHellParamListScriptableObject

    // �G�l�~�[�̃e�X�g
    public static bool bEnableEnemyTest = false;
    public static CpEnemySpawnParamScriptableObject TestEnemyScriptableObject = null;
}

#endif