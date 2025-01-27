using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.InputSystem.Utilities;

public struct FCpButtonInput
{
    public static FCpButtonInput Get(InputAction inputAction)
    {
        FCpButtonInput retInput;
        retInput.bWasPressed = inputAction.WasPressedThisFrame();
        retInput.bPressedHold = inputAction.IsPressed();
        retInput.bWasReleased = inputAction.WasReleasedThisFrame();
        return retInput;
    }

    public readonly bool WasPressed() => bWasPressed;
    public bool IsPressHold() => bPressedHold;
    public bool WasReleased() => bWasReleased;
    public void Reset()
    {
        bWasPressed = false;
        bPressedHold = false;
        bWasReleased = false;
    }
    public bool ExistsInput() { return bWasPressed | bPressedHold | bWasPressed; }


    bool bWasPressed;
    bool bPressedHold;
    bool bWasReleased;
}


public struct FCpPendingInput
{
    public void Update(CpInputActions inputActions)
    {
        _move = inputActions.Player.Move.ReadValue<Vector2>();
        _direction = inputActions.Player.Direction.ReadValue<Vector2>();
        _mouseLocation = inputActions.Player.MouseLocation.ReadValue<Vector2>();

        _shootButtonInput = FCpButtonInput.Get(inputActions.Player.Shoot);
        _lockonButtonInput = FCpButtonInput.Get(inputActions.Player.Lockon);
        _decideButtonInput = FCpButtonInput.Get(inputActions.Player.Decide);
        _pauseButtonInput = FCpButtonInput.Get(inputActions.Player.Pause);
    }

    public void Update(PlayerInput playerInput)
    {
        InputActionMap inputActionMapPlayer = playerInput.actions.FindActionMap("Player");
        _move = inputActionMapPlayer["Move"].ReadValue<Vector2>();
        _direction = inputActionMapPlayer["Direction"].ReadValue<Vector2>();
        _mouseLocation = inputActionMapPlayer["MouseLocation"].ReadValue<Vector2>();
        _shootButtonInput = FCpButtonInput.Get(inputActionMapPlayer["Shoot"]);
        _lockonButtonInput = FCpButtonInput.Get(inputActionMapPlayer["Lockon"]);
        _decideButtonInput = FCpButtonInput.Get(inputActionMapPlayer["Decide"]);
        _pauseButtonInput = FCpButtonInput.Get(inputActionMapPlayer["Pause"]);

        // UI
        InputActionMap inputActionMapUI = playerInput.actions.FindActionMap("UI");
        _uiMove = inputActionMapUI["Move"].ReadValue<Vector2>();
        _uiDecideInput = FCpButtonInput.Get(inputActionMapUI["Decide"]);
    }
    public bool ExistsInput()
    {
        if (_move.sqrMagnitude > FCpInputDefine.sqrDeadZone)
        {
            return true;
        }

        if (_direction.sqrMagnitude > FCpInputDefine.sqrDeadZone)
        {
            return true;
        }


        bool bButtonInputExists = false;
        bButtonInputExists |= _shootButtonInput.ExistsInput();
        bButtonInputExists |= _lockonButtonInput.ExistsInput();
        bButtonInputExists |= _decideButtonInput.ExistsInput();
        bButtonInputExists |= _pauseButtonInput.ExistsInput();
        return bButtonInputExists;
    }


    public void Reset()
    {
        _move = Vector2.zero;
        _direction = Vector2.zero;
    }

    public Vector2 getMove() => _move;
    public Vector2 getDirection() => _direction;
    public Vector2 getMouseLocation() => _mouseLocation;
    public Vector2 getUiMove() => _uiMove;
    public bool WasPressed(ECpButton button)
    {
        FCpButtonInput input = FindButtonInput(button);
        return input.WasPressed();
    }

    public bool IsPressHold(ECpButton button)
    {
        FCpButtonInput input = FindButtonInput(button);
        return input.IsPressHold();
    }

    public bool WasReleased(ECpButton button)
    {
        FCpButtonInput input = FindButtonInput(button);
        return input.WasReleased();
    }

    FCpButtonInput FindButtonInput(ECpButton button)
    {
        switch (button)
        {
            case ECpButton.Shoot: return _shootButtonInput;
            case ECpButton.Lockon: return _lockonButtonInput;
            case ECpButton.Decide: return _decideButtonInput;
            case ECpButton.Pause: return _pauseButtonInput;

            case ECpButton.UiDecide: return _uiDecideInput;
            default:
                CpDebug.LogError("存在しないボタンを使おうとしました");
                FCpButtonInput dummy = new FCpButtonInput();
                return dummy;
        }
    }

    // Player関連
    Vector2 _move;
    Vector2 _direction;
    Vector2 _mouseLocation;
    FCpButtonInput _shootButtonInput;
    FCpButtonInput _lockonButtonInput;
    FCpButtonInput _decideButtonInput;
    FCpButtonInput _pauseButtonInput;

    // UI関連
    Vector2 _uiMove;
    FCpButtonInput _uiDecideInput;
}

public class CpInputManager : MonoBehaviour
{
    public static CpInputManager Create()
    {
        GameObject Obj = new GameObject("CtdControllerManager", typeof(CpInputManager));
        DontDestroyOnLoad(Obj);
        Obj.SetActive(true);

        CpInputManager inputManager = Obj.GetComponent<CpInputManager>();
        inputManager.enabled = true;

        inputManager.Initialize();

        return inputManager;
    }

    void Initialize()
    {
        _input = gameObject.AddComponent<PlayerInput>();

        var op = Addressables.LoadAssetAsync<InputActionAsset>("CpInputActions");
        InputActionAsset inputActionAsset = op.WaitForCompletion();
        _input.actions = inputActionAsset;
        _input.neverAutoSwitchControlSchemes = false;
        SwitchInputAction(ECpInputActionType.Player);

        _deviceManager = new CpInputDeviceManager();
        _latestDirectionInputDevice = ECpDirectionInputDevice.None;
    }

    void Update()
    {
        UpdateAllInputs();
        UpdateDirectionInputDevice();
        _deviceManager.Update();
    }


    public static CpInputManager Get() => CpGameManager.Instance.InputManager;
    public CpInputDeviceManager GetInputDeviceManager() => _deviceManager;

    public void SwitchInputAction(ECpInputActionType newActionType)
    {
        string actionStr = newActionType switch
        {
            ECpInputActionType.Player => "Player",
            ECpInputActionType.UI => "UI",
            _ => CpDebug.LogErrorRetStr("ERROR")
        };

        _input.SwitchCurrentActionMap(actionStr);
    }

    public Vector2 GetMoveInput() { return _pendingInput.getMove(); }
    public Vector2 GetDirectionInput() => _pendingInput.getDirection();
    public Vector2 GetMouseLocation() => _pendingInput.getMouseLocation();
    public bool WasPressed(ECpButton button) => _pendingInput.WasPressed(button);
    public bool IsPressHold(ECpButton button) => _pendingInput.IsPressHold(button);
    public bool WasReleased(ECpButton button) => _pendingInput.WasReleased(button);

    public ECpControlScheme GetCurrentControlScheme()
    {
        string currentScheme = _input.currentControlScheme;
        return currentScheme switch
        {
            "KeyboardAndMouse" => ECpControlScheme.KeyboardAndMouse,
            "Gamepad" => ECpControlScheme.Gamepad,
            _ => throw new System.Exception()
        };
    }

    public ECpDirectionInputDevice GetDirectionInputDevice() => _latestDirectionInputDevice;

    void UpdateAllInputs()
    {
        //_pendingInput.Update(_cpInputActions);
        _pendingInput.Update(_input);
    }

    void UpdateDirectionInputDevice()
    {
        ECpDirectionInputDevice currentDirInputDevice = CalcDirectionInputDevice();
        if (currentDirInputDevice != ECpDirectionInputDevice.None)
        {
            _latestDirectionInputDevice = currentDirInputDevice;
        }
    }
    ECpDirectionInputDevice CalcDirectionInputDevice()
    {
        bool bPadDirectionTriggered = GetDirectionInput().magnitude > 0.8f;
        if (bPadDirectionTriggered)
        {
            return ECpDirectionInputDevice.RightStick;
        }
        bool bMouseTriggered = _deviceManager.IsMouseTriggered();
        if (bMouseTriggered)
        {
            return ECpDirectionInputDevice.Mouse;
        }

        return ECpDirectionInputDevice.None;
    }


    CpInputDeviceManager _deviceManager;
    PlayerInput _input;
    FCpPendingInput _pendingInput;
    ECpDirectionInputDevice _latestDirectionInputDevice;
}