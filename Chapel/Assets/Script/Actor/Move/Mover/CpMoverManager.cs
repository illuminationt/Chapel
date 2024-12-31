using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ECpMoverUpdateResult
{
    NotActive,// äàìÆíÜÇ≈ÇÕÇ»Ç¢
    Moving,//à⁄ìÆíÜ
    Finished,// ÇøÇÂÇ§Ç«èIÇÌÇ¡ÇΩÇ∆Ç±ÇÎ
}

public enum ECpMoveStopReason
{
    MoverFinished,//
}

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

    public FCpMoverId RequestStart(CpMoveParamBase moveParam)
    {
        _currentMover = CreateMover(moveParam);
        return _currentMover.GetId();
    }

    public void RequestStop(ECpMoveStopReason reason)
    {
        _bRequestedStop = true;
    }

    FCpMoverContext CreateContext()
    {
        FCpMoverContext context;
        context.OwnerMoverManager = this;
        context.OwnerActor = _ownerActor;
        context.InitialOwnerPosition = _ownerActor.transform.position;
        context.InitialVelocity = _currentVelocity;

        ICpActorForwardInterface forwardInterface = _ownerActor;
        context.InitialOwnerDegree = forwardInterface.GetForwardDegree();
        return context;
    }

    public ECpMoverUpdateResult Update()
    {
        if (_currentMover == null)
        {
            _currentVelocity = Vector2.zero;
            return ECpMoverUpdateResult.NotActive;
        }

        if (_bRequestedStop)
        {
            OnMoveFinished.Invoke(_currentMover.GetId());
            _currentMover = null;
            return ECpMoverUpdateResult.Finished;
        }

        _currentMover.Update();
        _currentVelocity = _currentMover.GetVelocity();

        bool bFinished = _currentMover.IsFinished();
        if (bFinished)
        {
            OnMoveFinished.Invoke(_currentMover.GetId());
            _currentMover = null;
            return ECpMoverUpdateResult.Finished;
        }

        return ECpMoverUpdateResult.Moving;
    }

    public Vector2 GetDeltaMove()
    {
        if (_currentMover == null)
        {
            return Vector2.zero;
        }
        return _currentMover.GetDeltaMove();
    }

    public Vector2 GetVelocity()
    {
        return _currentVelocity;
    }

    public FCpMoverId GetCurrentMoverId()
    {
        if (_currentMover != null)
        {

            return _currentMover.GetId();
        }

        return FCpMoverId.INVALID_ID;
    }
    public UnityEvent<FCpMoverId> OnMoveFinished => _onMoveFinished;

    Vector2 _currentVelocity = Vector2.zero;
    CpMoverBase _currentMover = null;
    CpActorBase _ownerActor = null;

    bool _bRequestedStop = false;
    UnityEvent<FCpMoverId> _onMoveFinished = new UnityEvent<FCpMoverId>();
}
