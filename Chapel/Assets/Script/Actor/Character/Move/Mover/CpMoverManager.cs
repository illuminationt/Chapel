using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CpMoverManager
{
    public CpMoverManager(CpActorBase actor)
    {
        _ownerActor = actor;
    }

    public bool IsActive()
    {
        if (_currentMover == null) { return false; }

        if (_currentMover.IsFinished())
        {
            return false;
        }
        return true;
    }

    public void RequestStart(CpMoveParamBase moveParam)
    {
        _currentMover = CreateMover(moveParam);
    }


    FCpMoverContext CreateContext()
    {
        FCpMoverContext context;
        context.OwnerActor = _ownerActor;
        context.OwnerPosition = _ownerActor.transform.position;
        context.Velocity = _currentVelocity;

        ICpActorForwardInterface forwardInterface = _ownerActor;
        context.OwnerDegree = forwardInterface.GetForwardDegree();
        return context;
    }

    public void Update()
    {
        if (_currentMover != null)
        {
            bool bFinished = _currentMover.IsFinished();
            if (!bFinished)
            {
                _currentMover.Update();
                _currentVelocity = _currentMover.GetVelocity();
            }
        }
    }

    public Vector2 GetDeltaMove()
    {
        if (_currentMover == null)
        {
            return Vector2.zero;
        }
        return _currentMover.GetDeltaMove();
    }

    Vector2 _currentVelocity = Vector2.zero;
    CpMoverBase _currentMover = null;
    CpActorBase _ownerActor = null;
}
