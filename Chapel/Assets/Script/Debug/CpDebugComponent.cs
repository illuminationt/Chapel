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
        var input = CpInputManager.Get();
        CpDebug.Log("move=" + input.GetMoveInput());
        if (Input.GetKeyDown(KeyCode.I))
        {
            throw new InvalidOperationException();
        }

        //SltDebugDraw.DrawArrow(Vector2.zero, new Vector2(3, 4), Color.red, 0.11f);
    }
}
