using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CpPlayerShootSlotBase : ScriptableObject
{
    
    public abstract void execute(in FCpShootControlParam controlParam);
}
