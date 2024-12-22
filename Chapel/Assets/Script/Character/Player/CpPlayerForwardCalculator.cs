using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPlayerForwardCalculator
{
    public CpPlayerForwardCalculator(Transform inPlayerTransform)
    {
        playerTransfomrm = inPlayerTransform;
    }

    public void execute()
    {
        var controller = CpInputManager.Instance;
        Vector2 currentDirection = controller.getDirectionInput();

        const float directionInputDeadZone = 0.8f;
        float currentDirInputSize = currentDirection.magnitude;
        if (currentDirInputSize > directionInputDeadZone)
        {
            latestForwardVector = currentDirection;
        }

        CpDebug.Log("dir"+currentDirection);
    }

    public Vector2 getForwardVector()
    {
        return latestForwardVector;
    }


    Transform playerTransfomrm;
    Vector2 latestForwardVector = Vector2.right;
}
