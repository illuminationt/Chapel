using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CpGamePlayStateRoomExplore : CpGamePlayStateBase
{
    public CpGamePlayStateRoomExplore(CpGamePlayManager owner) : base(owner) { }
    public override ECpGamePlayState GetGamePlayState()
    {
        return ECpGamePlayState.RoomExplore;
    }

    public override List<ECpGamePlayState> GetCanStackStates()
    {
        return new List<ECpGamePlayState> { ECpGamePlayState.Root };
    }
}
