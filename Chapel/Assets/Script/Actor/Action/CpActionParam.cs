using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ECpActionParamType
{
    None = 0,

    Scale = 1,
}
public interface ICpActionParam
{
    public ECpActionParamType GetActionParamType();
}
public struct FCpActRunnerContext
{
    public void Reset()
    {
        this = new FCpActRunnerContext();
    }
    public CpActRunnerManager OwnerActRunnerManager;
    public CpActorBase OwnerActor;
}