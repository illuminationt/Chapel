using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// ゲーム内で使うボタン。特定のHIDに依存しない、抽象化した入力
public enum ECpButton
{
    Shoot,
    Decide,
    Pause,
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
            default:
                CpDebug.LogError("存在しないボタンを使おうとしました");
                FCpButtonInput dummy = new FCpButtonInput();
                return dummy;
        }
    }

    public Vector2 _move;
    public Vector2 _direction;
    public FCpButtonInput _shootButtonInput;
    public FCpButtonInput _decideButtonInput;
    public FCpButtonInput _pauseButtonInput;
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
        _cpInputActions = new CpInputActions();
        _cpInputActions.Enable();
    }
    void Update()
    {
        UpdateAllInputs();
    }

    public Vector2 GetMoveInput() { return _pendingInput.getMove(); }
    public Vector2 GetDirectionInput() => _pendingInput.getDirection();
    public bool WasPressed(ECpButton button) => _pendingInput.WasPressed(button);
    public bool IsPressHold(ECpButton button) => _pendingInput.IsPressHold(button);
    public bool WasReleased(ECpButton button) => _pendingInput.WasReleased(button);

    void UpdateAllInputs()
    {
        _pendingInput.Update(_cpInputActions);
    }

    CpInputActions _cpInputActions;
    FCpPendingInput _pendingInput;
}

////
//public enum ECpInputType
//{
//    Player,
//    UI,
//}

//public struct FCpInputActions
//{
//    public bool ExistsInput()
//    {
//        if (move.sqrMagnitude > FCpInputDefine.sqrDeadZone)
//        {
//            return true;
//        }

//        if (direction.sqrMagnitude > FCpInputDefine.sqrDeadZone)
//        {
//            return true;
//        }

//        return bShoot | bDecide | bPause;
//    }

//    public void Reset()
//    {
//        move = Vector2.zero;
//        direction = Vector2.zero;
//        bShoot = false;
//        bDecide = false;
//        bPause = false;
//    }

//    public Vector2 getMove() => move;
//    public Vector2 getDirection() => direction;

//    public bool GetBool(ECpButton button)
//    {
//        switch (button)
//        {
//            case ECpButton.Shoot: return bShoot;
//            case ECpButton.Decide: return bDecide;
//            case ECpButton.Pause: return bPause;
//            default:
//                CpDebug.LogError("存在しないボタンの入力を取得しようとしました");
//                return false;
//        }
//    }

//    public Vector2 move;
//    public Vector2 direction;
//    public bool bShoot;
//    public bool bDecide;
//    public bool bPause;
//}


//// InputActionからの入力を管理
//public class CpInputActionMap
//{
//    public CpInputActionMap()
//    {
//        _move = new InputAction("Move", InputActionType.Value);
//        _direction = new InputAction("Direction", InputActionType.Value);
//        _shoot = new InputAction("Shoot", InputActionType.Button);
//        _decide = new InputAction("Decide", InputActionType.Button);
//        _pause = new InputAction("Pause", InputActionType.Button);
//    }

//    public void ConsumePendingInput(ref FCpInputActions Actions)
//    {
//        _pendingInputActions.move = GetVector2InputValue(_move);
//        _pendingInputActions.direction = GetVector2InputValue(_direction);
//        _pendingInputActions.bShoot = GetButtonInputValue(_shoot);
//        _pendingInputActions.bDecide = GetButtonInputValue(_decide);
//        _pendingInputActions.bPause = GetButtonInputValue(_pause);
//        // UI
//        //Vector2 StickMove = _pendingInputActions.move;
//        //_pendingInputActions.UiMoveUp = GetUiMoveInputValue(UiMoveUp, StickMove.y, true);
//        //_pendingInputActions.UiMoveDown = GetUiMoveInputValue(UiMoveDown, StickMove.y, false);
//        //_pendingInputActions.UiMoveLeft = GetUiMoveInputValue(UiMoveLeft, StickMove.x, false);
//        //_pendingInputActions.UiMoveRight = GetUiMoveInputValue(UiMoveRight, StickMove.x, true);

//        //_pendingInputActions.UiCancel = GetButtonInputValue(UiCancel);
//        //_pendingInputActions.UiOk = GetButtonInputValue(UiOk);

//        Actions = _pendingInputActions;

//        // 入力があったかチェック
//        bHasLastInput = HasLastInput();
//    }

//    Vector2 GetVector2InputValue(InputAction Action)
//    {
//        if (Action.activeControl == null)
//        {
//            return Vector2.zero;
//        }

//        Vector2 RetValue = Action.ReadValue<Vector2>();
//        //if (!CtdFuncLib.IsNearlyZero(RetValue, CtdInputDefine.DeadZone))
//        //{
//        //    SetLastInputAction(Move);
//        //}
//        return RetValue;
//    }
//    bool GetButtonInputValue(InputAction Action)
//    {
//        if (Action.activeControl == null)
//        {
//            return false;
//        }

//        bool bRetPressed = Action.IsPressed();
//        if (bRetPressed)
//        {
//            //SetLastInputAction(Action);
//        }
//        return bRetPressed;
//    }
//    public bool HasLastInput()
//    {
//        return _pendingInputActions.ExistsInput();
//    }

//    public void SetEnable(ECpInputType InputType, bool bEnable)
//    {
//        ForEachInput(InputType, (InputAction Action) =>
//        {
//            if (bEnable)
//            {
//                Action.Enable();
//            }
//            else
//            {
//                Action.Disable();
//            }
//        });
//    }
//    void ForEachInput(ECpInputType InputType, UnityAction<InputAction> Function)
//    {
//        switch (InputType)
//        {
//            case ECpInputType.Player:
//                Function(_move);
//                Function(_direction);
//                Function(_shoot);
//                Function(_decide);
//                Function(_pause);
//                break;

//            //case EpInputType.UI:
//            //    Function(UiMoveUp);
//            //    Function(UiMoveDown);
//            //    Function(UiMoveLeft);
//            //    Function(UiMoveRight);
//            //    Function(UiOk);
//            //    Function(UiCancel);
//            //    break;

//            default:
//                CpDebug.LogError();
//                break;
//        }
//    }

//    public void Dispose()
//    {
//        _move.Dispose();
//        _direction.Dispose();
//        _shoot.Dispose();
//        _decide.Dispose();
//        _pause.Dispose();
//    }

//    FCpInputActions _pendingInputActions = new FCpInputActions();
//    bool bHasLastInput = false;
//    InputAction _lastInputAction = null;

//    private InputAction _move;
//    private InputAction _direction;
//    private InputAction _shoot;
//    private InputAction _decide;
//    private InputAction _pause;

//}

//// プレイヤーごとの入力をまとめる(プレイヤーの数だけ)
//public class CpInputActionMapUnit
//{
//    public void CreateInputActionMap()
//    {
//        InputActionMap = new CpInputActionMap();
//    }

//    public void UpdateAllInput()
//    {
//        _prevInput = _currentInput;
//        InputActionMap.ConsumePendingInput(ref _currentInput);
//    }

//    CpInputActionMap InputActionMap;

//    FCpInputActions _prevInput = new FCpInputActions();
//    FCpInputActions _currentInput = new FCpInputActions();

//    public void SetEnable(ECpInputType InputType, bool bEnable)
//    {
//        InputActionMap.SetEnable(InputType, bEnable);
//    }
//    public void SetEnable(bool bEnable)
//    {
//        InputActionMap.SetEnable(ECpInputType.Player, bEnable);
//        InputActionMap.SetEnable(ECpInputType.UI, bEnable);
//    }

//    public void SetInputType(ECpInputType InputType)
//    {
//        switch (InputType)
//        {
//            case ECpInputType.Player:
//                InputActionMap.SetEnable(ECpInputType.Player, true);
//                InputActionMap.SetEnable(ECpInputType.UI, false);
//                break;

//            case ECpInputType.UI:
//                InputActionMap.SetEnable(ECpInputType.Player, false);
//                InputActionMap.SetEnable(ECpInputType.UI, true);
//                break;

//            default:
//                CpDebug.LogError();
//                break;
//        }
//    }

//    public void ResetInput()
//    {
//        _prevInput.Reset();
//        _currentInput.Reset();
//    }

//    public void Dispose()
//    {
//        InputActionMap.Dispose();
//    }

//    public bool isPressed(ECpButton button)
//    {
//        bool pp = _prevInput.GetBool(button);
//        bool cp = _currentInput.GetBool(button);
//        return ((pp ^ cp) && !pp);
//    }
//    public bool isSomePressed(params ECpButton[] Buttons)
//    {
//        foreach (ECpButton Button in Buttons)
//        {
//            if (isPressed(Button))
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    public bool isPressHold(ECpButton button)
//    {
//        return _currentInput.GetBool(button);
//    }
//    public bool isSomePressHold(params ECpButton[] Buttons)
//    {
//        foreach (ECpButton Button in Buttons)
//        {
//            if (isPressHold(Button))
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    public bool isReleased(ECpButton button)
//    {
//        bool pp = _prevInput.GetBool(button);
//        bool cp = _currentInput.GetBool(button);
//        return ((pp ^ cp) & pp);
//    }
//    public bool isAllReleased(params ECpButton[] Buttons)
//    {
//        foreach (ECpButton Button in Buttons)
//        {
//            if (!isReleased(Button))
//            {
//                return false;
//            }
//        }

//        return true;
//    }

//    public Vector2 GetMove()
//    {
//        return _currentInput.move;
//    }

//    public bool HasLastInput() { return InputActionMap.HasLastInput(); }
//}

//public class CpInputManager : SingletonMonoBehaviour<CpInputManager>
//{
//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    static void Initialize()
//    {
//        CpDebug.Log("Cpコントローラーが作成されました.");

//        GameObject Obj = new GameObject("CpInputManager", typeof(CpInputManager));
//        DontDestroyOnLoad(Obj);
//        Obj.SetActive(true);

//        CpInputManager inputMananager = Obj.GetComponent<CpInputManager>();
//        inputMananager.enabled = true;
//    }

//    private void Awake()
//    {
//        _inputActionMapUnit.CreateInputActionMap();
//    }

//    private void Update()
//    {
//        _inputActionMapUnit.UpdateAllInput();
//    }

//    public Vector2 GetMoveInput()
//    {
//        Vector2 deltaMove = Vector2.zero;
//        deltaMove += _inputActionMapUnit.GetMove();
//        if (Mathf.Abs(deltaMove.x) < FCpInputDefine.DeadZone)
//        {
//            deltaMove.x = 0f;
//        }
//        if (Mathf.Abs(deltaMove.y) < FCpInputDefine.DeadZone)
//        {
//            deltaMove.y = 0f;
//        }
//        return deltaMove;

//        //Vector2 deltaMove = Vector2.zero;
//        //if (Input.GetKey(KeyCode.W))
//        //{
//        //    deltaMove.y += 1f;
//        //}
//        //if (Input.GetKey(KeyCode.S))
//        //{
//        //    deltaMove.y -= 1f;
//        //}
//        //if (Input.GetKey(KeyCode.D))
//        //{

//        //    deltaMove.x += 1f;
//        //}
//        //if (Input.GetKey(KeyCode.A))
//        //{

//        //    deltaMove.x -= 1f;
//        //}
//        //return deltaMove;
//    }

//    public Vector2 getDirectionInput()
//    {
//        float horizontal = Input.GetAxis("Horizontal");
//        float vertical = Input.GetAxis("Vertical");
//        Vector2 direction = new Vector2(horizontal, vertical);
//        return direction;
//    }

//    public bool isPressed(ECpButton button)
//    {
//        bool bPressed = false;
//        bPressed |= _inputActionMapUnit.isPressed(button);
//        return bPressed;
//    }
//    public bool IsPressHold(ECpButton button)
//    {
//        return _inputActionMapUnit.isPressHold(button);
//    }
//    public bool IsReleased(ECpButton button)
//    {
//        return _inputActionMapUnit.isReleased(button);
//    }

//    CpInputActionMapUnit _inputActionMapUnit = new CpInputActionMapUnit();

//}
