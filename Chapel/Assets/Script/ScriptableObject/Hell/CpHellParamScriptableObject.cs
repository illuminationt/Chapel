using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEditor;


[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpHellParam")]
public class CpHellParamScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    public string Comment = null;


    [Button]
    void OnTestButtonClicked()
    {
        CpDebugParam.bEnableHellTest = true;
        CpDebugParam.TestHellParamScriptableObject = this;
        CpDebugParam.HellTestObjectNormalizedPosition = HellTestObjectNormalizedPosition;

        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    public Vector2 HellTestObjectNormalizedPosition = new Vector2(0.5f, 0.85f);
#endif

    public CpMultiHellParam MultiHellParam;
}
