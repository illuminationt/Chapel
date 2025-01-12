using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public class CpGameplayEffectBase
{
}

[System.Serializable]
public class CpGEAddPlayerAmmo : CpGameplayEffectBase
{
    public int DeltaAmmo = 0;
}

[System.Serializable]
public class CpGEPlayerSpeed : CpGameplayEffectBase
{
    public float Speed = 200;
}

public class CpGameplayEffectHandler
{
    public UnityEvent<CpGameplayEffectBase> OnInvokeGameplayEffect => _onInvokeGameplayEffect;
    UnityEvent<CpGameplayEffectBase> _onInvokeGameplayEffect = new UnityEvent<CpGameplayEffectBase>();
}

[System.Serializable]
public class CpPlayerItemGameplayEffect
{
    [SerializeReference]
    public CpGameplayEffectBase Effect = null;
}