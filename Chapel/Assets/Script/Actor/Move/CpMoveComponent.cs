using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ECpMoverUpdateType
{
    None = 0,
    UpdateFunction,
    FixedUpdateFunction,
    Manually,
}

[RequireComponent(typeof(CpActorBase))]
public class CpMoveComponent : MonoBehaviour
{
    private void Awake()
    {
        _moverUpdateType = GetComponent<CpActorBase>().GetMoverUpdateType();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
    }
    private void Start()
    {
        if (_moverUpdateType == ECpMoverUpdateType.None)
        {
            string str = $"{gameObject}ÇÃMoveComponent._moverUpdateTypeÇ™ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ";
            Assert.IsTrue(_moverUpdateType != ECpMoverUpdateType.None,
                str);
        }
    }
    private void Update()
    {
        if (_moverUpdateType == ECpMoverUpdateType.UpdateFunction)
        {
            UpdateManually();
        }
    }

    private void FixedUpdate()
    {
        if (_moverUpdateType == ECpMoverUpdateType.FixedUpdateFunction)
        {
            UpdateManually();
        }
    }
    public void UpdateManually()
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
                SltUtil.AddToPosition(transform, deltaMove);
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
                CpActorBase ownerActor = GetComponent<CpActorBase>();
                _moverManager = new CpMoverManager(ownerActor);
            }
            return _moverManager;
        }
    }

    public UnityEvent<FCpMoverId> OnMoveFinished
    {
        get
        {
            return MoverManager.OnMoveFinished;
        }
    }

    ECpMoverUpdateType _moverUpdateType = ECpMoverUpdateType.None;
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
