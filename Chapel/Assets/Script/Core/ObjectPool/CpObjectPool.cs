using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


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

    public T Create<T>(T obj) where T : MonoBehaviour
    {
        T newObj = Instantiate<T>(obj);
        return newObj;
    }
    public GameObject Create(GameObject objPrefab)
    {
        GameObject newObj = Instantiate(objPrefab);
        return newObj;
    }

    Dictionary<CpPlayerShot, ObjectPool<CpPlayerShot>> _playerShotPool;
}
