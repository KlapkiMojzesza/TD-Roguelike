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
    [SerializeField] GameObject buildingCanvas;
    [SerializeField] TowerScriptableObject towerData;
    [SerializeField] TMP_Text towerNameText;
    [SerializeField] TMP_Text towerInfoText;
    [SerializeField] TMP_Text towerStatsText;
    [SerializeField] RawImage towerIcon;
    [SerializeField] TMP_Text upgradeNameText;
    [SerializeField] RawImage upgradeIcon;
    [SerializeField] TMP_Text upgradeInfoText;

    public List<UpgradeScriptableObject> upgradesPurchased;

    UpgradeScriptableObject currentUpgradeData;

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
        towerStatsText.text = "TO BE DONE";
        towerIcon.texture = towerData.towerIcon;
    }

    private void OnDestroy()
    {
        //controls.Player.Info.performed -= HandlePlayerMouseInfo;
        PlayerStructureInteract.OnPlayerInteract -= ShowUI;
        UpgradeButton.OnUpgradeChoose -= HandleUpgradeChoose;
    }

    private void HandleUpgradeChoose(UpgradeScriptableObject upgradeData)
    {
        /*
         if (previous purchased)
         Upgrade button active
         return

        Upgrade button false

         
         */

        currentUpgradeData = upgradeData;
        upgradeNameText.text = upgradeData.upgradeName;
        upgradeInfoText.text = upgradeData.upgradeInfo;
        upgradeIcon.texture = upgradeData.upgradeIcon;
    }

    public void HandleUpgradeClicked()
    {
        if (!buildingCanvas.activeSelf) return;

        //this will be different with seve system
        if (upgradesPurchased.Contains(currentUpgradeData)) return;

        upgradesPurchased.Add(currentUpgradeData);

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
                towerData.towerPrice += (int)currentUpgradeData.value;
                break;

            case UpgradeType.TowerCustom:

                break;
        }
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
