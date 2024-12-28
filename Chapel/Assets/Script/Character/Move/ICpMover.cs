using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpMover
{
    public abstract CpMoveComponent GetMoveComponent();

    public void StartMove(CpMoveParamBase moveParam)
    {
        GetMoveComponent().RequestStart(moveParam);
    }

    public void StartMove(in FCpMoveParamLinear moveParamLinear)
    {
        GetMoveComponent().RequestStart(moveParamLinear);
    }
}