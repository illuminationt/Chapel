using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpActorBase))]
public class CpMoveComponent : MonoBehaviour
{
    private void Update()
    {
        MoverManager.Update();

        if (MoverManager.IsActive())
        {
            Vector2 deltaMove = _moverManager.GetDeltaMove();
            SltUtil.AddToPosition(transform, deltaMove);
        }
    }

    public void RequestStart(CpMoveParamBase moveParam)
    {
        MoverManager.RequestStart(moveParam);
    }

    public void RequestStart(in FCpMoveParamLinear moveParamLinear)
    {
        MoverManager.RequestStart(moveParamLinear);
    }
    public void RequestStart(in FCpMoveParamCurve moveParamCurve)
    {
        MoverManager.RequestStart(moveParamCurve);
    }
    public void RequestStart(in FCpMoveParamTween paramTween)
    {
        MoverManager.RequestStart(paramTween);
    }


    public void RequestStart(CpMoveParamScriptableObjectBase moveParamSO)
    {
        MoverManager.RequestStart(moveParamSO.GetMoveParam());
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

    CpMoverManager _moverManager;
}
