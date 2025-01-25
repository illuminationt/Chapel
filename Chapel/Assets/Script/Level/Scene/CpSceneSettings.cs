using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[PreferBinarySerialization]
[CreateAssetMenu(menuName = "ScriptableObject/CpSceneScriptableObject")]
public class CpSceneSettings : ScriptableObject
{
#if CP_EDITOR
    [TextArea]
    public string Comment = null;
#endif
    public static CpSceneSettings Get() => CpGameSettings.Get().SceneSettings;

    public LoadSceneMode GetLoadSceneMode(ECpSceneType sceneType)
    {
        CpSceneParam sceneParam = FindSceneParam(sceneType);
        return sceneParam.LoadSceneMode;
    }
    public string GetSceneName(ECpSceneType sceneType)
    {
        CpSceneParam sceneParam = FindSceneParam(sceneType);
        return sceneParam.SceneName;
    }

    CpSceneParam FindSceneParam(ECpSceneType sceneType)
    {
        for (int i = 0; i < SceneParams.Count; i++)
        {
            if (SceneParams[i].SceneType == sceneType)
            {
                return SceneParams[i];
            }
        }

        Assert.IsTrue(false);
        return null;
    }


    [SerializeField]
    List<CpSceneParam> SceneParams = new List<CpSceneParam>();
}
