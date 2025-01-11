using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// 自機の正面方向決定タイプ
public enum ECpPlayerForwardType
{
    None = -1,
    Direction = 0,// 方向をスティックで決める
    FocalPoint = 1,// 向く方向を座標で決める
}

public class CpPlayerForwardCalculator
{
    public CpPlayerForwardCalculator(Transform inPlayerTransform)
    {
        _playerTransfomrm = inPlayerTransform;
        _aimMarkerController = new CpPlayerAimMarkerController();
        _forwardTypeOnRightStick = ECpPlayerForwardType.FocalPoint;
    }

    public void Update()
    {
        ECpPlayerForwardType currentForwardType = CalcCurrentForwardType();
        switch (currentForwardType)
        {
            case ECpPlayerForwardType.None:
                break;

            case ECpPlayerForwardType.Direction:
                ExecuteDirection();
                break;

            case ECpPlayerForwardType.FocalPoint:
                ExecuteFocalPoint();
                break;

            default:
                throw new System.NotImplementedException();
        }
    }

    void ExecuteDirection()
    {
        throw new System.NotImplementedException();

    }

    void ExecuteFocalPoint()
    {
        CpInputManager inputManager = CpInputManager.Get();
        ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();

        _aimMarkerController.Update(directionInputDevice);
    }


    public Vector2 GetForwardVector()
    {
        Vector2 selfPosition = _playerTransfomrm.position;
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
        ECpPlayerForwardType currentForwardType = CalcCurrentForwardType();
        switch (currentForwardType)
        {
            case ECpPlayerForwardType.None:
                return Vector2.zero;

            case ECpPlayerForwardType.Direction:
                var input = CpInputManager.Get();
                Vector2 currentDirection = input.GetDirectionInput();

                const float directionInputDeadZone = 0.8f;
                float currentDirInputSize = currentDirection.magnitude;
                if (currentDirInputSize > directionInputDeadZone)
                {
                    latestForwardVector = currentDirection;
                }

                Vector2 retFocalLocation = SltMath.AddVec(_playerTransfomrm.position, latestForwardVector * 100f);
                return retFocalLocation;

            case ECpPlayerForwardType.FocalPoint:
                Vector2 aimMarkerPosition = _aimMarkerController.GetAimMarkerWorldPosition();
                return aimMarkerPosition;

            default:
                throw new System.NotImplementedException();

        }

        //CpInputManager inputManager = CpInputManager.Get();
        //ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();
        //switch (directionInputDevice)
        //{
        //    case ECpDirectionInputDevice.RightStick:
        //        return CalcFocalLocation_RightStick();
        //    case ECpDirectionInputDevice.Mouse:
        //        return CalcFocalLocation_Mouse();
        //    case ECpDirectionInputDevice.None:
        //        // まだ操作入力を受けていないならデフォルト方向を向くような座標を返す
        //        {
        //            Vector2 defaultFocal = SltMath.AddVec(playerTransfomrm.position, Vector2.right * 100f);
        //            return defaultFocal;
        //        }
        //    default:
        //        Assert.IsTrue(false);
        //        return Vector2.zero;
        //}
    }

    // 現在使用すべき正面方向決定方法を算出
    ECpPlayerForwardType CalcCurrentForwardType()
    {
        CpInputManager inputManager = CpInputManager.Get();
        ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();
        switch (directionInputDevice)
        {
            case ECpDirectionInputDevice.None:
                return ECpPlayerForwardType.None;

            // マウス操作中は焦点座標決定方式を使う
            case ECpDirectionInputDevice.Mouse:
                return ECpPlayerForwardType.FocalPoint;

            // パッドを使用中は、「パッド使用時の正面方向決定方法」を使う
            case ECpDirectionInputDevice.RightStick:
                return _forwardTypeOnRightStick;

            default:
                throw new System.NotImplementedException();
        }
    }

    //Vector2 CalcFocalLocation_RightStick()
    //{
    //    var input = CpInputManager.Get();
    //    Vector2 currentDirection = input.GetDirectionInput();

    //    const float directionInputDeadZone = 0.8f;
    //    float currentDirInputSize = currentDirection.magnitude;
    //    if (currentDirInputSize > directionInputDeadZone)
    //    {
    //        latestForwardVector = currentDirection;
    //    }

    //    Vector2 retFocalLocation = SltMath.AddVec(playerTransfomrm.position, latestForwardVector * 100f);
    //    return retFocalLocation;
    //}

    //Vector2 CalcFocalLocation_Mouse()
    //{
    //    var input = CpInputManager.Get();

    //    Vector2 mouseScreenPosition = input.GetMouseLocation();
    //    Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    //    return mouseWorldPosition;
    //}

    Transform _playerTransfomrm;
    ECpPlayerForwardType _forwardTypeOnRightStick;
    CpPlayerAimMarkerController _aimMarkerController = null;
    Vector2 latestForwardVector = Vector2.right;
}
