using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CpGamePlayStateSceneTransition : CpGamePlayStateBase
{
    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.RoomTransition;
    }
    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return new List<ECpGamePlayState> {
            ECpGamePlayState.RoomExplore,};
    }

    public void Setup(ECpSceneType nextSceneType)
    {
        _nextSceneType = nextSceneType;
    }

    protected override void OnStartInternal()
    {

    }
    protected override void UpdateInternal()
    {
        FinishState();
    }

    protected override void OnFinishedInternal()
    {

    }

    ECpSceneType _nextSceneType = ECpSceneType.None;
}
