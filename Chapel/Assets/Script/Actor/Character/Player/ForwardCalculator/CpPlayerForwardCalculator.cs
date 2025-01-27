using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


// ���@�̐��ʕ�������^�C�v
public enum ECpPlayerForwardType
{
    None = -1,
    Direction = 0,// �������X�e�B�b�N�Ō��߂�
    FocalPoint = 1,// �������������W�Ō��߂�
}

public class CpPlayerForwardCalculator
{
    public static CpPlayerForwardCalculator Create(Transform inPlayerTransform, ICpLockonControlInterface lockonControl)
    {
        CpPlayerForwardCalculator instance = new CpPlayerForwardCalculator();
        instance.Initialize(inPlayerTransform, lockonControl);
        return instance;
    }
    void Initialize(Transform inPlayerTransform, ICpLockonControlInterface lockonControl)
    {
        _playerTransfomrm = inPlayerTransform;
        _aimMarkerController = new CpPlayerAimMarkerController();
        _forwardTypeOnRightStick = ECpPlayerForwardType.Direction;
        _lockonControl = lockonControl;
    }

    public void Update()
    {
        ECpPlayerForwardType currentForwardType = CalcCurrentForwardType();
        switch (currentForwardType)
        {
            case ECpPlayerForwardType.None:
                break;

            case ECpPlayerForwardType.Direction:
                Update_ForwardTypeDirection();
                break;

            case ECpPlayerForwardType.FocalPoint:
                Update_ForwardTypeFocalPoint();
                break;

            default:
                throw new System.NotImplementedException();
        }
    }

    void Update_ForwardTypeDirection()
    {
        _aimMarkerController.SetActive(false);
    }

    void Update_ForwardTypeFocalPoint()
    {
        Vector2 lockonDirection = Vector2.zero;
        if (_lockonControl.GetLockonDirection(ref lockonDirection))
        {
            _aimMarkerController.SetActive(false);
        }
        else
        {
            CpInputManager inputManager = CpInputManager.Get();
            ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();

            _aimMarkerController.SetActive(true);
            _aimMarkerController.Update(directionInputDevice);
        }
    }

    public Vector2 GetForwardVector()
    {
        Vector2 forward = Vector2.zero;
        if (_lockonControl.GetLockonDirection(ref forward))
        {
            return forward;
        }
        else
        {
            Vector2 selfPosition = _playerTransfomrm.position;
            Vector2 focalPosition = CalcFocalLocation();
            Vector2 retForward = (focalPosition - selfPosition).normalized;
            return retForward;
        }
    }
    public float GetForwardDegree()
    {
        return SltMath.ToDegree(GetForwardVector());
    }

    // �_���I�ƂȂ�œ_���W���Z�o
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

    // ���ݎg�p���ׂ����ʕ���������@���Z�o
    ECpPlayerForwardType CalcCurrentForwardType()
    {
        CpInputManager inputManager = CpInputManager.Get();
        ECpDirectionInputDevice directionInputDevice = inputManager.GetDirectionInputDevice();
        switch (directionInputDevice)
        {
            case ECpDirectionInputDevice.None:
                return ECpPlayerForwardType.None;

            // �}�E�X���쒆�͏œ_���W����������g��
            case ECpDirectionInputDevice.Mouse:
                return ECpPlayerForwardType.FocalPoint;

            // �p�b�h���g�p���́A�u�p�b�h�g�p���̐��ʕ���������@�v���g��
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

    ICpLockonControlInterface _lockonControl = null;

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
