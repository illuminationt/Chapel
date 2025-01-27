using UnityEngine;
using UniRx;

#if CP_DEBUG
using ImGuiNET;
#endif

public class CpPlayerAmmo
{
    public CpPlayerAmmo()
    {
    }

    public void AddAmmo(int delta)
    {
        _currentAmmo.Value += delta;
    }

    public bool IsRemainAmmo()
    {
        return _currentAmmo.Value > 0;
    }

    readonly ReactiveProperty<int> _currentAmmo = new ReactiveProperty<int>(10000);
    public IReadOnlyReactiveProperty<int> CurrentAmmo => _currentAmmo;
#if CP_DEBUG
    public void DrawImGui()
    {
        string currentAmmoStr = $"CurrentAmmo = {_currentAmmo}";
        ImGui.Text(currentAmmoStr);
        int delta = 0;
        if (ImGui.Button("+10")) { delta += 10; }
        ImGui.SameLine();
        if (ImGui.Button("-1")) { delta--; }
        if (delta != 0)
        {
            AddAmmo(delta);
        }
    }
#endif
}
