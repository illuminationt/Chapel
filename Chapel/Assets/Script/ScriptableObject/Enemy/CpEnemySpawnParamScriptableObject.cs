using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpEnemySpawnParamScriptableObject")]
public class CpEnemySpawnParamScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;

    [Button]
    void TestEnemySpawn()
    {
        CpDebugParam.bEnableEnemyTest = true;
        CpDebugParam.TestEnemyScriptableObject = this;

        EditorApplication.ExecuteMenuItem("Edit/Play");

    }
#endif

    public CpEnemySpawnParam EnemySpawnParam => _enemySpawnParam;

    [SerializeField]
    CpEnemySpawnParam _enemySpawnParam = null;
}
