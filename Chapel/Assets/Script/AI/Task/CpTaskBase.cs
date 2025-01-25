using ImGuiNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECpTaskType
{
    None = 0,
    Move = 1,
    Rotate = 5,
    Hell = 10,
    Animation = 20,
    Wait = 50,
}

public abstract class CpTaskBase : SltTaskBase
{
    public abstract ECpTaskType GetTaskType();
    protected CpActorBase OwnerActor
    {
        get
        {
            if (_ownerActor == null)
            {
                _ownerActor = Owner.GetComponent<CpActorBase>();
            }
            return _ownerActor;
        }
    }
    CpActorBase _ownerActor = null;

#if CP_DEBUG
    public void DrawImGui()
    {
        string taskTypeStr = $"TaskType = {SltEnumUtil.ToString(GetTaskType())}";
        ImGui.Text(taskTypeStr);

        string classStr = $"TaskClass = {GetType().Name}";
        ImGui.Text(classStr);
    }
#endif
}

public abstract class CpUnitBase : SltUnitBase
{

}