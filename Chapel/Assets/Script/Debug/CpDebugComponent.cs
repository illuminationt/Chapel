using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
public class CpDebugComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var input = CpInputManager.Instance;
        if (input.WasPressed(ECpButton.Shoot))
        {
            CpDebug.LogError("SHOOT PRESSED");
        }

        if (input.IsPressHold(ECpButton.Shoot))
        {
            CpDebug.LogError("SHOOT HOLD");
        }

        if (input.WasReleased(ECpButton.Shoot))
        {
            CpDebug.LogError("SHOOT RELEASED");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            input.SwitchInputAction(ECpInputActionType.Player);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            input.SwitchInputAction(ECpInputActionType.UI);
        }

        if (Input.GetKey(KeyCode.T))
        {
            Vector2 mouseLocation = input.GetMouseLocation();
            CpDebug.Log(mouseLocation);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            throw new InvalidOperationException();
        }

        SltDebugDraw.DrawArrow(Vector2.zero, new Vector2(3, 4), Color.red, 0.11f);
    }
}
