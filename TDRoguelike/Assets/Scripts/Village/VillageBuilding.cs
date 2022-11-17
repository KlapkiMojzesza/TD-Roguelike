using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VillageBuilding : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask buildingLayer;

    [Header("To Attach")]
    [SerializeField] TowerScriptableObject towerData;
    [SerializeField] GameObject buildingCanvas;
    [SerializeField] GameObject upgradeMenu;
    [SerializeField] GameObject upgradeButton;
    [SerializeField] GameObject upgradeButtonBlocked;
    [SerializeField] GameObject upgradeButtonPurchased;
    [Space(10)]
    [SerializeField] TMP_Text towerNameText;
    [SerializeField] TMP_Text towerInfoText;
    [SerializeField] TMP_Text towerStatsText;
    [SerializeField] RawImage towerIcon;
    [Space(10)]
    [SerializeField] TMP_Text upgradeNameText;
    [SerializeField] RawImage upgradeIcon;
    [SerializeField] TMP_Text upgradeInfoText;

    public List<UpgradeButton> upgradesPurchased;

    public static event Action<UpgradeButton> OnUpgradePurchased;

    UpgradeScriptableObject currentUpgradeData;
    UpgradeButton currentUpgradeButtonLogic;

    Controls controls;

    private void Start()
    {
        /*controls = new Controls();
        controls.Player.Enable();
        controls.Player.Info.performed += HandlePlayerMouseInfo;*/

        PlayerStructureInteract.OnPlayerInteract += ShowUI;
        UpgradeButton.OnUpgradeChoose += HandleUpgradeChoose;

        towerNameText.text = towerData.towerName;
        towerInfoText.text = towerData.towerInfo;
        towerIcon.texture = towerData.towerIcon;
        SetTowerStats();
    }

    private void OnDestroy()
    {
        //controls.Player.Info.performed -= HandlePlayerMouseInfo;
        PlayerStructureInteract.OnPlayerInteract -= ShowUI;
        UpgradeButton.OnUpgradeChoose -= HandleUpgradeChoose;
    }

    private void HandleUpgradeChoose(UpgradeScriptableObject upgradeData, UpgradeButton upgradeButtonLogic)
    {
        upgradeButton.SetActive(false);
        upgradeButtonBlocked.SetActive(false);
        upgradeButtonPurchased.SetActive(false);

        if (upgradesPurchased.Contains(upgradeButtonLogic))
        {
            upgradeButtonPurchased.SetActive(true);
        }
        else if (upgradeButtonLogic.UpgradePossible())
        {
            upgradeButton.SetActive(true);
        }
        else
        {
            upgradeButtonBlocked.SetActive(true);
        }

        currentUpgradeButtonLogic = upgradeButtonLogic;
        currentUpgradeData = upgradeData;

        upgradeNameText.text = upgradeData.upgradeName;
        upgradeInfoText.text = upgradeData.upgradeInfo;
        upgradeIcon.texture = upgradeData.upgradeIcon;
    }

    public void HandleUpgradeClicked()
    {
        if (!buildingCanvas.activeSelf) return;

        //this will be different with seve system
        if (upgradesPurchased.Contains(currentUpgradeButtonLogic)) return;

        upgradeMenu.SetActive(false);

        currentUpgradeButtonLogic.isPurchased = true;
        OnUpgradePurchased?.Invoke(currentUpgradeButtonLogic);
        upgradesPurchased.Add(currentUpgradeButtonLogic);

        UpgradeType upgradeType = currentUpgradeData.upgradeType;
        switch(upgradeType)
        {
            case UpgradeType.TowerDamage:
                towerData.towerDamage += currentUpgradeData.value;
                break;

            case UpgradeType.TowerRange:
                towerData.towerRange += currentUpgradeData.value;
                break;

            case UpgradeType.TowerFireRate:
                towerData.towerFireRate += currentUpgradeData.value;
                break;

            case UpgradeType.TowerEnemyPierce:
                towerData.towerEnemyPierce += (int)currentUpgradeData.value;
                break;

            case UpgradeType.TowerCustom:

                break;
        }

        SetTowerStats();
    }

    private void ShowUI(GameObject wantedBuilding)
    {
        if (wantedBuilding == this.gameObject)
        {
            buildingCanvas.SetActive(true);
            return;
        }
        buildingCanvas.SetActive(false);       
    }

    private void SetTowerStats()
    {
        towerStatsText.text = $"Damage: {towerData.towerDamage.ToString()}\n" +
                              $"FireRate: {towerData.towerFireRate.ToString()}\n" +
                              $"Range: {towerData.towerRange.ToString()}\n" +
                              $"Pierce: {towerData.towerEnemyPierce.ToString()}";
    }

    /*private void HandlePlayerMouseInfo(InputAction.CallbackContext cpntext)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit buildingHit;

        if (!Physics.Raycast(ray, out buildingHit, Mathf.Infinity, buildingLayer))
        {
            buildingCanvas.SetActive(false);
            return;
        }

        GameObject towerHitGameObject = buildingHit.transform.gameObject;
        if (towerHitGameObject != this.gameObject)
        {
            buildingCanvas.SetActive(false);
            return;
        }

        buildingCanvas.SetActive(true);
    }*/
}
