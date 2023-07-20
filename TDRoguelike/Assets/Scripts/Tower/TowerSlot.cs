using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerSlot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _slotIndex = -1;
    [SerializeField] private int _unlockPrice = 50;

    [Header("To Attach")]
    [SerializeField] private TowerSlot _requiredSlot;
    [SerializeField] private RawImage _towerIconImage;
    [SerializeField] private Texture _slotUnlockedTexture;
    [SerializeField] private TMP_Text[] _slotUnlockPriceText;
    [SerializeField] private GameObject _leftUpgradeLevelBackground;
    [SerializeField] private GameObject _rightUpgradeLevelBackground;
    [SerializeField] private TMP_Text _leftUpgradeLevelText;
    [SerializeField] private TMP_Text _rightUpgradeLevelText;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _buyButtonBlocked;
    [SerializeField] private GameObject _previousSlotBlockedButton;
    [SerializeField] private GameObject _selectTowerButton;
    [SerializeField] private GameObject _placeTowerButton;
    [SerializeField] private GameObject _placedTowerButtonBlocked;

    public static event Action<int> OnSlotUnlockedButtonClicked;
    public static event Action<TowerSlot> OnSelectTowerButtonClicked;
    public static event Action<GameObject> OnPlaceTowerButtonClicked;

    private TowerManager _towerManager;
    private GameObject _towerAssignedToSlot = null;

    bool _towerSlotUnlocked = false;
    bool _towerIsPlaced = false;


    private void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;

        _towerManager = (TowerManager)FindObjectOfType(typeof(TowerManager));
        TowerManager.OnMoneyAmountChanged += UpdateButtonStatus;
        TowerManager.OnTowerPlaced += HandleTowerPlaced;
        TowerSlot.OnSlotUnlockedButtonClicked += CheckIfRequiredSlotUnlocked;
        TowerInGameUpgrades.OnTowerUpgrade += HandleTowerUpgrade;

        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
        _slotUnlockPriceText[0].text = _unlockPrice.ToString();
        _slotUnlockPriceText[1].text = _unlockPrice.ToString();
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ActiveSceneChanged;
        TowerManager.OnMoneyAmountChanged -= UpdateButtonStatus;
        TowerManager.OnTowerPlaced -= HandleTowerPlaced;
        TowerSlot.OnSlotUnlockedButtonClicked -= CheckIfRequiredSlotUnlocked;
        TowerInGameUpgrades.OnTowerUpgrade -= HandleTowerUpgrade;
    }

    private void HandleTowerUpgrade(int leftUpgradeLevel, int rightUpgradeLevel, GameObject upgradedTower)
    {
        if (upgradedTower != _towerAssignedToSlot) return;

        _leftUpgradeLevelText.text = (leftUpgradeLevel + 1).ToString();
        _rightUpgradeLevelText.text = (rightUpgradeLevel + 1).ToString();
    }

    private void ActiveSceneChanged(Scene currentScene, Scene nextScene)
    {
        //if next scene is not menu
        _towerIsPlaced = false;
        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
    }

    private void HandleTowerPlaced(Tower placedTower)
    {
        if (placedTower.gameObject != _towerAssignedToSlot) return;
        _towerIsPlaced = true;
        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
    }

    private void CheckIfRequiredSlotUnlocked(int slotIndex)
    {
        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
    }

    public void UnlockTowerButton()
    {
        if (_towerManager.GetCurrentMoneyAmount() < _unlockPrice) return;
        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
        _towerSlotUnlocked = true;
        _towerIconImage.texture = _slotUnlockedTexture;
        OnSlotUnlockedButtonClicked?.Invoke(_unlockPrice);
    }

    public void PickTowerButton()
    {
        OnSelectTowerButtonClicked?.Invoke(this);
    }

    //called from TowerManager
    public void PickTower(GameObject towerSelectedByPlayer)
    {
        _towerIconImage.texture = towerSelectedByPlayer.GetComponent<Tower>().TowerData.TowerIcon;
        _towerAssignedToSlot = towerSelectedByPlayer;
        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
        _leftUpgradeLevelBackground.SetActive(true);
        _rightUpgradeLevelBackground.SetActive(true);
    }

    public void PlaceTowerButton()
    {
        OnPlaceTowerButtonClicked?.Invoke(_towerAssignedToSlot);
    }

    public int GetSlotIndex()
    {
        return _slotIndex;
    }

    private void UpdateButtonStatus(int currentMoneyAmount)
    {
        HideAllButtons();

        if (_towerIsPlaced)
        {
            _placedTowerButtonBlocked.SetActive(true);
            return;
        }

        if (_towerAssignedToSlot != null)
        {
            _placeTowerButton.SetActive(true);
            return;
        }

        if (_towerSlotUnlocked)
        {
            _selectTowerButton.SetActive(true);
            return;
        }

        if (_requiredSlot != null)
        {
            if (!_requiredSlot.IsUnlocked())
            {
                _previousSlotBlockedButton.SetActive(true);
                return;
            }
        }

        if (currentMoneyAmount >= _unlockPrice)
        {
            _buyButton.SetActive(true);
            return;
        }

        _buyButtonBlocked.SetActive(true);
    }

    private void HideAllButtons()
    {
        _buyButton.SetActive(false);
        _buyButtonBlocked.SetActive(false);
        _previousSlotBlockedButton.SetActive(false);
        _selectTowerButton.SetActive(false);
        _placeTowerButton.SetActive(false);
        _placedTowerButtonBlocked.SetActive(false);
    }

    public bool IsUnlocked()
    {
        return _towerSlotUnlocked;
    }    
}
