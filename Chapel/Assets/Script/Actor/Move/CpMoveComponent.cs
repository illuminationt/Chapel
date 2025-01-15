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
        if (_moverUpdateType == ECpMoverUpdateType.None)
        {
            Assert.IsTrue(_moverUpdateType != ECpMoverUpdateType.None);
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
        ECpMoverUpdateResult updateResult = MoverManager.Update();

        if (updateResult == ECpMoverUpdateResult.Moving)
        {
            Vector2 deltaMove = _moverManager.GetDeltaMove();
            SltUtil.AddToPosition(transform, deltaMove);
        }

        if (updateResult == ECpMoverUpdateResult.Finished)
        {
            OnMoveFinished.Invoke(_moverManager.GetCurrentMoverId());
        }
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

    public FCpMoverId RequestStart(in FCpMoveParamPhysical param)
    {
        return MoverManager.RequestStart(param);
    }

    public FCpMoverId RequestStart(CpMoveParamScriptableObjectBase moveParamSO)
    {
        return MoverManager.RequestStart(moveParamSO.GetMoveParam());
    }

    public void RequestStop(ECpMoveStopReason reason)
    {
        MoverManager.RequestStop(reason);
    }

    public Vector2 GetVelocity() { return _moverManager.GetVelocity(); }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        MoverManager.OnCollisionEnter2D(collision);
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
            return _moverManager.OnMoveFinished;
        }
    }

    [SerializeField]
    ECpMoverUpdateType _moverUpdateType = ECpMoverUpdateType.None;
    CpMoverManager _moverManager;
}
