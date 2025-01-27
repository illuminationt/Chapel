using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix;
using Sirenix.OdinInspector;
using UnityEditor;


[System.Serializable]
public class CpHellRequestOption
{
    public float AdditionalFirstDelay = 0f;
    public float DeltaDegree = 0f;
}

[System.Serializable]
public class CpHellParamListElement
{
#if CP_EDITOR
    public string Comment;
#endif

    public CpHellParamScriptableObject Setting = null;
    public CpHellRequestOption Option;
}
[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpHellParamList")]
public class CpHellParamListScriptableObject : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;

    [Button]
    void TestHell()
    {
        CpDebugParam.bEnableHellTest = true;
        CpDebugParam.TestHellListScriptableObject = this;
        CpDebugParam.HellTestObjectNormalizedPosition = HellTestObjectNormalizedPosition;

        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    public Vector2 HellTestObjectNormalizedPosition = new Vector2(0.5f, 0.85f);
#endif

    public List<CpHellParamListElement> HellParamList => _hellParamList;
    [SerializeField]
    List<CpHellParamListElement> _hellParamList = new List<CpHellParamListElement>();
}
