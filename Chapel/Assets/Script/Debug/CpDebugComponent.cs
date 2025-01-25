using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Linq;
using UImGui;
public class CpDebugComponent : MonoBehaviour
{
    public CpHellParamScriptableObject HellParamScriptableObject = null;
    public CpEnemyBase EnemyPrefab = null;
    public CpEnemySpawnParamScriptableObject ESP_Test = null;
    public CpFloorMasterDataScriptableObject floorMasterData = null;

    public GameObject roomPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CpTaskComponent taskComp = GetComponent<CpTaskComponent>();
            taskComp.StartStateMachine();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CpHellComponent hellComp = GetComponent<CpHellComponent>();
            hellComp.RequestStart(HellParamScriptableObject.MultiHellParam);
        }

        //_timer += CpTime.DeltaTime;
        //if (_timer > 1f && count < 11111111)
        //{
        //    _timer = 0f;
        //    count++;
        //    CpEnemyBase newEnemy = Instantiate(EnemyPrefab); ;
        //    newEnemy.transform.position = new Vector2(0f, 70f);
        //}

        if (Input.GetKeyDown(KeyCode.K))
        {
            CpEnemyShot[] shots = FindObjectsByType<CpEnemyShot>(FindObjectsSortMode.None);
            CpDebug.LogError("EnemyShot Num:" + shots.Count());
            for (int i = shots.Count() - 1; i >= 0; i--)
            {
                shots[i].DebugRelease();
            }
            foreach (CpEnemyShot shot in shots)
            {
                shot.DebugRelease();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject go = Instantiate(roomPrefab);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CpGamePlayManager gamePlayManager = CpGamePlayManager.Get();
            gamePlayManager.RequestEnterFloor(floorMasterData);
        }

    }
    private void OnEnable()
    {
        UImGuiUtility.Layout += OnLayout;

    }

    private void OnDisable()
    {
        UImGuiUtility.Layout -= OnLayout;

    }

    void OnLayout(UImGui.UImGui uImGui)
    {
    }
}
