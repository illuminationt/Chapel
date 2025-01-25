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

// 1��Shot�����Ƃ��̃p�����[�^
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

// �P�̕��킪���p�����[�^�̍\���v�f
// �P�̃p�����[�^�ŕ��G�ȋ������\�z������A�P���ȋ�����g�ݍ��킹��ق����y
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
        // �p����ł̂ݎ������Ă�������
    }

    protected bool CreateShot(in FCpShootControlParam controlParam, CpPlayerWeaponShotParam weaponShotParam)
    {
        CpPlayerShot newShot = CpObjectPool.Get().Get(weaponShotParam.PlayerShot);
        newShot.OnCreated(weaponShotParam.GeneralParam, controlParam);

        // ���ʃp�����[�^�ݒ�

        // �������W�ݒ�
        FCpShotSpawnLocationRequestParam locationReqParam;
        locationReqParam.origin = controlParam.origin;
        locationReqParam.forwardDegree = SltMath.ToDegree(controlParam.forward);
        Vector2 spawnLoc = weaponShotParam.LocationParam.GetLocation(locationReqParam);
        newShot.transform.position = spawnLoc;

        // �ړ��J�n
        CpMoveComponent moveComp = newShot.GetComponent<CpMoveComponent>();
        moveComp.RequestStart(weaponShotParam.MoveParam);

        // �A�N�V�����J�n
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

// ��莞�ԊԊu�Ŏw�肵���e�𔭎˂���
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

    // �����l�͑傫�Ȓl
    float _timer = 100f;
}

// �P�̕��킪���p�����[�^�����ׂĂ܂Ƃ߂�����
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
