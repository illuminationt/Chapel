using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPilotComponent : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    public void execute()
    {
        var input = CpInputManager.Instance;

        Vector2 deltaMove = input.GetMoveInput();
        deltaMove *= _speed * Time.deltaTime;
        move(deltaMove);
    }

    void move(in Vector2 deltaMove)
    {
        transform.AddToPosition(deltaMove);
    }
}
