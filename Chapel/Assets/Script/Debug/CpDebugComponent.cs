using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (input.IsPressHold(ECpButton.Shoot)) {
            CpDebug.LogError("SHOOT HOLD");
        }

        if(input.WasReleased(ECpButton.Shoot))
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
    }
}
