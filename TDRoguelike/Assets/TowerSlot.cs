using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TowerSlot : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _slotIndex = -1;
    [SerializeField] private int _unlockPrice = 50;

    [Header("To Attach")]
    [SerializeField] private RawImage _towerIconImage;
    [SerializeField] private Texture _slotUnlockedTexture;
    [SerializeField] private TMP_Text[] _slotUnlockPriceText;
    [SerializeField] private GameObject _buyButton;
    [SerializeField] private GameObject _buyButtonBlocked;
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
        _towerManager = (TowerManager)FindObjectOfType(typeof(TowerManager));
        TowerManager.OnMoneyAmountChanged += UpdateButtonStatus;
        TowerManager.OnTowerPlaced += HandleTowerPlaced;

        HideAllButtons();
        UpdateButtonStatus(_towerManager.GetCurrentMoneyAmount());
        _slotUnlockPriceText[0].text = _unlockPrice.ToString();
        _slotUnlockPriceText[1].text = _unlockPrice.ToString();
    }

    private void OnDestroy()
    {
        TowerManager.OnMoneyAmountChanged -= UpdateButtonStatus;
        TowerManager.OnTowerPlaced -= HandleTowerPlaced;
    }

    private void UpdateButtonStatus(int currentMoneyAmount)
    {
        if (_towerIsPlaced) return;

        if (_towerAssignedToSlot != null) return;

        if (_towerSlotUnlocked) return;

        HideAllButtons();

        if (currentMoneyAmount >= _unlockPrice)
        {
            _buyButton.SetActive(true);
            return;
        }

        _buyButtonBlocked.SetActive(true);
    }

    private void HandleTowerPlaced(Tower placedTower)
    {
        if (placedTower.gameObject != _towerAssignedToSlot) return;
        _towerIsPlaced = true;
        HideAllButtons();
        _placedTowerButtonBlocked.SetActive(true);
    }

    public void UnlockTowerButton()
    {
        if (_towerManager.GetCurrentMoneyAmount() < _unlockPrice) return;
        HideAllButtons();
        _towerSlotUnlocked = true;
        _selectTowerButton.SetActive(true);
        _towerIconImage.texture = _slotUnlockedTexture;
        OnSlotUnlockedButtonClicked?.Invoke(_unlockPrice);
    }

    public void PickTowerButton()
    {
        OnSelectTowerButtonClicked?.Invoke(this);
    }

    public void PickTower(GameObject towerSelectedByPlayer)
    {
        _towerIconImage.texture = towerSelectedByPlayer.GetComponent<Tower>().TowerData.TowerIcon;
        _towerAssignedToSlot = towerSelectedByPlayer;
        HideAllButtons();
        _placeTowerButton.SetActive(true);
    }

    public void PlaceTowerButton()
    {
        OnPlaceTowerButtonClicked?.Invoke(_towerAssignedToSlot);
    }

    public int GetSlotIndex()
    {
        return _slotIndex;
    }

    private void HideAllButtons()
    {
        _buyButton.SetActive(false);
        _buyButtonBlocked.SetActive(false);
        _selectTowerButton.SetActive(false);
        _placeTowerButton.SetActive(false);
        _placedTowerButtonBlocked.SetActive(false);
    }
}
