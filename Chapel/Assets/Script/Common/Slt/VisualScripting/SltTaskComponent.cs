using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[DisallowMultipleComponent]
[RequireComponent(typeof(StateMachine))]
public class SltTaskComponent : MonoBehaviour
{
    [HideInInspector]
    public List<SltTaskBase> ActiveTasks = new List<SltTaskBase>();
    // Update��for���[�v���ŐV�����������ꂽ�^�X�N���ꎞ�ۑ�����z��.
    [HideInInspector]
    public List<SltTaskBase> PendingActiveTasks = new List<SltTaskBase>();
    // �^�X�N���܂Ƃ߂č폜���邽�߂Ɉꎞ�ۑ�����z��.
    [HideInInspector]
    public List<SltTaskBase> PendingRemoveTasks = new List<SltTaskBase>();

    // ���ɊJ�n�ς݃t���O
    bool bAlreadyStart = false;


    private void Update()
    {
        float DeltaTime = Time.deltaTime;
        for (int Index = ActiveTasks.Count - 1; Index >= 0; Index--)
        {
            SltTaskBase Task = ActiveTasks[Index];

            // ExternalCancel���ꂽ�ꍇ�ɔ�����Finish�`�F�b�N
            if (!Task.IsFinished)
            {
                ActiveTasks[Index].Update(DeltaTime);
            }
            if (ActiveTasks[Index].IsFinished)
            {
                ActiveTasks.RemoveAt(Index);
            }
        }

        // ����for���[�v���Ő������ꂽTask��ActiveTask��ɒǉ�
        // List�̌����Fhttps://www.delftstack.com/ja/howto/csharp/how-to-join-two-lists-together-in-csharp/
        ActiveTasks.AddRange(PendingActiveTasks);
        PendingActiveTasks.Clear();
    }

    public void RegisterTask(SltTaskBase newTask)
    {
        if (newTask == null)
        {
            Assert.IsTrue(false);
            return;
        }

        // ����Unit����A�P�t���[���ɕ�����^�X�N�����s�����̂�h��(Unit��GUID���g���Ĕ��肷��j
        bool bSameGUID = PendingActiveTasks.Any(x => x.UnitGuid == newTask.UnitGuid);
        if (bSameGUID)
        {
            Assert.IsTrue(false);
            return;
        }

        newTask.Owner = gameObject;
        newTask.OwnerTaskComponent = this;

        // �^�X�N�J�n
        PendingActiveTasks.Add(newTask);
        newTask.OnStart();
    }

    public void RegisterRemoveTask(SltTaskBase RemovedTask)
    {
        PendingRemoveTasks.Add(RemovedTask);
    }

    public void StartStateMachine()
    {
        if (bAlreadyStart)
        {
            Debug.LogError("���ɊJ�n�ς�");
            return;
        }
        StateMachine stateMachine = GetComponent<StateMachine>();

        // StateMachine��������Ɛݒ肳��Ă邩�`�F�b�N
#if UNITY_EDITOR
        if (stateMachine.nest.macro == null)
        {
            Debug.LogError("StateMachine graph is null");
        }
#endif
        stateMachine.enabled = true;
        bAlreadyStart = true;
    }
}
