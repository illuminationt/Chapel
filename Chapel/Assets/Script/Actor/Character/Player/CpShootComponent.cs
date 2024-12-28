using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpShootControlParam
{
    public CpPlayer Player;
    public Vector2 origin;
    public Vector2 forward;
}

public class CpShootComponent : MonoBehaviour
{
    public GameObject testShotPrefab = null;
    public FCpMoveParamLinear MoveParamLinear;
    public CpPlayerWeaponParamScriptableObject PlayerWeaponParamScriptableObject;
    public CpPlayerWeaponParamScriptableObject PlayerWeaponParamScriptableObject2;

    CpPlayerShootSlot _slotDefault = new CpPlayerShootSlot();
    CpPlayerShootSlot _slotSpecial = new CpPlayerShootSlot();


    // �e�۔��ˏ���.
    // �O������̃p�����[�^��controlParam�Ŏ󂯎��.
    public void execute(in FCpShootControlParam controlParam)
    {
        _slotDefault.Update(controlParam);
        _slotSpecial.Update(controlParam);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            RegisterDefaultWeapon();
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            RegisterSpecialWeapon();
        }
    }

    void RegisterDefaultWeapon()
    {
        _slotDefault.RegisterWeapon(PlayerWeaponParamScriptableObject.WeaponParam);
    }
    void RegisterSpecialWeapon()
    {
        _slotDefault.RegisterWeapon(PlayerWeaponParamScriptableObject2.WeaponParam);
    }

    CpShotBase CreateShot(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab);
        return newObj.GetComponent<CpShotBase>();
    }
}
