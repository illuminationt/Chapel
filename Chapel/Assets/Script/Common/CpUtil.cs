using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class CpUtil
{
    public static Vector2 GetWorldPositionFromScreenPosition(in Vector2 screenPosition)
    {
        Vector2 screenPoint = new Vector2(screenPosition.x, screenPosition.y);
        Camera mainCamera = Camera.main;
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPoint);
        return worldPosition;
    }

    public static Vector2 GetScreenPositionFromWorldPositoni(in Vector2 worldPosition)
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(worldPosition);
        return screenPosition;
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
