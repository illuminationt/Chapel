using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CpPlayerAimMarkerParam
{
    public CpPlayerAimMarker MarkerPrefab = null;
    // スティックでエイムマーカーを移動させるときの移動速度（スクリーン座標）
    public float MoveSpeedOnRightStickOnScreenSpace = 0.5f;
}
