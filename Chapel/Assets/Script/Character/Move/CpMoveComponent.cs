using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpMoveComponent : MonoBehaviour
{

    private void Update()
    {
        _moverManager.Update();

        if (_moverManager.IsActive())
        {
            Vector2 deltaMove = _moverManager.GetDeltaMove();
            SltUtil.AddToPosition(transform, deltaMove);
        }
    }

    public void RequestStart(CpMoveParamBase moveParam)
    {
        _moverManager.RequestStart(moveParam);
    }

    public void RequestStart(in FCpMoveParamLinear moveParamLinear)
    {
        _moverManager.RequestStart(moveParamLinear);
    }
    public void RequestStart(in FCpMoveParamCurve moveParamCurve)
    {
        _moverManager.RequestStart(moveParamCurve);
    }


    public void RequestStart(CpMoveParamScriptableObjectBase moveParamSO)
    {
        _moverManager.RequestStart(moveParamSO.GetMoveParam());
    }

    CpMoverManager _moverManager = new CpMoverManager();
}