using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPlayerShootSlotElement
{

}

// �v���C���[�������ێ����邽�߂̃X���b�g�B
// �X���b�g���X�V����ƁA���̎��������Ă镐��ɉ������������s��
public class CpPlayerShootSlot
{
    public void RegisterWeapon(CpPlayerWeaponParam weaponParam)
    {
        _weaponParam = weaponParam;
    }
    public void Update(in FCpShootControlParam controlParam)
    {
        _weaponParam?.Update(controlParam);
    }

    // �X���b�g�����ݕێ����Ă��镐��p�����[�^(�s��)
    CpPlayerWeaponParam _weaponParam;
}
