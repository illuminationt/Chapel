using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpFlowRoomParam
{
    public CpPlayer Player;
    public CpRoom PrevRoom;
    public CpRoom NewRoom;
    CpRoomTransitionParam RoomTransitionParam;
}

public class CpFlowRoomTransition : CpGameFlowElementBase
{
    public override ECpGameFlowType GetGameFlowType() { return ECpGameFlowType.RoomTransition; }
    public void Setup(in FCpFlowRoomParam param)
    {
        _param = param;
    }



    FCpFlowRoomParam _param;

}
