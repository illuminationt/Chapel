using ImGuiNET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class CpGameFlowUtil
{
    public static bool CanStart(ECpGameFlowType newFlowType, ECpGameFlowType activeFlowType)
    {
        int newFlowValue = (int)newFlowType;
        int activeFlowValue = (int)activeFlowType;
        if (newFlowValue > activeFlowValue)
        {
            return true;
        }
        else { return false; }
    }

    public static System.Type GetFlowElementType(ECpGameFlowType flowType)
    {
        System.Type flowElementType = flowClassDict[flowType];
        return flowElementType;
    }

    static readonly Dictionary<ECpGameFlowType, System.Type> flowClassDict = new Dictionary<ECpGameFlowType, System.Type>() {
        { ECpGameFlowType.Root, typeof(CpGameFlowElementRoot) },
        { ECpGameFlowType.GamePlay, typeof(CpFlowGamePlay) },
        { ECpGameFlowType.RoomTransition, typeof(CpFlowRoomTransition) },
        { ECpGameFlowType.SceneTransition, typeof(CpFlowSceneTransition) },
 };
}

public class CpGameFlowManager
{
    public static CpGameFlowManager Create()
    {
        CpGameFlowManager manager = new CpGameFlowManager();
        return manager;
    }
    public static CpGameFlowManager Get()
    {
        return CpGameManager.Instance.GameFlowManager;
    }

    public bool CanStart(ECpGameFlowType newFlowType)
    {
        CpGameFlowElementBase activeFlow = GetActiveFlowElement();

        ECpGameFlowType activeFlowType = activeFlow.GetGameFlowType();
        bool bCanStart = CpGameFlowUtil.CanStart(newFlowType, activeFlowType);
        return bCanStart;
    }
    public T RequestStart<T>(ECpGameFlowType newFlowType) where T : CpGameFlowElementBase
    {
        if (!CanStart(newFlowType))
        {
            return null;
        }

        System.Type newFlowSystemType = CpGameFlowUtil.GetFlowElementType(newFlowType);
        CpGameFlowElementBase newFlow = (CpGameFlowElementBase)Activator.CreateInstance(newFlowSystemType);

        CpGameFlowElementBase activeFlow = GetActiveFlowElement();
        activeFlow.SetChild(newFlow);

        Assert.IsTrue(newFlow.GetType() == typeof(T));
        return (T)newFlow;
    }

    public bool CanUpdate(ECpGameFlowType flowType)
    {
        ECpGameFlowType activeFlowType = GetActiveFlowType();
        return flowType == activeFlowType;
    }

    public CpGameFlowElementBase GetActiveFlowElement()
    {
        if (_rootGameFlowElement == null)
        {
            return null;
        }

        CpGameFlowElementBase parent = _rootGameFlowElement;
        CpGameFlowElementBase child = parent.GetChild();
        int loopCounter = 0;
        while (true)
        {
            if (child == null)
            {
                return parent;
            }

            CpGameFlowElementBase prevChild = child;
            parent = child;
            child = prevChild.GetChild();
            if (loopCounter++ > 1000)
            {
                Assert.IsTrue(false);
                return null;
            }
        }
    }
    ECpGameFlowType GetActiveFlowType()
    {
        CpGameFlowElementBase activeFlow = GetActiveFlowElement();
        return activeFlow.GetGameFlowType();
    }

    CpGameFlowElementBase _rootGameFlowElement = null;

#if DEBUG

    ECpGameFlowType reqFlowType = ECpGameFlowType.None;
    public void DrawImGui()
    {
        if (ImGui.TreeNode("Request Flow"))
        {
            SltImGui.EnumValueCombo(ref reqFlowType);
            if (ImGui.Button(""))
            {
                switch (reqFlowType)
                {
                    case ECpGameFlowType.None:
                        break;
                    case ECpGameFlowType.Root:
                        break;
                    case ECpGameFlowType.GamePlay:
                        break;
                    case ECpGameFlowType.RoomTransition:
                        break;
                    case ECpGameFlowType.EnemyAppearance:
                        break;
                    case ECpGameFlowType.SceneTransition:
                        break;
                }
            }
        }
    }
#endif
}
