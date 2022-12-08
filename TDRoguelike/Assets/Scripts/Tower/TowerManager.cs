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

    public static event Action<Tower> OnTowerSelect;
    public static event Action OnTowerDeselect;
    public static event Action OnTowerPlaced;
    public static event Action OnNextWaveButtonClicked;

    private List<GameObject> _towersPlaced = new List<GameObject>();
    private GameObject _currentTowerPrefab;
    private Tower _currentTower;
    private int _currentMoneyAmount;

    private Controls _controls;

    private void Start()
    {
        _currentMoneyAmount = _startMoneyAmount;
        _moneyAmountText.text = _currentMoneyAmount.ToString();

        _controls = new Controls();
        _controls.Player.Enable();

        _controls.Player.Info.performed += HandlePlayerMouseInfo;
        _controls.Player.Shoot.performed += HandlePlayerMouseClick;
        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    private void OnDestroy()
    {
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
        _controls.Player.Shoot.performed -= HandlePlayerMouseClick;
        PlayerBase.OnBaseDestroyed -= HandleBaseDestruction;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
    }

    void Update()
    {
        if (_currentTowerPrefab == null) return;

        MoveTowerPrefab();
    }

    private void HandlePlayerMouseClick(InputAction.CallbackContext context)
    {
        if (IsMouseOverUI()) return;
        if (_currentTowerPrefab == null) return;
        if (!_currentTower.CanBePlaced()) return;

        PlaceTower();
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext context)
    {
        if (_currentTowerPrefab == null) return;
        Destroy(_currentTowerPrefab);
        OnTowerDeselect?.Invoke();
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
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

    private void PlaceTower()
    {
        _currentMoneyAmount -= _currentTower.TowerData.TowerPrice;
        _moneyAmountText.text = _currentMoneyAmount.ToString();

        _currentTower.SetOrginalColor();

        _currentTower.PlaceTower();
        _towersPlaced.Add(_currentTowerPrefab);
        _currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
        OnTowerPlaced?.Invoke();
    }

    public void SwitchTowers(int towerIndex)
    {
        if (TowerPrefabs[towerIndex].GetComponent<Tower>().TowerData.TowerPrice > _currentMoneyAmount) return;

        _currentTowerPrefab = Instantiate(TowerPrefabs[towerIndex]);
        _currentTower = _currentTowerPrefab.GetComponent<Tower>();
        OnTowerSelect?.Invoke(_currentTower);
    }

    public void HandleWaveEnd(int amount)
    {
        _currentMoneyAmount += amount;
        _moneyAmountText.text = _currentMoneyAmount.ToString();
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
    }

    public void SpawnNextWave()
    {
        OnNextWaveButtonClicked?.Invoke();
    }

    public void CancelButtonClick()
    {
        Destroy(_currentTowerPrefab);
        OnTowerDeselect?.Invoke();
    }
}
