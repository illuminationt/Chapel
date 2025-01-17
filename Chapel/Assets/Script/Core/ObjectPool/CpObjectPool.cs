using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public interface ICpPoolable : ISltPoolable
{
}

public class CpPlayerShotPool : SltObjectPool<SltObjects<CpPlayerShot>, CpPlayerShot> { }
public class CpEnemyShotPool : SltObjectPool<SltObjects<CpEnemyShot>, CpEnemyShot> { }

public class CpDropItemPool : SltObjectPool<SltObjects<CpDropItem>, CpDropItem> { }

public class CpObjectPool : MonoBehaviour
{
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
    public UnityEvent<CpPlayerShot> OnPlayerShotActivated => _playerShotPool.OnObjectActivated;
    public UnityEvent<CpPlayerShot> OnPlayerShotReleased => _playerShotPool.OnObjectReleased;
    CpPlayerShotPool _playerShotPool = new CpPlayerShotPool();


    public CpEnemyShot Get(CpEnemyShot prefab) { return _enemyShotPool.Get(prefab); }
    public void Release(CpEnemyShot instance) { _enemyShotPool.Release(instance); }
    public UnityEvent<CpEnemyShot> OnEnemyShotActivated => _enemyShotPool.OnObjectActivated;
    public UnityEvent<CpEnemyShot> OnEnemyShotReleased => _enemyShotPool.OnObjectReleased;

    CpEnemyShotPool _enemyShotPool = new CpEnemyShotPool();


    public CpDropItem Get(CpDropItem prefab) { return _dropItemPool.Get(prefab); }
    public void Release(CpDropItem instance) { _dropItemPool.Release(instance); }
    public UnityEvent<CpDropItem> OnDropItemActivated => _dropItemPool.OnObjectActivated;
    public UnityEvent<CpDropItem> OnDropItemReleased => _dropItemPool.OnObjectReleased;
    CpDropItemPool _dropItemPool = new CpDropItemPool();

}
