using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpEnemySpawner
{
    public void RequestSpawn(CpEnemySpawnParam param)
    {
        foreach (CpEnemySpawnParamElement element in param.Elements)
        {
            SpawnEnemy(element);
        }
    }

    void SpawnEnemy(CpEnemySpawnParamElement element)
    {
        CpEnemyBase newEnemy = MonoBehaviour.Instantiate(element.Prefab);

        FCpEnemyInitializeParam initParam;
        initParam.EnemySpecificParam = element.SpecificParam;
        initParam.LocationParam = element.LocationParam;
        newEnemy.InitializeEnemy(initParam);
        SpawnedEnemyList.Add(newEnemy);
    }

    List<CpEnemyBase> SpawnedEnemyList = new List<CpEnemyBase>();
}
