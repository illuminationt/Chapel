using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPlayerShot : CpShotBase
{
    public void OnCreated(in FCpShootControlParam controlParam)
    {
        _forwardDegreeOnCreated = SltMath.ToDegree(controlParam.forward);
    }

    // start ICpForwardInterface
    public override float GetForwardDegree()
    {
        return _forwardDegreeOnCreated;
    }
    // end of ICpForwardInterface

    float _forwardDegreeOnCreated = 0f;
}
