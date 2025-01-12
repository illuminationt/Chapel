using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpActorBase : MonoBehaviour,
    ICpActorForwardInterface,
    ICpTweenable,
    ICpActRunnable
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
        Destroy(gameObject);
    }
    public virtual float GetForwardDegree()
    {
        Assert.IsTrue(false);
        return 0f;
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

    protected Transform _transform = null;
    SltTweenManager _tweenManager = null;
    CpActRunnerManager _actRunnerManager = null;
}
