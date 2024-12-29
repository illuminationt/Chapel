using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// 1つのShotを撃つときのパラメータ
[System.Serializable]
public class CpPlayerWeaponShotParam
{
    [SerializeField] public CpPlayerShot PlayerShot = null;
    [SerializeField] public FCpShotSpawnLocationParam LocationParam;
    [SerializeField] public FCpMoveParam MoveParam;
}

// １つの武器が持つパラメータの構成要素
// １つのパラメータで複雑な挙動を構築するより、単純な挙動を組み合わせるほうが楽
[System.Serializable]
public class CpPlayerWeaponParamElementBase
{
    public void Update(in FCpShootControlParam controlParam)
    {
        CpInputManager input = CpInputManager.Get();
        bool bWasPressed = input.WasPressed(ECpButton.Shoot);
        bool bPressHold = input.IsPressHold(ECpButton.Shoot);
        bool bWasReleased = input.WasReleased(ECpButton.Shoot);

        UpdateInternal(controlParam, bWasPressed, bPressHold, bWasReleased);
    }

    protected virtual void UpdateInternal(in FCpShootControlParam controlParam, bool bPressed, bool bPressHold, bool bReleased)
    {
        // 継承先でのみ実装してください
    }

    protected void CreateShot(in FCpShootControlParam controlParam, CpPlayerWeaponShotParam weaponShotParam)
    {
        CpPlayerShot newShot = CpObjectPool.Get().Create(weaponShotParam.PlayerShot);
        newShot.OnCreated(controlParam);

        FCpShotSpawnLocationRequestParam locationReqParam;
        locationReqParam.origin = controlParam.origin;
        locationReqParam.forwardDegree = SltMath.ToDegree(controlParam.forward);
        Vector2 spawnLoc = weaponShotParam.LocationParam.GetLocation(locationReqParam);
        newShot.transform.position = spawnLoc;

        ICpMover mover = newShot;
        mover.StartMove(weaponShotParam.MoveParam);
    }
}

// 一定時間間隔で指定した弾を発射する
[System.Serializable]
public class CpPlayerWeaponParamElementInterval : CpPlayerWeaponParamElementBase
{
    protected override void UpdateInternal(in FCpShootControlParam controlParam, bool bPressed, bool bPressHold, bool bReleased)
    {
        _timer += CpTime.DeltaTime;

        if (bPressHold)
        {
            if (_timer > Interval)
            {
                _timer = 0f;
                CreateShot(controlParam, WeaponShotParam);
            }
        }
    }

    [SerializeField] CpPlayerWeaponShotParam WeaponShotParam;
    [SerializeField] float Interval = 0.2f;

    // 初期値は大きな値
    float _timer = 100f;
}

// １つの武器が持つパラメータをすべてまとめたもの
[System.Serializable]
public class CpPlayerWeaponParam
{
    public void Update(in FCpShootControlParam controlParam)
    {
        foreach (var element in _weaponElements)
        {
            element.Update(controlParam);
        }
    }
    [SerializeField] List<CpPlayerWeaponParamElementInterval> _weaponElements;
}
