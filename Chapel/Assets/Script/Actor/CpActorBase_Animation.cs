using NUnit.Framework;
using UnityEngine;

// CpActorBase のアニメーション関連
public partial class CpActorBase
{
    public void PlayAnimation(string animName, float speed = 1f)
    {
        if (_animator == null)
        {
            Assert.IsTrue(false, $"{name}に関連するAnimatorが存在しません");
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

    // アニメーションを適用する対象のスプライトレンダラ
    // 回転軸を合わせるためにスプライトだけ子供オブジェクトとして扱うことがあり、
    // その時は子供オブジェクトに対してアニメーションを再生する必要がある。
    // インターフェースを統一するために使う.
    [SerializeField]
    private SpriteRenderer _animatedSpriteRenderer = null;
}
