using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CpMoveComponent))]
public class CpShotBase : CpActorBase, ICpMover
{
    public CpMoveComponent _moveComponent = null;

    // start ICpMover interface
    public CpMoveComponent GetMoveComponent() { return _moveComponent; }

    // end of ICpMover

    // start ICpForwardInterface
    public override float GetForwardDegree()
    {
        Assert.IsTrue(false);
        return 0f;
    }
    // end of ICpForwardInterface
}
