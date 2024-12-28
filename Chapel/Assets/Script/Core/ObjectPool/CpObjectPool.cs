using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpObjectPool : SingletonMonoBehaviour<CpObjectPool>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        GameObject Obj = new GameObject("CpObjectPool", typeof(CpObjectPool));
        DontDestroyOnLoad(Obj);
        Obj.SetActive(true);

        CpObjectPool CtrlManager = Obj.GetComponent<CpObjectPool>();
        CtrlManager.enabled = true;
    }
    public static CpObjectPool Get() => CpObjectPool.Instance;

    public T Create<T>(T obj) where T : MonoBehaviour
    {
        T newObj = Instantiate<T>(obj);
        return newObj;
    }
}
