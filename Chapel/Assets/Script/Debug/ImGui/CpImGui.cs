using UnityEngine;
using UImGui;
using ImGuiNET;
using Unity.VisualScripting;

public class CpImGui : MonoBehaviour
{
#if DEBUG
    private void Awake()
    {
        DontDestroyOnLoad(this);
        imGuiComponent = GetComponent<UImGui.UImGui>();
    }
    private void Start()
    {
        Camera camera = Camera.main;
        imGuiComponent.SetCamera(camera);
    }
    void Update()
    {
        Camera camera = Camera.main;
        imGuiComponent.SetCamera(camera);

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

            // タブ2
            if (ImGui.BeginTabItem("Dungeon"))
            {
                CpDungeonManager dungeonManager = CpDungeonManager.Get();
                dungeonManager.DrawImGui();
                ImGui.EndTabItem();
            }


            ImGui.EndTabBar();
        }
        ImGui.Text("DUMMY");
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
