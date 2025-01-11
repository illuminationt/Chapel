using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPlayerShootSlotElement
{

}

// プレイヤーが武器を保持するためのスロット。
// スロットを更新すると、その時装備してる武器に応じた処理を行う
public class CpPlayerShootSlot
{
    public void RegisterWeapon(CpPlayerWeaponParam weaponParam)
    {
        _weaponParam = weaponParam;
    }
    public void Update(in FCpShootControlParam controlParam, ref FCpShootControlResult refResult)
    {
        _weaponParam?.Update(controlParam, ref refResult);
    }

    // スロットが現在保持している武器パラメータ(不変)
    CpPlayerWeaponParam _weaponParam;
}
