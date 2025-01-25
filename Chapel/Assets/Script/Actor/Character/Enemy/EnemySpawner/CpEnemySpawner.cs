using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpEnemySpawner
{
    public static CpEnemySpawner Create()
    {
        CpEnemySpawner enemySpawner = new CpEnemySpawner();
        enemySpawner.Initialize();
        return enemySpawner;
    }

    void Initialize()
    {
        CpEnemyBase.OnEnemyDead.AddListener(OnEnemyDead);
    }

    void OnEnemyDead(CpEnemyBase enemy) { }


    public void RequestSpawn(CpEnemySpawnParam param)
    {
        foreach (CpEnemySpawnParamElement element in param.Elements)
        {
            if (!element.bEnable)
            {
                continue;
            }
            SpawnEnemy(element);
        }
    }

    public bool IsAllEnemyDead()
    {
        int count = _spawnedEnemyList.Count;
        for (int index = 0; index < count; index++)
        {
            CpEnemyBase enemy = _spawnedEnemyList[index];
            if (enemy == null) { continue; }

            GameObject go = enemy.gameObject;
            if (go == null)
            {
                continue;
            }

            return false;
        }

        return true;
    }

    void SpawnEnemy(CpEnemySpawnParamElement element)
    {
        CpEnemyBase newEnemy = MonoBehaviour.Instantiate(element.Prefab);
        newEnemy.gameObject.SetActive(false);

        FCpEnemyInitializeParam initParam;
        initParam.OwnerSpawner = this;
        initParam.EnemySpecificParam = element.SpecificParam;
        initParam.LocationParam = element.LocationParam;
        newEnemy.InitializeEnemy(initParam);
        _spawnedEnemyList.Add(newEnemy);
    }

    List<CpEnemyBase> _spawnedEnemyList = new List<CpEnemyBase>();
}
