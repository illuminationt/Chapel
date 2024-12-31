using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public partial class CpActRunnerManager
{
    FCpActRunnerContext CreateContext()
    {
        FCpActRunnerContext newContext;
        newContext.OwnerActRunnerManager = this;
        newContext.OwnerActor = _ownerActor;
        return newContext;
    }
    public FCpActRunnerId RequestStart(in FCpActParamScale param)
    {
        CpActRunnerBase newRunner = CpActRunnerScale.Create(param, CreateContext());
        _actRunners.Add(newRunner);
        return newRunner.GetId();
    }

    public FCpActRunnerId RequestStart(ICpActionParam iparam)
    {
        ECpActionParamType actionParamType = iparam.GetActionParamType();
        switch (actionParamType)
        {
            case ECpActionParamType.None: return FCpActRunnerId.INVALID_ID;
            case ECpActionParamType.Scale: return RequestStart((FCpActParamScale)iparam);

            default:
                Assert.IsTrue(false, "存在しないアクションが実行された");
                return FCpActRunnerId.INVALID_ID;
        }
    }

}
