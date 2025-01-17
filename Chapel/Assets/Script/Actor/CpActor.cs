using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class CpActorBase : MonoBehaviour,
    ICpActorForwardInterface,
    ICpTweenable,
    ICpActRunnable,
    ICpPoolable
{
    protected virtual void Awake()
    {
        _transform = transform;
    }
    protected virtual void Update()
    {
        ICpActRunnable actRunnable = this;
        actRunnable.UpdateActRunnerManager();
    }

    protected virtual void Release()
    {
        throw new System.NotImplementedException();
    }
    public virtual float GetForwardDegree()
    {
        Assert.IsTrue(false);
        return 0f;
    }

    public virtual ECpMoverUpdateType GetMoverUpdateType()
    {
        Assert.IsTrue(false, $"{gameObject.name}‚ÍGetMoverUpdateType‚ðŽÀ‘•‚µ‚Ä‚¢‚Ü‚¹‚ñ");
        return ECpMoverUpdateType.None;
    }

    // ICpTweenable
    public SltTweenManager GetTweenManager()
    {

        if (_tweenManager == null)
        {
            _tweenManager = new SltTweenManager();
        }
        return _tweenManager;
    }

    // end of ICpTweenable

    // ICpActRunnable
    public CpActRunnerManager GetActRunnerManager()
    {
        return _actRunnerManager;
    }
    public CpActRunnerManager GetOrCreateActRunnerManager()
    {
        if (_actRunnerManager == null)
        {
            _actRunnerManager = new CpActRunnerManager(this);
        }
        return _actRunnerManager;
    }
    // end of ICpActRunnable

    // ICpPoolable
    public ISltPoolable Instantiate(ISltPoolable prefab)
    {
        CpActorBase actorPrefab = prefab as CpActorBase;
        return MonoBehaviour.Instantiate(actorPrefab);
    }

    public GameObject GetPooledGameObject() { return gameObject; }
    public Transform GetPooledTransform() { return _transform; }
    public int GetPoolInstanceId() { return _instanceId; }
    public void SetPoolInstanceId(int instanceid) { _instanceId = instanceid; }
    public virtual void ResetOnRelease()
    {
        _tweenManager = null;
        _actRunnerManager = null;
    }
    // end of ICpPoolable

    protected Transform _transform = null;
    SltTweenManager _tweenManager = null;
    CpActRunnerManager _actRunnerManager = null;
    int _instanceId = -1;
}
