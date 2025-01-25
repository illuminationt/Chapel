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
        _forwardTypeOnRightStick = ECpPlayerForwardType.Direction;
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
        // throw new System.NotImplementedException();
        _aimMarkerController.SetActive(false);
    }

    void ExecuteFocalPoint()
    {
        CpInputManager inputManager = CpInputManager.Get();
        ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();

        _aimMarkerController.SetActive(true);
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

    Transform _playerTransfomrm;
    ECpPlayerForwardType _forwardTypeOnRightStick;
    CpPlayerAimMarkerController _aimMarkerController = null;
    Vector2 latestForwardVector = Vector2.right;


#if CP_DEBUG
    public void DrawImGui()
    {
        SltImGui.EnumValueCombo(ref _forwardTypeOnRightStick);

        if (SltImGui.TreeNode("AimMarkerController"))
        {
            _aimMarkerController.DrawImGui();
            SltImGui.TreePop();
        }
    }
#endif
}
