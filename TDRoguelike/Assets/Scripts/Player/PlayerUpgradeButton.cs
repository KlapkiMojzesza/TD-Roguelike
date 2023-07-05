using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeButton : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeScriptableObject _playerUpgradeData;
    [SerializeField] private PlayerUpgradeButton _requiredUpgrade;
    [SerializeField] private GameObject _blockedIcon;
    [SerializeField] private GameObject _purchasedIcon;
    [SerializeField] private GameObject[] _upgradePriceIcons;

    public static event Action<PlayerUpgradeButton, PlayerUpgradeScriptableObject> OnUpgradeChoose;

    public bool IsPurchased = false;

    private void Start()
    {
        PlayerUpgradesManager.OnUpgradePurchased += HandlePurchase;

        _purchasedIcon.SetActive(false);

        if (_requiredUpgrade != null) _blockedIcon.SetActive(true);

        SetUpgradePrice(_playerUpgradeData.UpgradePrice);
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
        OnUpgradeChoose?.Invoke(this, _playerUpgradeData);
    }

    private void HandlePurchase(PlayerUpgradeButton upgradeButton)
    {
        if (upgradeButton == this) _purchasedIcon.SetActive(true);

        if (_requiredUpgrade == null) return;

        if (!_requiredUpgrade.IsPurchased) return;

        if (_blockedIcon == null) return;

        _blockedIcon.SetActive(false);
    }

    private void SetUpgradePrice(int price)
    {
        for (int i = 0; i < price; i++)
        {
            _upgradePriceIcons[i].SetActive(true);
        }
    }
}
