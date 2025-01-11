using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpEnemySpawnerManager
{
    public static CpEnemySpawnerManager Create()
    {
        CpEnemySpawnerManager newSpawnerManager = new CpEnemySpawnerManager();
        return newSpawnerManager;
    }
    public static CpEnemySpawnerManager Get()
    {
        return CpGameManager.Instance.EnemySpawnerManager;
    }

    public CpEnemySpawner RequestSpawn(CpEnemySpawnParam param)
    {
        CpEnemySpawner newSpawner = new CpEnemySpawner();
        newSpawner.RequestSpawn(param);
        _enemySpawners.Add(newSpawner);

        return newSpawner;
    }

    List<CpEnemySpawner> _enemySpawners = new List<CpEnemySpawner>();
}
