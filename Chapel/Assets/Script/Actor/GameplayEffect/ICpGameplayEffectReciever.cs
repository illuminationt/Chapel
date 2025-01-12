using UnityEngine;
using UnityEngine.Events;

public interface ICpGameplayEffectReciever
{
    public CpGameplayEffectHandler GetGameplayEffectHandler();
    public void OnGameplayEffectRecieve(ICpGameplayEffectSender sender)
    {
        OnGameplayEffectInvoked.Invoke(sender.GetGameplayEffect());
    }
    public UnityEvent<CpGameplayEffectBase> OnGameplayEffectInvoked
    {
        get
        {
            return GetGameplayEffectHandler().OnInvokeGameplayEffect;
        }
    }
}

