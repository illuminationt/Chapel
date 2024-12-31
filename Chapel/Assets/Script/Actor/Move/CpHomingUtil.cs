using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class CpHomingUtil
{
    public static Vector2 CalcHomingTargetLocation(ECpHomingTarget homingTarget)
    {
        switch (homingTarget)
        {
            case ECpHomingTarget.Player:
                return CpUtil.GetPlayerWorldPosition();
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }
}
