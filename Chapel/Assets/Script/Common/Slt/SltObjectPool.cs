using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public interface ISltPoolable
{
    public ISltPoolable Instantiate(ISltPoolable prefab);
    public GameObject GetPooledGameObject();
    public Transform GetPooledTransform();
    public int GetPoolInstanceId();
    public void SetPoolInstanceId(int instanceId);
    // プールに戻すときのリセット処理
    public void ResetOnRelease();

    public void SetActive(bool bActive)
    {
        GetPooledGameObject().SetActive(bActive);

        if (bActive)
        {

        }
        else
        {
            GetPooledTransform().SetParent(null);
        }
    }

    public bool EqualsInstanceId(ISltPoolable other)
    {
        return GetPoolInstanceId() == other.GetPoolInstanceId();
    }

    // 
    public void OnActivated() { }
    public void OnReleased() { }
}

public class SltObjects<T> where T : MonoBehaviour, ISltPoolable, new()
{

    public SltObjects()
    {
    }

    // return:Instantiateしたなら true
    public bool Get(out T Instance, bool bActive = true)
    {
        bool bInstantiate = false;
        if (Instances.Count == 0)
        {
            // 新しく作成
            Instance = GameObject.Instantiate(Prefab);

            Instance.SetPoolInstanceId(Prefab.GetPoolInstanceId());

            bInstantiate = true;
        }
        else
        {
            Instance = Instances.Pop();
            if (Instance == null)
            {
                Get(out Instance, bActive);
            }
            bInstantiate = false;
        }

        if (bActive)
        {
            Instance.SetActive(true);
            //  Instance.ResetOnGetFromObjectPool();
        }

        if (bUseActiveInstancesList)
        {
            ActiveInstances.Add(Instance);
        }
        return bInstantiate;
    }

    public void Release(T Instance)
    {
        if (Instances.Contains(Instance))
        {
            return;
        }

        Instance.SetActive(false);

        Instances.Push(Instance);

        if (bUseActiveInstancesList)
        {
            ActiveInstances.Remove(Instance);
        }
    }

    public int GetActiveInstancesCount() { return ActiveInstances.Count; }

    public T Prefab;
    public SltObjectPool<SltObjects<T>, T> OwnerPool;
    public bool bUseActiveInstancesList;
    Stack<T> Instances = new Stack<T>();
    // Stackに入っていないInstance（ゲーム中でActiveなInstance）
    List<T> ActiveInstances = new List<T>();
}

public class SltObjectPool<TPool, TPrefab> where TPool : SltObjects<TPrefab>, new() where TPrefab : MonoBehaviour, ISltPoolable, new()
{
    List<TPool> ObjectPools = new List<TPool>();
    public TPrefab Get(TPrefab InPrefab, bool bActive = true)
    {
        TPool Pool = FindPoolFromPrefab(InPrefab);
        TPrefab RetInstance;
        bool bInstantiated;

        if (Pool == null)
        {
            // 初めてInPrefabのインスタンスを作成する
            TPool NewPool = new TPool();
            NewPool.Prefab = InPrefab;
            NewPool.bUseActiveInstancesList = bUseActiveInstancesList;
            ObjectPools.Add(NewPool);

            // インスタンス作成
            bInstantiated = NewPool.Get(out RetInstance, bActive);
        }
        else
        {
            bInstantiated = Pool.Get(out RetInstance, bActive);
        }

        RetInstance.OnActivated();

        if (bInstantiated)
        {
            _onNewObjectCreated.Invoke(RetInstance);
        }
        _onObjectActive.Invoke(RetInstance);
        return RetInstance;
    }

    public bool Release(TPrefab Instance)
    {
        TPool Pool = FindPoolFromInstance(Instance);
#if UNITY_EDITOR
        if (Pool == null)
        {
            Assert.IsTrue(false);
            return false;
        }
#endif
        Instance.ResetOnRelease();
        Instance.OnReleased();
        _onObjectReleased.Invoke(Instance);

        // プールに戻す
        Pool.Release(Instance);

        return true;
    }

    public int GetActiveInstancesCount(TPrefab InPrefab)
    {
        TPool Pool = FindPoolFromPrefab(InPrefab);
        if (Pool == null)
        {
            return 0;
        }

        return Pool.GetActiveInstancesCount();
    }

    public void EnableActiveInstancesList()
    {
        bUseActiveInstancesList = true;
    }

    TPool FindPoolFromInstance(TPrefab Instance)
    {
        for (int j = 0; j < ObjectPools.Count; j++)
        {
            TPrefab Prefab = ObjectPools[j].Prefab;
            if (Instance.EqualsInstanceId(Prefab))
            {
                return ObjectPools[j];
            }
        }
        return null;
    }

    TPool FindPoolFromPrefab(TPrefab InPrefab)
    {
        for (int j = 0; j < ObjectPools.Count; j++)
        {
            TPrefab Prefab = ObjectPools[j].Prefab;
            if (Prefab.GetPoolInstanceId() == InPrefab.GetPoolInstanceId())
            {
                return ObjectPools[j];
            }
        }
        return null;
    }


    public bool bUseActiveInstancesList { get; private set; }

    public UnityEvent<TPrefab> OnNewObjectCreated => _onNewObjectCreated;
    public UnityEvent<TPrefab> OnObjectActivated => _onObjectActive;
    public UnityEvent<TPrefab> OnObjectReleased => _onObjectReleased;

    UnityEvent<TPrefab> _onNewObjectCreated = new UnityEvent<TPrefab>();
    UnityEvent<TPrefab> _onObjectActive = new UnityEvent<TPrefab>();
    UnityEvent<TPrefab> _onObjectReleased = new UnityEvent<TPrefab>();
}
