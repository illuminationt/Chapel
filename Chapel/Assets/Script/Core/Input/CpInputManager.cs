using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

        _shootButtonInput = FCpButtonInput.Get(inputActions.Player.Shoot);
        _decideButtonInput = FCpButtonInput.Get(inputActions.Player.Decide);
        _pauseButtonInput = FCpButtonInput.Get(inputActions.Player.Pause);
    }

    public void  Update(PlayerInput playerInput)
    {
        InputActionMap inputActionMapPlayer = playerInput.actions.FindActionMap("Player");
        _move = inputActionMapPlayer["Move"].ReadValue<Vector2>();
        _direction = inputActionMapPlayer["Direction"].ReadValue<Vector2>();
        _shootButtonInput = FCpButtonInput.Get(inputActionMapPlayer["Shoot"]);
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
            case ECpButton.Shoot:return _shootButtonInput;
            case ECpButton.Decide:return _decideButtonInput;
            case ECpButton.Pause:return _pauseButtonInput;

            case ECpButton.UiDecide:return _uiDecideInput;
            default:
                CpDebug.LogError("存在しないボタンを使おうとしました");
                FCpButtonInput dummy = new FCpButtonInput();
                return dummy;
        }
    }

    // Player関連
    Vector2 _move;
    Vector2 _direction;
    FCpButtonInput _shootButtonInput;
    FCpButtonInput _decideButtonInput;
    FCpButtonInput _pauseButtonInput;

    // UI関連
    Vector2 _uiMove;
    FCpButtonInput _uiDecideInput;
}

public class CpInputManager : SingletonMonoBehaviour<CpInputManager>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        GameObject Obj = new GameObject("CtdControllerManager", typeof(CpInputManager));
        DontDestroyOnLoad(Obj);
        Obj.SetActive(true);

        CpInputManager CtrlManager = Obj.GetComponent<CpInputManager>();
        CtrlManager.enabled = true;
    }

    protected override void Awake()
    {
        _input = gameObject.AddComponent<PlayerInput>();

        var op = Addressables.LoadAssetAsync<InputActionAsset>("CpInputActions");
        InputActionAsset inputActionAsset = op.WaitForCompletion();
        _input.actions = inputActionAsset;

        SwitchInputAction(ECpInputActionType.Player);
    }
    void Update()
    {
        UpdateAllInputs();
    }

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
    public bool WasPressed(ECpButton button) => _pendingInput.WasPressed(button);
    public bool IsPressHold(ECpButton button) => _pendingInput.IsPressHold(button);
    public bool WasReleased(ECpButton button) => _pendingInput.WasReleased(button);

    void UpdateAllInputs()
    {
        //_pendingInput.Update(_cpInputActions);
        _pendingInput.Update(_input);
    }

    PlayerInput _input;
    FCpPendingInput _pendingInput;
}