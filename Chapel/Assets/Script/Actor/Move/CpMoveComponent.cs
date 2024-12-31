using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CpActorBase))]
public class CpMoveComponent : MonoBehaviour
{
    private void Update()
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

    public FCpMoverId RequestStart(CpMoveParamScriptableObjectBase moveParamSO)
    {
        return MoverManager.RequestStart(moveParamSO.GetMoveParam());
    }

    public void RequestStop(ECpMoveStopReason reason)
    {
        MoverManager.RequestStop(reason);
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

    CpMoverManager _moverManager;
}
