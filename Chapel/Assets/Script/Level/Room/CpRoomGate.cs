using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpRoomGate : MonoBehaviour
{
    SpriteRenderer _sprite = null;
    BoxCollider2D _collider = null;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Open(bool bOpen)
    {
        if (_sprite != null)
        {
            _sprite.enabled = !bOpen;
        }
        if (_collider != null)
        {
            _collider.enabled = !bOpen;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 defaultDirection = Vector2.up;
        float objRotationZ = transform.eulerAngles.z;

        Vector2 gateDir = SltMath.RotateVector(defaultDirection, objRotationZ);

        Vector2 start = transform.position;
        SltDebugDrawOnGizmos.DrawArrow(start, gateDir, 24, Color.green);
    }
}
