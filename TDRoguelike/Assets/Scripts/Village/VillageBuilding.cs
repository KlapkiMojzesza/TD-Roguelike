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
    [SerializeField] private LayerMask _buildingLayer;

    [Header("To Attach")]
    [SerializeField] private TowerScriptableObject _towerData;
    [SerializeField] private GameObject _buildingCanvas;
    [SerializeField] private GameObject _upgradeInfoMenu;
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _upgradeButtonBlocked;
    [SerializeField] private GameObject _upgradeButtonPurchased;
    [Space(10)]
    [SerializeField] private TMP_Text _towerNameText;
    [SerializeField] private TMP_Text _towerInfoText;
    [SerializeField] private TMP_Text _towerStatsText;
    [SerializeField] private RawImage _towerIcon;
    [Space(10)]
    [SerializeField] private TMP_Text _upgradeNameText;
    [SerializeField] private RawImage _upgradeIcon;
    [SerializeField] private TMP_Text _upgradeInfoText;

    public List<UpgradeButton> UpgradesPurchased;

    public static event Action<UpgradeButton> OnUpgradePurchased;

    private UpgradeScriptableObject _currentUpgradeData;
    private UpgradeButton _currentUpgradeButtonLogic;

    private Controls _controls;

    private void Start()
    {
        /*controls = new Controls();
        controls.Player.Enable();
        controls.Player.Info.performed += HandlePlayerMouseInfo;*/

        PlayerStructureInteract.OnPlayerInteract += ShowUI;
        UpgradeButton.OnUpgradeChoose += HandleUpgradeChoose;

        _towerNameText.text = _towerData.TowerName;
        _towerInfoText.text = _towerData.TowerInfo;
        _towerIcon.texture = _towerData.TowerIcon;
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
        _upgradeButton.SetActive(false);
        _upgradeButtonBlocked.SetActive(false);
        _upgradeButtonPurchased.SetActive(false);

        if (UpgradesPurchased.Contains(upgradeButtonLogic))
        {
            _upgradeButtonPurchased.SetActive(true);
        }
        else if (upgradeButtonLogic.UpgradePossible())
        {
            _upgradeButton.SetActive(true);
        }
        else
        {
            _upgradeButtonBlocked.SetActive(true);
        }

        _currentUpgradeButtonLogic = upgradeButtonLogic;
        _currentUpgradeData = upgradeData;

        _upgradeNameText.text = upgradeData.UpgradeName;
        _upgradeInfoText.text = upgradeData.UpgradeInfo;
        _upgradeIcon.texture = upgradeData.UpgradeIcon;
    }

    public void HandleUpgradeClicked()
    {
        if (!_buildingCanvas.activeSelf) return;

        //this will be different with seve system
        if (UpgradesPurchased.Contains(_currentUpgradeButtonLogic)) return;

        _upgradeInfoMenu.SetActive(false);

        _currentUpgradeButtonLogic.IsPurchased = true;
        OnUpgradePurchased?.Invoke(_currentUpgradeButtonLogic);
        UpgradesPurchased.Add(_currentUpgradeButtonLogic);

        UpgradeType upgradeType = _currentUpgradeData.UpgradeType;
        switch(upgradeType)
        {
            case UpgradeType.TowerDamage:
                _towerData.TowerDamage += _currentUpgradeData.Value;
                break;

            case UpgradeType.TowerRange:
                _towerData.TowerRange += _currentUpgradeData.Value;
                break;

            case UpgradeType.TowerFireRate:
                _towerData.TowerFireRate += _currentUpgradeData.Value;
                break;

            case UpgradeType.TowerEnemyPierce:
                _towerData.TowerEnemyPierce += (int)_currentUpgradeData.Value;
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
            _buildingCanvas.SetActive(true);
            return;
        }
        _buildingCanvas.SetActive(false);       
    }

    private void SetTowerStats()
    {
        _towerStatsText.text = $"Damage: {_towerData.TowerDamage.ToString()}\n" +
                              $"FireRate: {_towerData.TowerFireRate.ToString()}\n" +
                              $"Range: {_towerData.TowerRange.ToString()}\n" +
                              $"Pierce: {_towerData.TowerEnemyPierce.ToString()}";
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
