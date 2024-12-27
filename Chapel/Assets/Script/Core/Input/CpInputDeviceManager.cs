using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;



public class CpInputDeviceManager
{
    InputAction inputActionKeyboard = new InputAction(type: InputActionType.PassThrough, binding: "<Keyboard>/AnyKey", interactions: "Press");
    InputAction inputActionMouse = new InputAction(type: InputActionType.PassThrough, binding: "<Mouse>/*", interactions: "Press");
    InputAction inputActionXInput = new InputAction(type: InputActionType.PassThrough, binding: "<XInputController>/*", interactions: "Press");
    InputAction inputActionDualShock4 = new InputAction(type: InputActionType.PassThrough, binding: "<DualShockGamepad>/*", interactions: "Press");
    InputAction inputActionSwitchProController = new InputAction(type: InputActionType.PassThrough, binding: "<SwitchProControllerHID>/*", interactions: "Press");

    TSltBitFlag<ECpInputDeviceType> _latestInputDeviceTypes;
    public CpInputDeviceManager()
    {
        inputActionKeyboard.Enable();
        inputActionMouse.Enable();
        inputActionXInput.Enable();
        inputActionDualShock4.Enable();
        inputActionSwitchProController.Enable();
    }

    public static CpInputDeviceManager Get()
    {
        return CpInputManager.Instance.GetInputDeviceManager();
    }

    public void Update()
    {
        // 最後に入力のあったデバイスを保存する
        TSltBitFlag<ECpInputDeviceType> currentInputDevices = FindTriggeredDevice();
        if (currentInputDevices.Any())
        {
            _latestInputDeviceTypes = currentInputDevices;
        }
    }

    public ECpControlScheme GetPrioritizeControlScheme()
    {
        ECpInputDeviceType prioritizeDevice = GetLatestPrioritizeInputDevice();
        ECpControlScheme prioritizeScheme = CpInputUtil.ToControlScheme(prioritizeDevice);
        return prioritizeScheme;
    }

    ECpInputDeviceType GetLatestPrioritizeInputDevice()
    {
        return FindMostPrioritizeInputDevice(_latestInputDeviceTypes);
    }

    ECpInputDeviceType FindMostPrioritizeInputDevice()
    {
        TSltBitFlag<ECpInputDeviceType> triggeredDevices = FindTriggeredDevice();
        return FindMostPrioritizeInputDevice(triggeredDevices);
    }
    ECpInputDeviceType FindMostPrioritizeInputDevice(in TSltBitFlag<ECpInputDeviceType> deviceFlag)
    {
        int maxPriority = -1;
        ECpInputDeviceType prioritizeDevice = ECpInputDeviceType.None;
        foreach (ECpInputDeviceType device in Enum.GetValues(typeof(ECpInputDeviceType)))
        {
            if (!deviceFlag.Get(device))
            {
                continue;
            }

            int devicePrioirty = GetInputDevicePriority(device);
            if (devicePrioirty > maxPriority)
            {
                maxPriority = devicePrioirty;
                prioritizeDevice = device;
            }
        }

        return prioritizeDevice;
    }

    TSltBitFlag<ECpInputDeviceType> FindTriggeredDevice()
    {
        TSltBitFlag<ECpInputDeviceType> retFlag = default;

        if (inputActionKeyboard.triggered || inputActionMouse.triggered)
        {
            retFlag.Set(ECpInputDeviceType.KeyboardAndMouse, true);
        }
        if (inputActionXInput.triggered)
        {
            retFlag.Set(ECpInputDeviceType.XBox, true);
        }
        if (inputActionDualShock4.triggered)
        {
            retFlag.Set(ECpInputDeviceType.DualShock4, true);
        }
        if (inputActionSwitchProController.triggered)
        {
            retFlag.Set(ECpInputDeviceType.Switch, true);
        }

        return retFlag;
    }

    int GetInputDevicePriority(ECpInputDeviceType deviceType)
    {
        return deviceType switch
        {
            ECpInputDeviceType.KeyboardAndMouse => 0,
            ECpInputDeviceType.XBox => 1,
            ECpInputDeviceType.DualShock4 => 2,
            ECpInputDeviceType.Switch => 3,
            _ => -1,
        };
    }
}
