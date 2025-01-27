using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CpHudAmmo : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _ammoCount = null;

    private void Awake()
    {
        CpGamePlayManager.Get().OnStartPlayable.AddListener(OnPlayable);
    }

    protected void OnDestroy()
    {
        CpGamePlayManager gameplayManager = CpGamePlayManager.Get();
        gameplayManager?.OnStartPlayable.RemoveListener(OnPlayable);
    }


    public void OnPlayable()
    {
        CpShootComponent shootComponent = CpGameManager.Instance.Player.GetShootComponent();
        shootComponent.OnAmmoChanged.Subscribe(OnAmmoChanged);
    }

    void OnAmmoChanged(int ammo)
    {
        _ammoCount.text = ammo.ToString();
    }
}
