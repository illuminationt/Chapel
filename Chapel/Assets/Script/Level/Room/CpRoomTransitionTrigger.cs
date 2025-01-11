using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CpRoomTransitionRequestParam
{
    public ECpRoomConnectDirectionType ConnectDirection = ECpRoomConnectDirectionType.None;
}

public class CpRoomTransitionTrigger : MonoBehaviour
{
    public ECpRoomConnectDirectionType ConnectDirection;

    public void OnTrigger()
    {
        CpDungeonManager dungeonManager = CpDungeonManager.Get();
        CpRoomTransitionRequestParam reqParam = new CpRoomTransitionRequestParam();
        reqParam.ConnectDirection = ConnectDirection;
        dungeonManager.RequestRoomTransition(reqParam);
    }
}
