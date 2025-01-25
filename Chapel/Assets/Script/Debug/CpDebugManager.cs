using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpDebugManager : MonoBehaviour
{
#if CP_DEBUG

    private void Start()
    {
        if (CpDebugParam.bEnableHellTest)
        {
            CpDungeonManager.CreateDummyRoom();
            SltDelay.Delay(this, 1f, StartHellTest);
        }

        if (CpDebugParam.bEnableEnemyTest)
        {
            CpDungeonManager.CreateDummyRoom();
            SltDelay.Delay(this, 1f, StartEnemySpawnTest);
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

    void StartEnemySpawnTest()
    {
        CpEnemySpawnerManager spawnerManager = CpEnemySpawnerManager.Get();
        spawnerManager.RequestSpawn(CpDebugParam.TestEnemyScriptableObject.EnemySpawnParam);
    }

#endif
}
