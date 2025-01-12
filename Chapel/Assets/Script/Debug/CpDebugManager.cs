using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpDebugManager : MonoBehaviour
{
    private void Start()
    {
        if (CpDebugParam.bEnableHellTest)
        {
            StartHellTest();
        }
    }

    void StartHellTest()
    {
        CpHellTestObject prefab = CpPrefabSettings.Get().HellTestObjectPrefab;
        CpHellTestObject obj = Instantiate(prefab);
        Vector2 position = CpUtil.GetWorldPositionFromNormalizedPosition(CpDebugParam.HellTestObjectNormalizedPosition);
        obj.transform.position = position;

        obj.RequestStartHell(CpDebugParam.TestHellParamScriptableObject);
    }
}
