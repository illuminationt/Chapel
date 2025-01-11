using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPilotComponent : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    Rigidbody2D _rigidbody2D = null;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Execute()
    {
        var input = CpInputManager.Get();

        //Vector2 deltaMove = input.GetMoveInput();
        //deltaMove *= _speed * Time.fixedDeltaTime;
        //move(deltaMove);

        Vector2 newDir = input.GetMoveInput();
        Vector2 newVelocity = newDir * _speed;
        _rigidbody2D.linearVelocity = newVelocity;
    }

    void move(in Vector2 deltaMove)
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 newPosition = currentPosition + deltaMove;
        _rigidbody2D.MovePosition(currentPosition + deltaMove);

        CpDebug.LogWarning("new:" + newPosition + "delta:" + deltaMove);
        //transform.AddToPosition(deltaMove);
    }
}
