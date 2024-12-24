using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FCpInputDefine
{
    public static float sqrDeadZone => DeadZone * DeadZone;
    public static readonly float DeadZone = 0.5f;
}