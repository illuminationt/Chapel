using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CpMoverCreator
{
    public static CpMoverLinear Create(in FCpMoveParamLinear param, in FCpMoverContext context)
    {
        return CpMoverLinear.Create(param, context);
    }
}
