using NUnit.Framework;
using UnityEngine;

// CpActorBase �̃A�j���[�V�����֘A
public partial class CpActorBase
{
    public void PlayAnimation(string animName, float speed = 1f)
    {
        if (_animator == null)
        {
            Assert.IsTrue(false, $"{name}�Ɋ֘A����Animator�����݂��܂���");
        }

        _animator.Play(animName);
        _animator.speed = speed;
    }

    public bool IsAnimationFinished(string animName)
    {
        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // �A�j���[�V������K�p����Ώۂ̃X�v���C�g�����_��
    // ��]�������킹�邽�߂ɃX�v���C�g�����q���I�u�W�F�N�g�Ƃ��Ĉ������Ƃ�����A
    // ���̎��͎q���I�u�W�F�N�g�ɑ΂��ăA�j���[�V�������Đ�����K�v������B
    // �C���^�[�t�F�[�X�𓝈ꂷ�邽�߂Ɏg��.
    [SerializeField]
    private SpriteRenderer _animatedSpriteRenderer = null;
}
