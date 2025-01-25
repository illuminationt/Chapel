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
        HellTestObjectNormalizedPosition = Vector2.zero;

        bEnableEnemyTest = false;
        TestEnemyScriptableObject = null;
    }

    // �e���̃e�X�g
    public static bool bEnableHellTest = false;
    public static CpHellParamScriptableObject TestHellParamScriptableObject = null;
    public static Vector2 HellTestObjectNormalizedPosition;


    // �G�l�~�[�̃e�X�g
    public static bool bEnableEnemyTest = false;
    public static CpEnemySpawnParamScriptableObject TestEnemyScriptableObject = null;
}

#endif