using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class CpPlayerWeaponShotGeneralParam
{
    public float LifeTime = -1f;
}

// 1つのShotを撃つときのパラメータ
[System.Serializable]
public class CpPlayerWeaponShotParam
{
    [SerializeField]
    public CpPlayerShot PlayerShot = null;
    [SerializeField]
    public CpPlayerWeaponShotGeneralParam GeneralParam = null;
    [SerializeField]
    public FCpShotSpawnLocationParam LocationParam;

    [SerializeReference]
    public ICpMoveParam MoveParam;

    [SerializeReference]
    public List<ICpActionParam> ActionParams;

    public bool IsActionEnable(int actionIndex)
    {
#if CP_EDITOR
        if (IgnoreActionIndexes.Contains(actionIndex))
        {
            return false;
        }
#endif
        return true;
    }

#if CP_EDITOR
    public List<int> IgnoreActionIndexes = new List<int>();
#endif
}

// １つの武器が持つパラメータの構成要素
// １つのパラメータで複雑な挙動を構築するより、単純な挙動を組み合わせるほうが楽
[System.Serializable]
public class CpPlayerWeaponParamElementBase
{
    public void Update(in FCpShootControlParam controlParam, ref FCpShootControlResult refResult)
    {
        CpInputManager input = CpInputManager.Get();
        bool bWasPressed = input.WasPressed(ECpButton.Shoot);
        bool bPressHold = input.IsPressHold(ECpButton.Shoot);
        bool bWasReleased = input.WasReleased(ECpButton.Shoot);

        UpdateInternal(controlParam, bWasPressed, bPressHold, bWasReleased, ref refResult);
    }
    protected virtual void UpdateInternal(in FCpShootControlParam controlParam, bool bPressed, bool bPressHold, bool bReleased, ref FCpShootControlResult refResult)
    {
        // 継承先でのみ実装してください
    }

    protected bool CreateShot(in FCpShootControlParam controlParam, CpPlayerWeaponShotParam weaponShotParam)
    {
        CpPlayerShot newShot = CpObjectPool.Get().Get(weaponShotParam.PlayerShot);
        newShot.OnCreated(weaponShotParam.GeneralParam, controlParam);

        // 共通パラメータ設定

        // 初期座標設定
        FCpShotSpawnLocationRequestParam locationReqParam;
        locationReqParam.origin = controlParam.origin;
        locationReqParam.forwardDegree = SltMath.ToDegree(controlParam.forward);
        Vector2 spawnLoc = weaponShotParam.LocationParam.GetLocation(locationReqParam);
        newShot.transform.position = spawnLoc;

        // 移動開始
        CpMoveComponent moveComp = newShot.GetComponent<CpMoveComponent>();
        moveComp.RequestStart(weaponShotParam.MoveParam);

        // アクション開始
        ICpActRunnable actRunnable = newShot;
        for (int actionIndex = 0; actionIndex < weaponShotParam.ActionParams.Count; actionIndex++)
        {
            if (weaponShotParam.IsActionEnable(actionIndex))
            {
                ICpActionParam iactionParam = weaponShotParam.ActionParams[actionIndex];
                actRunnable.RequestStart(iactionParam);
            }
        }

        return true;
    }
}

// 一定時間間隔で指定した弾を発射する
[System.Serializable]
public class CpPlayerWeaponParamElementInterval : CpPlayerWeaponParamElementBase
{
    protected override void UpdateInternal(in FCpShootControlParam controlParam, bool bPressed, bool bPressHold, bool bReleased, ref FCpShootControlResult refResult)
    {
        _timer += CpTime.DeltaTime;

        if (!controlParam.Ammo.IsRemainAmmo())
        {
            return;
        }

        if (bPressHold)
        {
            if (_timer > Interval)
            {
                _timer = 0f;
                CreateShot(controlParam, WeaponShotParam);
                refResult.AmmoDelta -= 1;
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
    public void Update(in FCpShootControlParam controlParam, ref FCpShootControlResult refResult)
    {
        foreach (var element in _weaponElements)
        {
            element.Update(controlParam, ref refResult);
        }
    }
    [SerializeField] List<CpPlayerWeaponParamElementInterval> _weaponElements;
}
