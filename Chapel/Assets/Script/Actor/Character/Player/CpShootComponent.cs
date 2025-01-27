using ImGuiNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public struct FCpShootControlParam
{
    public CpPlayer Player;
    public CpPlayerAmmo Ammo;
    public Vector2 origin;
    public Vector2 forward;
}

public struct FCpShootControlResult
{
    public int AmmoDelta;
}

public class CpShootComponent : MonoBehaviour
{
    public CpPlayerWeaponParamScriptableObject PlayerWeaponParamScriptableObject;
    public CpPlayerWeaponParamScriptableObject PlayerWeaponParamScriptableObject2;

    CpPlayerShootSlot _slotDefault = new CpPlayerShootSlot();
    CpPlayerShootSlot _slotSpecial = new CpPlayerShootSlot();
    CpPlayerAmmo _ammo = new CpPlayerAmmo();
    CpPlayer _ownerPlayer = null;

    //
    public IReadOnlyReactiveProperty<int> OnAmmoChanged { get { return _ammo.CurrentAmmo; } }
    //

    public void Initialize(CpPlayer ownerPlayer)
    {
        _ownerPlayer = ownerPlayer;

        ICpGameplayEffectReciever geReciever = _ownerPlayer;
        geReciever.OnGameplayEffectInvoked.AddListener(OnGameplayEffectInvoke);
    }

    // 弾丸発射処理.
    // 外部からのパラメータはcontrolParamで受け取る.
    public void execute(FCpShootControlParam controlParam)
    {
        controlParam.Ammo = _ammo;

        FCpShootControlResult defaultSlotResult = new FCpShootControlResult();
        _slotDefault.Update(controlParam, ref defaultSlotResult);
        _ammo.AddAmmo(defaultSlotResult.AmmoDelta);

        FCpShootControlResult specialSlotResult = new FCpShootControlResult();
        _slotSpecial.Update(controlParam, ref specialSlotResult);



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

    void OnGameplayEffectInvoke(CpGameplayEffectBase geeffect)
    {
        if (geeffect is CpGEAddPlayerAmmo gePlayerAmmo)
        {
            _ammo.AddAmmo(gePlayerAmmo.DeltaAmmo);
        }
    }


#if CP_DEBUG
    public void DrawImGui()
    {
        ImGui.Text("Player Shoot Component");
        if (ImGui.TreeNode("Ammo"))
        {
            _ammo.DrawImGui();
            ImGui.TreePop();
        }
    }
#endif
}
