using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICpActRunnable
{
    public CpActRunnerManager GetActRunnerManager();
    public CpActRunnerManager GetOrCreateActRunnerManager();
    public FCpActRunnerId RequestStart(in FCpActParamScale param)
    {
        return GetOrCreateActRunnerManager().RequestStart(param);
    }

    public FCpActRunnerId RequestStart(ICpActionParam iparam)
    {
        return GetOrCreateActRunnerManager().RequestStart(iparam);
    }
    public void UpdateActRunnerManager()
    {
        CpActRunnerManager manager = GetActRunnerManager();
        if (manager != null)
        {
            manager.Update();
        }
    }
}
