using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Assertions;

// VisualScripting Unit�̊��
public abstract class SltUnitBase : Unit
{
    // �C���v�b�g�m�[�h
    [DoNotSerialize]
    [PortLabelHidden]
    public ControlInput input { get; private set; }

    // �A�E�g�v�b�g�i�f�t�H���g�A�m�[�h�������炷�����s����j
    [DoNotSerialize]
    [PortLabelHidden]// �t���[�̖��O���\���ɂ���
    public ControlOutput outputDefault { get; private set; }
    // �A�E�g�v�b�g�i�^�X�N�����j
    [DoNotSerialize]
    public ControlOutput outputSucceed { get; private set; }
    // �A�E�g�v�b�g�i�O������I��������ꂽ�j
    [DoNotSerialize]
    public ControlOutput outputExternal { get; private set; }

    // �ϐ���`

    // �A�E�g�v�b�g��Default��\������t���O;�p����̃R���X�g���N�^�Őݒ肵�܂�
    protected bool bUseDefault = true;
    // �A�E�g�v�b�g��OnSucceed��\������;�p����̃R���X�g���N�^�Őݒ肵�܂�
    protected bool bUseOnSucceed = true;
    // �A�E�g�v�b�g��OnExternal��\������;�p����̃R���X�g���N�^�Őݒ肵�܂�
    protected bool bUseOnFinishExternal = false;

    // �^�X�N�����֐��B�h����ŃI�[�o�[���C�h�K�{�B
    // Unit�ƃ^�X�N��1��1�Ή�����͂��I
    protected abstract SltTaskBase CreateTask();
    protected virtual void InitializeUnitVariables(Flow flow) { }

    protected sealed override void Definition()
    {
        input = ControlInput("", OnEnterUnit_Internal);

        if (bUseDefault)
        {
            outputDefault = ControlOutput("");
        }

        if (bUseOnSucceed)
        {
            outputSucceed = ControlOutput("OnSucceeded");
        }

        if (bUseOnFinishExternal)
        {
            outputExternal = ControlOutput("FinishExternal");
        }

        DefinitionInternal();
    }
    protected virtual void DefinitionInternal() { }

    private ControlOutput OnEnterUnit_Internal(Flow flow)
    {
        GraphReference graphReference = flow.stack.ToReference();
        GameObject Self = flow.stack.self;
        SltTaskComponent ownerComponent = Self.GetComponent<SltTaskComponent>();
        if (ownerComponent == null)
        {
            Assert.IsTrue(false);
            //SltDebug.LogError(Self.name + "'s TaskComponent is null");
            return null;
        }

        // �^�X�N�Ŏg�p����Unit�̕ϐ���������
        InitializeUnitVariables(flow);

        // �^�X�N���������s����R���|�[�l���g�ɓo�^
        SltTaskBase task = CreateTask();
        task.UnitGuid = guid;
        // �SUnit���ʂŎ���Output�t���[��o�^
        if (outputDefault != null && outputDefault.hasAnyConnection)
        {
            task.OnTaskStart.AddListener(() => Flow.New(graphReference).Run(outputDefault));
        }

        // Unit����o�邽�߂Ɏ��s����UnityAction�Ƀt���[�̊֐����o�C���h
        if (outputSucceed != null && outputSucceed.hasAnyConnection)
        {
            task.OnFinishAction.AddListener(() => Flow.New(graphReference).Run(outputSucceed));
        }

        // Unity����o��FInishExternal�o�C���h
        if (outputExternal != null && outputExternal.hasAnyConnection)
        {
            task.OnFinishExternal.AddListener(() => Flow.New(graphReference).Run(outputExternal));
        }

        // �^�X�N���s�R���|�[�l���g�Ƀ^�X�N��o�^
        ownerComponent.RegisterTask(task);

        return outputDefault;
    }
}
