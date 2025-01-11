using UnityEngine;

#if DEBUG
using ImGuiNET;
#endif

public class CpPlayerAmmo
{
    public void AddAmmo(int delta)
    {
        _currentAmmo += delta;
    }

    public bool IsRemainAmmo()
    {
        return _currentAmmo > 0;
    }

    int _currentAmmo = 10;
#if DEBUG
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
