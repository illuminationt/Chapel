using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject���Őݒ肷��l
[System.Serializable]
public abstract class CpMoveParamBase
{
}

// �Q�[�����̐i�s�󋵂ɉ�����Mover�ɓn�������l
public struct FCpMoverContext
{
    public Vector2 Velocity;
}

