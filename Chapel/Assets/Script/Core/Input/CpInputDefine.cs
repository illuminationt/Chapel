using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class FCpInputDefine
{
    public static float sqrDeadZone => DeadZone * DeadZone;
    public static readonly float DeadZone = 0.5f;
}

// ゲーム内で使うボタン。特定のHIDに依存しない、抽象化した入力
public enum ECpButton
{
    Shoot,
    Decide,
    Pause,

    UiDecide,
}

public enum ECpInputActionType
{
    Player,
    UI,
}
public enum ECpControlScheme
{
    Gamepad,
    KeyboardAndMouse,

    None,
}

[Flags]
public enum ECpInputDeviceType
{
    None,
    KeyboardAndMouse,
    XBox,
    DualShock4,
    Switch,

    Max,
}

public enum ECpDirectionInputDevice
{
    None,
    Mouse,
    RightStick,
}

public static class CpInputUtil
{
    public static ECpControlScheme ToControlScheme(ECpInputDeviceType device)
    {
        switch (device)
        {
            case ECpInputDeviceType.KeyboardAndMouse:
                return ECpControlScheme.KeyboardAndMouse;
            case ECpInputDeviceType.XBox:
            case ECpInputDeviceType.DualShock4:
            case ECpInputDeviceType.Switch:
                return ECpControlScheme.Gamepad;
            default:
                return ECpControlScheme.None;
        }
    }
}