using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class CpUtil
{
    public static Vector2 GetWorldPositionFromScreenPosition(in Vector2 screenPosition)
    {
        Vector2 screenPoint = new Vector2(screenPosition.x * CpScreen.Width, screenPosition.y * CpScreen.Height);
        Camera mainCamera = Camera.main;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
        return worldPosition;
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
}
