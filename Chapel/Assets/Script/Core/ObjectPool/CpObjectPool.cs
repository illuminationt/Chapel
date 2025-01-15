using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public interface ICpPoolable : ISltPoolable
{
}

public class CpPooledPlayerShot : SltObjects<CpPlayerShot> { }
public class CpPlayerShotPool : SltObjectPool<CpPooledPlayerShot, CpPlayerShot> { }

public class CpPooledEnemyShot : SltObjects<CpEnemyShot> { }
public class CpEnemyShotPool : SltObjectPool<CpPooledEnemyShot, CpEnemyShot> { }



public class CpObjectPool : MonoBehaviour
{
    // Ž©‹@
    public static CpObjectPool CreateObjectPool()
    {
        GameObject Obj = new GameObject("CpObjectPool", typeof(CpObjectPool));
        DontDestroyOnLoad(Obj);
        Obj.SetActive(true);

        CpObjectPool pool = Obj.GetComponent<CpObjectPool>();
        pool.enabled = true;

        return pool;
    }

    public static CpObjectPool Get() => CpGameManager.Instance.ObjectPool;

    public CpPlayerShot Get(CpPlayerShot prefab) { return _playerShotPool.Get(prefab); }
    public void Release(CpPlayerShot instance) { _playerShotPool.Release(instance); }

    public CpEnemyShot Get(CpEnemyShot prefab) { return _enemyShotPool.Get(prefab); }
    public void Release(CpEnemyShot instance) { _enemyShotPool.Release(instance); }

    CpPlayerShotPool _playerShotPool = new CpPlayerShotPool();
    CpEnemyShotPool _enemyShotPool = new CpEnemyShotPool();
}
