using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using Sirenix;
using Sirenix.OdinInspector;

public abstract partial class CpActorBase : MonoBehaviour,
    ICpActorForwardInterface,
    ICpTweenable,
    ICpActRunnable,
    ICpPoolable
{
    protected virtual void Awake()
    {
        _transform = transform;

        // 「アニメーションを行うスプライト」として設定されてるGameObjectのAnimatorを取得
        if (_animatedSpriteRenderer != null)
        {
            _animator = _animatedSpriteRenderer.GetComponent<Animator>();
        }
        else
        {
            _animator = GetComponent<Animator>();
        }
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
        Assert.IsTrue(false, $"{gameObject.name}はGetMoverUpdateTypeを実装していません");
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
    public virtual void OnActivated() { }
    public virtual void OnReleased() { }
    // end of ICpPoolable

    // 便利関数
    protected void AttachCurrentRoom()
    {
        CpRoomProxyManager roomProxyManager = CpRoomProxyManager.Get();
        CpRoomProxy activeRoomProxy = roomProxyManager.GetActiveRoomProxy();
        CpRoom roomInstnace = activeRoomProxy.GetRoomInstance();

        _transform.SetParent(roomInstnace.transform);
    }


    protected Transform _transform = null;
    SltTweenManager _tweenManager = null;
    CpActRunnerManager _actRunnerManager = null;
    Animator _animator = null;
    int _instanceId = -1;

#if CP_DEBUG

    [Button]
    public virtual void ValidateAsset()
    {
        StateMachine stateMachine = GetComponent<StateMachine>();
        CpDebug.Log("enabled:" + stateMachine.enabled);
    }

    public virtual void DrawImGui()
    {

    }
#endif
}
