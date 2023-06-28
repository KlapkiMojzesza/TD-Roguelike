using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeButton : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeButton _requiredUpgrade;
    [SerializeField] private GameObject _blockedIcon;
    [SerializeField] private GameObject _purchasedIcon;

    public static event Action<PlayerUpgradeButton> OnUpgradeChoose;

    public bool IsPurchased = false;

    private void Start()
    {
        PlayerUpgradesManager.OnUpgradePurchased += HandlePurchase;

        _purchasedIcon.SetActive(false);

        if (_requiredUpgrade != null) _blockedIcon.SetActive(true);
    }

    private void OnDestroy()
    {
        PlayerUpgradesManager.OnUpgradePurchased -= HandlePurchase;
    }

    public bool UpgradePossible()
    {
        if (_requiredUpgrade == null) return true;
        if (_requiredUpgrade.IsPurchased) return true;

        return false;
    }

    public void ChooseUpgrade()
    {
        OnUpgradeChoose?.Invoke(this);
    }

    private void HandlePurchase(PlayerUpgradeButton upgradeButton)
    {
        if (upgradeButton == this) _purchasedIcon.SetActive(true);

        if (_requiredUpgrade == null) return;

        if (!_requiredUpgrade.IsPurchased) return;

        if (_blockedIcon == null) return;

        _blockedIcon.SetActive(false);
    }
}
