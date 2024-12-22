using Oddworm.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpPilotComponent))]
[RequireComponent(typeof(CpShootComponent))]
public class CpPlayer : MonoBehaviour
{
    Transform _transform = null;
    CpPilotComponent _pilotComponent = null;
    CpShootComponent _shootComponent = null;
    CpPlayerForwardCalculator _forwardCalculator = null;

    public int TestValue = 123;
    private void Start()
    {
        _transform = GetComponent<Transform>();
        _pilotComponent = GetComponent<CpPilotComponent>();
        _shootComponent = GetComponent<CpShootComponent>();
        _forwardCalculator = new CpPlayerForwardCalculator(_transform);

        CpDebug.Log("a");
        Debug.Log("AJFNO");
    }

    private void Update()
    {
        _pilotComponent.execute();

        _forwardCalculator.execute();
        debugDrawDirection();

        updateShoot();
    }

    void updatePilot()
    {
        _pilotComponent.execute();
    }
    void updateShoot()
    {
        FCpShootControlParam shootControlParam = new FCpShootControlParam();
        shootControlParam.origin = _transform.position;
        shootControlParam.forward = _forwardCalculator.getForwardVector();
        _shootComponent.execute(shootControlParam);
    }

    void debugDrawDirection()
    {
        Vector2 dir = _forwardCalculator.getForwardVector();
        Vector2 start = _transform.position;
        Vector2 end = start + dir * 22f;
        CpDebugUtil.DrawArrow(start, end, 0f, Color.green);
    }
}
