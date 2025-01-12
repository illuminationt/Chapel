using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * ワールド座標,スクリーン座標,正規化座標について
 * 
 * ◇ワールド座標(WorldPosition):実際にオブジェクトが配置してある空間の座標。
 * 
 * ◇スクリーン座標(ScreenPosition):左下を(0,0)とし、1pxごとに座標が1大きくなる空間。
 * 例えば、(0,0)から右に500px,上に300px進んだところのスクリーン座標は(500,300)
 * 
 * ◇正規化座標(NormalizedPosition)
 * 画面の左下を(0,0),右上を(1,1)とする座標系
 * 
 */
public static class CpUtil
{
    public static Vector2 GetWorldPositionFromNormalizedPosition(in Vector2 normalizedPosition)
    {
        Vector2 screenPoint = new Vector2(normalizedPosition.x, normalizedPosition.y);
        Camera mainCamera = Camera.main;
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPoint);
        return worldPosition;
    }

    public static Vector2 GetNormalizedPositionFromWorldPosition(in Vector2 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
        return screenPosition;
    }

    public static Vector2 GetWorldPositionFromScreenPosition(in Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector2 GetNormalizedPositionFromScreenPosition(in Vector2 screenPosition)
    {
        Vector2 worldPosition = GetWorldPositionFromScreenPosition(screenPosition);
        Vector2 normalizedPosition = GetNormalizedPositionFromWorldPosition(worldPosition);
        return normalizedPosition;
    }

    public static bool IsInScreen(in Vector2 worldPosition, float boudingRadius)
    {
        Vector2 np = GetNormalizedPositionFromWorldPosition(worldPosition);
        return 0f <= np.x && np.x <= 1f && 0 <= np.y && np.y <= 1f;
    }

    public static Vector2 CalcDeltaVector(Vector2 offsetVector, ECpOffsetType offsetType)
    {
        switch (offsetType)
        {
            case ECpOffsetType.Absolute:
                return offsetVector;
            case ECpOffsetType.Screen:
                offsetVector.x *= CpScreen.Width;
                offsetVector.y *= CpScreen.Height;
                return offsetVector;
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    public static Vector2 GetPlayerWorldPosition()
    {
        CpPlayer player = CpGameManager.Instance.Player;
        Vector2 pos = player.transform.position;
        return pos;
    }

    public static Vector2 GetDirectionToPlayer(in Vector2 origin)
    {
        Vector2 playerPos = GetPlayerWorldPosition();
        return (playerPos - origin).normalized;
    }

}
