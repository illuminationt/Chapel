using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CpEnemyShotInitializeParam
{
    public FCpMoveParamEnemyShot enemyShotMoveParam;
}


public class CpEnemyShot : CpShotBase
{
    public void Initialize(CpEnemyShotInitializeParam initializeParam)
    {
        StartMove(initializeParam.enemyShotMoveParam);
    }

    void StartMove(in FCpMoveParamEnemyShot moveParam)
    {
        CpMoveComponent moveComp = GetComponent<CpMoveComponent>();
        moveComp.RequestStart(moveParam);
    }
}
