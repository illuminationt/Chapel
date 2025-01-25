using UnityEngine;
using UImGui;
using ImGuiNET;
using Unity.VisualScripting;

public class CpImGui : MonoBehaviour
{
#if CP_DEBUG
    [SerializeField]
    RenderTexture _imGuiRenderTexture = null;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        imGuiComponent = GetComponent<UImGui.UImGui>();
    }
    private void Start()
    {
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ImGui.SetNextWindowPos(new Vector2(50, 50), ImGuiCond.Once);
            bShowRootWindow = !bShowRootWindow;
            if (bShowRootWindow)
            {
                UImGuiUtility.Layout += DrawImGuiRoot;
            }
            else
            {
                UImGuiUtility.Layout -= DrawImGuiRoot;
            }
        }

        _imGuiRenderTexture.Release();
        _imGuiRenderTexture.width = Screen.width;
        _imGuiRenderTexture.height = Screen.height;
        _imGuiRenderTexture.Create();
    }

    public void SetActive(bool bActive)
    {
        gameObject.SetActive(bActive);
    }

    void DrawImGuiRoot(UImGui.UImGui uImGui)
    {
        if (ImGui.BeginTabBar("Tab"))
        {
            // タブ1
            if (ImGui.BeginTabItem("Player"))
            {
                CpPlayer player = CpPlayer.Get();
                player?.DrawImGui();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Enemy"))
            {
                DrawEnemy();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Item"))
            {
                DrawItem();
                ImGui.EndTabItem();
            }

            // タブ2
            if (ImGui.BeginTabItem("Dungeon"))
            {
                CpDungeonManager dungeonManager = CpDungeonManager.Get();
                dungeonManager.DrawImGui();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Gameplay State"))
            {
                CpGamePlayManager gameplayManager = CpGamePlayManager.Get();
                gameplayManager.DrawImGui();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("(未使用だよ)GameFlow"))
            {
                CpGameFlowManager flowManager = CpGameFlowManager.Get();
                flowManager.DrawImGui();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }
        ImGui.Text("DUMMY");
    }

    void DrawEnemy()
    {
        DrawCpActorBase<CpEnemyBase>();
    }

    void DrawItem()
    {
        DrawCpActorBase<CpDropItem>();
    }

    void DrawCpActorBase<T>()
    {
        CpActorBase[] actors = FindObjectsByType<CpActorBase>(FindObjectsSortMode.InstanceID);
        foreach (CpActorBase actor in actors)
        {
            if (typeof(T) == actor.GetType())
            {
                string treeTitle = actor.name + actor.GetInstanceID();
                if (ImGui.TreeNode(treeTitle))
                {
                    actor.DrawImGui();
                    ImGui.TreePop();
                }
            }
        }

    }

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    UImGui.UImGui imGuiComponent = null;
    bool bShowRootWindow = false;

#endif
}
