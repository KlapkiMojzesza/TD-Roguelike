using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private RawImage _upgradeIcon;
    [SerializeField] private GameObject _blockedIcon;
    [SerializeField] private GameObject _purchasedIcon;
    [SerializeField] private UpgradeScriptableObject _upgradeData;
    [SerializeField] private UpgradeButton _requiredUpgrade;

    public static event Action<UpgradeScriptableObject, UpgradeButton> OnUpgradeChoose;

    public bool IsPurchased = false;

    public bool UpgradePossible()
    {
        if (_requiredUpgrade == null) return true;
        if (_requiredUpgrade.IsPurchased) return true;

        return false;
    }

    private void Start()
    {
        VillageBuilding.OnUpgradePurchased += HandlePurchase;

        _upgradeIcon.texture = _upgradeData.UpgradeIcon;
    }

    private void OnDestroy()
    {
        VillageBuilding.OnUpgradePurchased -= HandlePurchase;
    }

    private void HandlePurchase(UpgradeButton upgradeButton)
    {
        if (upgradeButton == this) _purchasedIcon.SetActive(true);

        if (_requiredUpgrade == null) return;

        if (!_requiredUpgrade.IsPurchased) return;

        if (_blockedIcon == null) return;

        _blockedIcon.SetActive(false);
    }

    public void Upgrade()
    {
        OnUpgradeChoose?.Invoke(_upgradeData, this);
    }
}
