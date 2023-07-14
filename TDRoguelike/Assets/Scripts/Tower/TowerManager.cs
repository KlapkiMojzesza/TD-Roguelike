using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _startMoneyAmount = 100;
    [SerializeField] private LayerMask _groundLayer;

    [Header("To Attach")]
    [SerializeField] private TMP_Text _moneyAmountText;
    [SerializeField] public GameObject[] TowerPrefabs;

    public static event Action<Tower> OnTowerSelectedToPlace;
    public static event Action OnTowerDeselect;
    public static event Action<Tower> OnTowerPlaced;
    public static event Action OnNextWaveButtonClicked;
    public static event Action<int> OnMoneyAmountChanged;
    public static event Action<int> OnTowerSelectionSwitch;

    private Dictionary<int, GameObject> _playerTowers = new Dictionary<int, GameObject>();
    private List<GameObject> _towersPlaced = new List<GameObject>();
    private GameObject _currentTowerPrefab;
    private GameObject _currentTowerSelected;
    private Tower _currentTower;
    private TowerSlot _currentTowerSlot = null;
    private int _currentMoneyAmount;

    private Controls _controls;


    private void Start()
    {
        _currentMoneyAmount = _startMoneyAmount;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
        OnMoneyAmountChanged?.Invoke(_currentMoneyAmount);

        _controls = new Controls();
        _controls.Player.Enable();

        _controls.Player.Info.performed += HandlePlayerMouseInfo;
        _controls.Player.Shoot.performed += HandlePlayerMouseClick;
        TowerSlot.OnSelectTowerButtonClicked += ChangeSlot;
        TowerSlot.OnPlaceTowerButtonClicked += HandleTowerPlacement;
        TowerSlot.OnSlotUnlockedButtonClicked += BuyTowerSlot;
        WaveManager.OnWaveEnd += HandleWaveEnd;
        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        PauseManager.OnGamePaused += CancelTowerSelected;
        PlayerUpgradesManager.OnUpgradeMenuShow += CancelTowerSelected;

        _currentTowerSelected = TowerPrefabs[0];
    }

    private void OnDestroy()
    {
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
        _controls.Player.Shoot.performed -= HandlePlayerMouseClick;
        TowerSlot.OnSelectTowerButtonClicked -= ChangeSlot;
        TowerSlot.OnPlaceTowerButtonClicked -= HandleTowerPlacement;
        TowerSlot.OnSlotUnlockedButtonClicked -= BuyTowerSlot;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
        PlayerBase.OnBaseDestroyed -= HandleBaseDestruction;
        PauseManager.OnGamePaused -= CancelTowerSelected;
        PlayerUpgradesManager.OnUpgradeMenuShow -= CancelTowerSelected;
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (_currentTowerPrefab == null) return;

        _currentTowerPrefab.SetActive(false);
        _currentTower.HideTower();
        _currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
    }

    private void HandlePlayerMouseClick(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (IsMouseOverUI()) return;
        if (_currentTowerPrefab == null) return;
        if (!_currentTower.CanBePlaced()) return;

        PlaceTower();
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId);
    }

    private void PlaceTower()
    {
        _currentTower.SetOrginalColor();

        _currentTower.PlaceTower();
        _towersPlaced.Add(_currentTowerPrefab);
        OnTowerPlaced?.Invoke(_currentTowerPrefab.GetComponent<Tower>());
        _currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
    }

    private void ChangeSlot(TowerSlot towerSlot)
    {
        _currentTowerSlot = towerSlot;
    }

    private void HandleTowerPlacement(GameObject towerToPlace)
    {
        _currentTowerPrefab = towerToPlace;
        _currentTower = _currentTowerPrefab.GetComponent<Tower>();
        _currentTowerPrefab.SetActive(true);
        OnTowerSelectedToPlace?.Invoke(_currentTower);
    }

    public void BuyTowerSlot(int towerSlotPrice)
    {
        _currentMoneyAmount -= towerSlotPrice;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
        OnMoneyAmountChanged?.Invoke(_currentMoneyAmount);
    }

    public void HandleWaveEnd(int amount)
    {
        _currentMoneyAmount += amount;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
        OnMoneyAmountChanged?.Invoke(_currentMoneyAmount);
    }

    private void HandleBaseDestruction()
    {
        foreach (GameObject tower in _towersPlaced)
        {
            Destroy(tower);
        }

        EndLevel();
    }

    private void EndLevel()
    {
        _towersPlaced.Clear();
        _currentMoneyAmount = _startMoneyAmount;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
        OnMoneyAmountChanged?.Invoke(_currentMoneyAmount);
    }

    private void CancelTowerSelected()
    {
        if (_currentTowerPrefab == null) return;

        _currentTowerPrefab.SetActive(false);
        _currentTower.HideTower();
        _currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
    }

    void Update()
    {
        if (_currentTowerPrefab == null) return;

        MoveTowerPrefab();
    }

    private void MoveTowerPrefab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit groundHit;
        if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, _groundLayer))
        {
            _currentTowerPrefab.transform.position = groundHit.point;
        }
        _currentTower.SetTowerColor();
    }

    public void SwitchTowerSelectedButton(int selectedTowerIndex)
    {
        _currentTowerSelected = TowerPrefabs[selectedTowerIndex];
        OnTowerSelectionSwitch?.Invoke(selectedTowerIndex);
    }

    public void ChooseTowerButton()
    {
        _currentTowerPrefab = Instantiate(_currentTowerSelected);
        _currentTower = _currentTowerPrefab.GetComponent<Tower>();
        OnTowerSelectedToPlace?.Invoke(_currentTower);
        _playerTowers.Add(_currentTowerSlot.GetSlotIndex(), _currentTowerPrefab);
        _currentTowerSlot.PickTower(_currentTowerPrefab);
    }

    public void SpawnNextWaveButton()
    {
        OnNextWaveButtonClicked?.Invoke();
    }

    public void CancelButtonClick()
    {
        if (_currentTowerPrefab == null) return;

        _currentTowerPrefab.SetActive(false);
        _currentTower.HideTower();
        _currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
    }

    public int GetCurrentMoneyAmount()
    {
        return _currentMoneyAmount;
    }

    public void RemoveObstacle(int removePrice)
    {
        _currentMoneyAmount -= removePrice;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
        OnMoneyAmountChanged?.Invoke(_currentMoneyAmount);
    }

    public void BuyUpgrade(int upgradePrice)
    {
        _currentMoneyAmount -= upgradePrice;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
        OnMoneyAmountChanged?.Invoke(_currentMoneyAmount);
    }
}
