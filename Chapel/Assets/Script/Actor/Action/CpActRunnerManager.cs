using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class CpActRunnerManager
{
    public CpActRunnerManager(CpActorBase ownerActor)
    {
        _ownerActor = ownerActor;
    }

    public void Update()
    {
        for (int runnerIndex = _actRunners.Count - 1; runnerIndex >= 0; runnerIndex--)
        {
            CpActRunnerBase runner = _actRunners[runnerIndex];
            runner.Update();

            if (runner.IsFinished())
            {
                OnActionFinished.Invoke(runner.GetId());
                _actRunners.RemoveAt(runnerIndex);
            }
        }
    }

    public UnityEvent<FCpActRunnerId> OnActionFinished => _onActionFinished;

    List<CpActRunnerBase> _actRunners = new List<CpActRunnerBase>();
    CpActorBase _ownerActor = null;

    UnityEvent<FCpActRunnerId> _onActionFinished = new UnityEvent<FCpActRunnerId>();
}
