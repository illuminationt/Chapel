using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// 1��Shot�����Ƃ��̃p�����[�^
[System.Serializable]
public class CpPlayerWeaponShotParam
{
    [SerializeField] public CpPlayerShot PlayerShot = null;
    [SerializeField] public FCpShotSpawnLocationParam LocationParam;
    [SerializeField] public FCpMoveParam MoveParam;
}

// �P�̕��킪���p�����[�^�̍\���v�f
// �P�̃p�����[�^�ŕ��G�ȋ������\�z������A�P���ȋ�����g�ݍ��킹��ق����y
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
        // �p����ł̂ݎ������Ă�������
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

// ��莞�ԊԊu�Ŏw�肵���e�𔭎˂���
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

    // �����l�͑傫�Ȓl
    float _timer = 100f;
}

// �P�̕��킪���p�����[�^�����ׂĂ܂Ƃ߂�����
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
