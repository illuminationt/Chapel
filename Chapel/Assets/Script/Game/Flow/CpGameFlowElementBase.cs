using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using ImGuiNET;
using Unity.VisualScripting;
using Sirenix.OdinInspector.Editor.Internal.UIToolkitIntegration;

public enum ECpGameFlowType
{
    None = -1,
    Root = 0,
    GamePlay = 50,
    RoomTransition = 100,
    EnemyAppearance = 200,
    SceneTransition = 10000,
}

public abstract class CpGameFlowElementBase
{
    public abstract ECpGameFlowType GetGameFlowType();
    public void ReadyForActivation()
    {
        _bReadyForActivation = true;
        OnStart();
    }

    void OnStart()
    {
        OnStartInternal();
    }
    protected virtual void OnStartInternal() { }

    public void Update()
    {
        if (!_bReadyForActivation)
        {
            Assert.IsTrue(false, "ReadyForActivationが呼ばれていません");
        }
        if (_childFlowElement != null)
        {
            // 子供がいるならこのフローの更新は止める
            return;
        }
        UpdateInternal();
    }
    protected virtual void UpdateInternal() { }


    public void OnFinished()
    {
        OnFinishedInternal();

    }
    protected virtual void OnFinishedInternal() { }


    public void SetChild(CpGameFlowElementBase child)
    {
        _childFlowElement = child;
    }

    public CpGameFlowElementBase GetChild() => _childFlowElement;
    CpGameFlowElementBase _parentFlowElement = null;
    CpGameFlowElementBase _childFlowElement = null;
    bool _bReadyForActivation = false;

#if CP_DEBUG
    public void DrawImGui()
    {
        string flowStr = SltEnumUtil.ToString(GetGameFlowType());
        ImGui.Text($"FlowType={flowStr}");

        DrawImGuiInternal();

        if (_childFlowElement != null)
        {
            _childFlowElement.DrawImGui();
        }
        else
        {
            ImGui.Text("Child Flow DOESNOT exists");
        }
    }
    public virtual void DrawImGuiInternal()
    {

    }
#endif
}

public class CpGameFlowElementRoot : CpGameFlowElementBase
{
    public override ECpGameFlowType GetGameFlowType() { return ECpGameFlowType.Root; }

}