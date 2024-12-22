using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ECpButton
{
    Shoot,
    Decide,
    Pause,
}

public struct FCpInputActions
{
    public Vector2 move;
    public bool bShoot;
    public bool bDecide;
    public bool bPause;
}


// InputActionからの入力を管理
public class CpInputActionMap
{
    public CpInputActionMap()
    {
        _move = new InputAction("Move", InputActionType.Value);
        _direction = new InputAction("Directino", InputActionType.Value);
        _shoot = new InputAction("Shoot", InputActionType.Button);
        _decide = new InputAction("Decide", InputActionType.Button);
        _pause = new InputAction("Pause", InputActionType.Button);
    }

    public void ConsumePendingInput(ref FCpInputActions Actions)
    {
        PendingInputActions.move = GetVector2InputValue(Move);
        PendingInputActions.fire = GetButtonInputValue(Fire);
        PendingInputActions.weapon = GetButtonInputValue(Weapon);
        PendingInputActions.gear = GetButtonInputValue(Gear);
        PendingInputActions.pause = GetButtonInputValue(Pause);
        // UI
        Vector2 StickMove = PendingInputActions.move;
        PendingInputActions.UiMoveUp = GetUiMoveInputValue(UiMoveUp, StickMove.y, true);
        PendingInputActions.UiMoveDown = GetUiMoveInputValue(UiMoveDown, StickMove.y, false);
        PendingInputActions.UiMoveLeft = GetUiMoveInputValue(UiMoveLeft, StickMove.x, false);
        PendingInputActions.UiMoveRight = GetUiMoveInputValue(UiMoveRight, StickMove.x, true);

        PendingInputActions.UiCancel = GetButtonInputValue(UiCancel);
        PendingInputActions.UiOk = GetButtonInputValue(UiOk);

        Actions = PendingInputActions;

        // 入力があったかチェック
        bHasLastInput = FindHasLastInput();
    }

    Vector2 GetVector2InputValue(InputAction Action)
    {
        if (Action.activeControl == null)
        {
            return Vector2.zero;
        }

        Vector2 RetValue = Action.ReadValue<Vector2>();
        if (!CtdFuncLib.IsNearlyZero(RetValue, CtdInputDefine.DeadZone))
        {
            SetLastInputAction(Move);
        }
        return RetValue;
    }

    private InputAction _move;
    private InputAction _direction;
    private InputAction _shoot;
    private InputAction _decide;
    private InputAction _pause;
}

// プレイヤーごとの入力をまとめる(プレイヤーの数だけ)
public class CpInputActionMapUnit
{
    public void CreateInputActionMap()
    {
        InputActionMap = CpInputActionMap.Create();
    }

    public void UpdateAllInput()
    {
        _prevInput = _currentInput;
        InputActionMap.ConsumePendingInput(ref _currentInput);
    }

    CpInputActionMap InputActionMap;

    FCtdInputActions _prevInput = new FCtdInputActions();
    FCtdInputActions _currentInput = new FCtdInputActions();

    public void SetEnable(ECtdInputType InputType, bool bEnable)
    {
        InputActionMap.SetEnable(InputType, bEnable);
    }
    public void SetEnable(bool bEnable)
    {
        InputActionMap.SetEnable(ECtdInputType.PlayerWithBit, bEnable);
        InputActionMap.SetEnable(ECtdInputType.UI, bEnable);
    }

    public void SetInputType(ECtdInputType InputType)
    {
        switch (InputType)
        {
            case ECtdInputType.PlayerWithBit:
                InputActionMap.SetEnable(ECtdInputType.PlayerWithBit, true);
                InputActionMap.SetEnable(ECtdInputType.UI, false);
                break;

            case ECtdInputType.UI:
                InputActionMap.SetEnable(ECtdInputType.PlayerWithBit, false);
                InputActionMap.SetEnable(ECtdInputType.UI, true);
                break;

            default:
                CtdDebug.LogError();
                break;
        }
    }

    public void ResetInput()
    {
        _prevInput.Reset();
        _currentInput.Reset();
    }

    public void Dispose()
    {
        InputActionMap.Dispose();
    }

    public bool isPressed(ECtdButton button)
    {
        bool pp = _prevInput.getBool(button);
        bool cp = _currentInput.getBool(button);
        return ((pp ^ cp) && !pp);
    }
    public bool isSomePressed(params ECtdButton[] Buttons)
    {
        foreach (ECtdButton Button in Buttons)
        {
            if (isPressed(Button))
            {
                return true;
            }
        }
        return false;
    }

    public bool isPressHold(ECtdButton button)
    {
        return _currentInput.getBool(button);
    }
    public bool isSomePressHold(params ECtdButton[] Buttons)
    {
        foreach (ECtdButton Button in Buttons)
        {
            if (isPressHold(Button))
            {
                return true;
            }
        }
        return false;
    }

    public bool isReleased(ECtdButton button)
    {
        bool pp = _prevInput.getBool(button);
        bool cp = _currentInput.getBool(button);
        return ((pp ^ cp) & pp);
    }
    public bool isAllReleased(params ECtdButton[] Buttons)
    {
        foreach (ECtdButton Button in Buttons)
        {
            if (!isReleased(Button))
            {
                return false;
            }
        }

        return true;
    }

    public Vector2 GetMove()
    {
        return _currentInput.move;
    }

    public bool HasLastInput() { return InputActionMap.HasLastInput(); }

    public InputAction GetInputAction(ECtdKeyboardItem ItemType)
    {
        return InputActionMap.GetInputAction(ItemType);
    }

    public ECtdInputDeviceType GetLastInputDeviceType()
    {
        return InputActionMap.LastInputDeviceType;
    }
}

public class CpInputManager : SingletonMonoBehaviour<CpInputManager>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        CpDebug.Log("Cpコントローラーが作成されました.");

        GameObject Obj = new GameObject("CpInputManager", typeof(CpInputManager));
        DontDestroyOnLoad(Obj);
        Obj.SetActive(true);

        CpInputManager inputMananager = Obj.GetComponent<CpInputManager>();
        inputMananager.enabled = true;
    }

    private void Update()
    {

    }

    public Vector2 getMoveInput()
    {
        Vector2 deltaMove = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            deltaMove.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            deltaMove.y -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {

            deltaMove.x += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {

            deltaMove.x -= 1f;
        }
        return deltaMove;
    }

    public Vector2 getDirectionInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        return direction;
    }

    private InputAction move;
}
