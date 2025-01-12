using NUnit.Framework;
using UnityEngine;

public class CpDropItemPickupCollision : MonoBehaviour, ICpGameplayEffectReciever
{
    [SerializeField]
    CpPlayer _ownerPlayer = null;

    public CpGameplayEffectHandler GetGameplayEffectHandler()
    {
        if (_ownerPlayer == null)
        {
            Assert.IsTrue(false);
            return null;
        }

        return _ownerPlayer.GetGameplayEffectHandler();
    }
}
