using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpEnemySpawnParamScriptableObject")]
public class CpEnemySpawnParamScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;
#endif

    public CpEnemySpawnParam EnemySpawnParam => _enemySpawnParam;

    [SerializeField]
    CpEnemySpawnParam _enemySpawnParam = null;
}
