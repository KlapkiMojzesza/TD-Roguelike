using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] RawImage upgradeIcon;
    [SerializeField] GameObject blockedIcon;
    [SerializeField] GameObject purchasedIcon;
    [SerializeField] UpgradeScriptableObject upgradeData;
    [SerializeField] UpgradeButton requiredUpgrade;
    public Vector2Int buttonPosition = Vector2Int.zero;
    public bool isPurchased = false;

    public static event Action<UpgradeScriptableObject, UpgradeButton> OnUpgradeChoose;


    public bool UpgradePossible()
    {
        if (requiredUpgrade == null) return true;
        if (requiredUpgrade.isPurchased) return true;

        return false;
    }

    private void Start()
    {
        VillageBuilding.OnUpgradePurchased += HandlePurchase;

        upgradeIcon.texture = upgradeData.upgradeIcon;
    }

    private void OnDestroy()
    {
        VillageBuilding.OnUpgradePurchased -= HandlePurchase;
    }

    private void HandlePurchase(UpgradeButton upgradeButton)
    {
        if (upgradeButton == this) purchasedIcon.SetActive(true);

        if (requiredUpgrade == null) return;

        if (!requiredUpgrade.isPurchased) return;

        if (blockedIcon == null) return;

        blockedIcon.SetActive(false);
    }

    public void Upgrade()
    {
        OnUpgradeChoose?.Invoke(upgradeData, this);
    }
}
