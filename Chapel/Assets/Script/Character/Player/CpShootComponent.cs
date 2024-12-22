using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpShootControlParam
{
    public Vector2 origin;
    public Vector2 forward;
}

public class CpShootComponent : MonoBehaviour
{
    // 弾丸発射処理.
    // 外部からのパラメータはcontrolParamで受け取る.
    public void execute(in FCpShootControlParam controlParam)
    {

    }
}
