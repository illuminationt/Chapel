using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject等で設定する値
[System.Serializable]
public abstract class CpMoveParamBase
{
}

// ゲーム内の進行状況に応じてMoverに渡したい値
public struct FCpMoverContext
{
    public Vector2 Velocity;
}

