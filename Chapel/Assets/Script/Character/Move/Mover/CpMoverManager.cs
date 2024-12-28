using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CpMoverManager
{
    public CpMoverManager()
    {

    }

    public bool IsActive()
    {
        return _currentMover != null;
    }

    public void RequestStart(CpMoveParamBase moveParam)
    {
        _currentMover = CreateMover(moveParam);
    }


    FCpMoverContext CreateContext()
    {
        FCpMoverContext context;
        context.Velocity = _currentVelocity;
        return context;
    }

    public void Update()
    {
        _currentMover?.Update();
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
}
