using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CpPlayerForwardCalculator
{
    public CpPlayerForwardCalculator(Transform inPlayerTransform)
    {
        playerTransfomrm = inPlayerTransform;
    }

    public void execute()
    {
        Vector2 start = playerTransfomrm.position;
        Vector2 end = start + GetForwardVector() * 5f;

        SltDebugDraw.DrawArrow(start, end, Color.green, 0.1f, 0f);
    }

    public Vector2 GetForwardVector()
    {
        Vector2 selfPosition = playerTransfomrm.position;
        Vector2 focalPosition = CalcFocalLocation();
        Vector2 retForward = (focalPosition - selfPosition).normalized;
        return retForward;
    }
    public float GetForwardDegree()
    {
        return SltMath.ToDegree(GetForwardVector());
    }

    // 狙う的となる焦点座標を算出
    Vector2 CalcFocalLocation()
    {
        CpInputManager inputManager = CpInputManager.Get();
        ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();
        switch (directionInputDevice)
        {
            case ECpDirectionInputDevice.RightStick:
                return CalcFocalLocation_RightStick();
            case ECpDirectionInputDevice.Mouse:
                return CalcFocalLocation_Mouse();
            case ECpDirectionInputDevice.None:
                // まだ操作入力を受けていないならデフォルト方向を向くような座標を返す
                {
                    Vector2 defaultFocal = SltMath.AddVec(playerTransfomrm.position, Vector2.right * 100f);
                    return defaultFocal;
                }
            default:
                Assert.IsTrue(false);
                return Vector2.zero;
        }
    }

    Vector2 CalcFocalLocation_RightStick()
    {
        var input = CpInputManager.Get();
        Vector2 currentDirection = input.GetDirectionInput();

        const float directionInputDeadZone = 0.8f;
        float currentDirInputSize = currentDirection.magnitude;
        if (currentDirInputSize > directionInputDeadZone)
        {
            latestForwardVector = currentDirection;
        }

        Vector2 retFocalLocation = SltMath.AddVec(playerTransfomrm.position, latestForwardVector * 100f);
        return retFocalLocation;
    }

    Vector2 CalcFocalLocation_Mouse()
    {
        var input = CpInputManager.Get();

        Vector2 mouseScreenPosition = input.GetMouseLocation();
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }

    Transform playerTransfomrm;
    Vector2 latestForwardVector = Vector2.right;
}
