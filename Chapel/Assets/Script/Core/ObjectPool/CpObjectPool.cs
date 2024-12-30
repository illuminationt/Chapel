using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpObjectPool : MonoBehaviour
{
    public static CpObjectPool Create()
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
}
