using UnityEngine;

public class CpDropItemData
{

}

// “G‚ð“|‚·‚Æ—Ž‚Æ‚·ƒAƒCƒeƒ€
public class CpDropItem : MonoBehaviour, ICpGameplayEffectSender
{
    public CpPlayerItemGameplayEffect PlayerItemGameplayEffect = null;

    // ICpGameplayEffectSender
    public CpGameplayEffectBase GetGameplayEffect()
    {
        return PlayerItemGameplayEffect.Effect;
    }

    // end of ICpGameplayEffectSender

    protected void OnTriggerEnter2D(Collider2D collision)
    {

        ICpGameplayEffectReciever geReciever = collision.GetComponent<ICpGameplayEffectReciever>();
        geReciever?.OnGameplayEffectRecieve(this);

        CpPlayer player = collision.GetComponent<CpPlayer>();
        if (player != null)
        {
            Destroy(this.gameObject);
        }
    }
}
