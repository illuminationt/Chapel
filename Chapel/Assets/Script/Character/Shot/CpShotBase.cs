using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CpMoveComponent))]
public class CpShotBase : MonoBehaviour, ICpMover
{
    public CpMoveComponent _moveComponent = null;

    // start ICpMover interface
    public CpMoveComponent GetMoveComponent() { return _moveComponent; }
}
