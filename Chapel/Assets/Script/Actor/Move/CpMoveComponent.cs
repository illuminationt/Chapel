using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
public enum ECpMoverUpdateType
{
    None = 0,
    UpdateFunction,
    FixedUpdateFunction,
    Manually,
}

public class CpMoveComponent : MonoBehaviour
{
    private void Start()
    {
        _ownerActor = GetComponent<CpActorBase>();

        _rigidbody2D = _ownerActor.GetComponent<Rigidbody2D>();
        _transform = _ownerActor.transform;

        // バリデーション
        Assert.IsTrue(_ownerActor != null);
        if (GetMoverUpdateType() == ECpMoverUpdateType.None)
        {
            string str = $"{_ownerActor.name}のMoveComponent._moverUpdateTypeが設定されていません";
            Assert.IsTrue(GetMoverUpdateType() != ECpMoverUpdateType.None,
                str);
        }

        _ownerActor.OnActivatedCallback.Subscribe(OnOwnerActivated);
        _ownerActor.OnReleasedCallback.Subscribe(OnOwnerReleased);

        CpMoveComponentHolder holder = CpMoveComponentHolder.Get();
        holder.Register(this);
    }

    private void OnDestroy()
    {
        CpMoveComponentHolder holder = CpMoveComponentHolder.Get();
        holder.Unregister(this);
    }

    public ECpMoverUpdateType GetMoverUpdateType()
    {
        return _ownerActor.GetMoverUpdateType();
    }

    public void Execute()
    {
        UpdateManually();
    }
    void OnOwnerActivated(Unit _)
    {
        enabled = true;
    }
    void OnOwnerReleased(Unit _)
    {
        enabled = false;
    }

    void UpdateManually()
    {
        float currentYaw = _transform.eulerAngles.z;
        ECpMoverUpdateResult updateResult = MoverManager.Update(currentYaw);

        if (updateResult == ECpMoverUpdateResult.Moving)
        {
            Vector2 deltaMove = _moverManager.GetDeltaMove();
            if (_rigidbody2D != null)
            {
                Vector2 velocity = deltaMove / CpTime.SmoothDeltaTime;
                _rigidbody2D.linearVelocity = velocity;
            }
            else
            {
                SltUtil.AddToPosition(_transform, deltaMove);
            }

            float deltaYaw = _moverManager.GetDeltaRotZ();
            if (deltaYaw != 0f)
            {
                _transform.AddToRotationZ(deltaYaw);
            }
        }

        MoverManager.OnMoverValueApplied();
    }

    public FCpMoverId RequestStart(CpMoveParamBase moveParam)
    {
        return MoverManager.RequestStart(moveParam);
    }

    public FCpMoverId RequestStart(ICpMoveParam moveParamInterface)
    {
        return MoverManager.RequestStart(moveParamInterface);
    }
    public FCpMoverId RequestStart(in FCpMoveParamLinear moveParamLinear)
    {
        return MoverManager.RequestStart(moveParamLinear);
    }
    public FCpMoverId RequestStart(in FCpMoveParamCurve moveParamCurve)
    {
        return MoverManager.RequestStart(moveParamCurve);
    }
    public FCpMoverId RequestStart(in FCpMoveParamTween paramTween)
    {
        return MoverManager.RequestStart(paramTween);
    }
    public FCpMoverId RequestStart(in FCpMoveParamEnemyShot paramEnemyShot)
    {
        return MoverManager.RequestStart(paramEnemyShot);
    }

    public FCpMoverId RequestStart(in FCpMoveParamHomingCloseToTarget param)
    {
        return MoverManager.RequestStart(param);
    }
    public FCpMoverId RequestStart(in FCpMoveParamHomingOnlyRotate param)
    {
        return MoverManager.RequestStart(param);
    }
    public FCpMoverId RequestStart(in FCpMoveParamHomingForceClose param)
    {
        return MoverManager.RequestStart(param);
    }

    public FCpMoverId RequestStart(in FCpMoveParamPhysical param)
    {
        return MoverManager.RequestStart(param);
    }

    public FCpMoverId RequestStart(in FCpMoveParamRotate param)
    {
        return MoverManager.RequestStart(param);
    }

    public FCpMoverId RequestStart(in FCpMoveParamRotateInfFixedDirection param)
    {
        return MoverManager.RequestStart(param);
    }
    public FCpMoverId RequestStart(in FCpMoveParamRotateInfToTarget param)
    {
        return MoverManager.RequestStart(param);
    }

    public FCpMoverId RequestStart(in FCpMoveParamTrampoline param)
    {
        return MoverManager.RequestStart(param);
    }

    public FCpMoverId RequestStart(CpMoveParamScriptableObjectBase moveParamSO)
    {
        return MoverManager.RequestStart(moveParamSO.GetMoveParam());
    }

    public void RequestStopAll()
    {
        MoverManager.RequestStopAll();
    }

    public void RequestStop(in FCpMoverId id, ECpMoveStopReason reason)
    {
        MoverManager.RequestStop(id, reason);
    }

    public Vector2 GetVelocity() { return _moverManager.GetVelocity(); }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        MoverManager.OnCollisionEnter2D(collision);
    }

    public void Reset()
    {
        OnMoveFinished.RemoveAllListeners();
        _moverManager.Reset();
    }

    CpMoverManager MoverManager
    {
        get
        {
            if (_moverManager == null)
            {
                _moverManager = new CpMoverManager(OwnerActor);
            }
            return _moverManager;
        }
    }
    CpActorBase OwnerActor
    {
        get
        {
            if (_ownerActor == null)
            {
                _ownerActor = GetComponent<CpActorBase>();
            }
            return _ownerActor;
        }
    }

    public UnityEvent<FCpMoverId> OnMoveFinished
    {
        get
        {
            return MoverManager.OnMoveFinished;
        }
    }

    CpActorBase _ownerActor = null;
    CpMoverManager _moverManager;
    Rigidbody2D _rigidbody2D = null;
    Transform _transform = null;

#if CP_DEBUG
    public void DrawImGui()
    {
        _moverManager.DrawImGui();
    }

    public bool DebugExistsMover()
    {
        return _moverManager.DebugGetMoverCount() > 0;
    }
#endif 
}
