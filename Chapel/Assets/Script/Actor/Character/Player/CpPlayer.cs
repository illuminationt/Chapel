using Oddworm.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpPilotComponent))]
[RequireComponent(typeof(CpShootComponent))]
public class CpPlayer : CpActorBase
{
    Transform _transform = null;
    CpPilotComponent _pilotComponent = null;
    CpShootComponent _shootComponent = null;
    CpDebugComponent _debugComponent = null;
    CpPlayerForwardCalculator _forwardCalculator = null;

    public int TestValue = 123;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _pilotComponent = GetComponent<CpPilotComponent>();
        _shootComponent = GetComponent<CpShootComponent>();
        _forwardCalculator = new CpPlayerForwardCalculator(_transform);
    }

    private void Update()
    {
        _pilotComponent.execute();

        _forwardCalculator.execute();
        debugDrawDirection();

        updateShoot();

    }

    // ICpActorForwardInterface
    public override float GetForwardDegree()
    {
        return _forwardCalculator.GetForwardDegree();
    }

    void updatePilot()
    {
        _pilotComponent.execute();
    }
    void updateShoot()
    {
        FCpShootControlParam shootControlParam = new FCpShootControlParam();
        shootControlParam.origin = _transform.position;
        shootControlParam.forward = _forwardCalculator.GetForwardVector();
        _shootComponent.execute(shootControlParam);
    }

    void debugDrawDirection()
    {
        Vector2 dir = _forwardCalculator.GetForwardVector();
        Vector2 start = _transform.position;
        Vector2 end = start + dir * 22f;
        // SltDebugDraw.DrawArrow(start, end, 3f, 0f, Color.green);
    }
}
