using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int startMoneyAmount = 100;
    [SerializeField] private LayerMask groundLayer;

    [Header("To Attach")]
    [SerializeField] TMP_Text moneyAmountText;
    [SerializeField] private GameObject[] towerPrefabs;

    public static event Action OnTowerSelect;
    public static event Action OnTowerDeselect;
    public static event Action OnTowerPlaced;
    public static event Action OnNextWaveButtonClicked;

    private List<GameObject> towersPlaced = new List<GameObject>();
    private GameObject currentTowerPrefab;
    private Tower currentTower;
    private int currentMoneyAmount;

    private void Start()
    {
        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString();

        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    private void OnDestroy()
    {
        PlayerBase.OnBaseDestroyed -= HandleBaseDestruction;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
    }

    void Update()
    {
        if (currentTowerPrefab == null) return;

        MoveTowerPrefab();

        if (Input.GetMouseButtonUp(1))
        {
            Destroy(currentTowerPrefab);
            OnTowerDeselect?.Invoke();
            return;
        }

        if (Input.GetMouseButtonDown(0) && currentTower.CanBePlaced() && !IsMouseOverUI())
        {
            PlaceTower();
        }

    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private void MoveTowerPrefab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit groundHit;
        if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, groundLayer))
        {
            currentTowerPrefab.transform.position = groundHit.point;
        }
        currentTower.SetTowerColor();
    }

    private void PlaceTower()
    {
        currentMoneyAmount -= currentTower.GetTowerPrize();
        moneyAmountText.text = currentMoneyAmount.ToString();

        currentTower.SetOrginalColor();

        currentTower.PlaceTower();
        towersPlaced.Add(currentTowerPrefab);
        currentTowerPrefab = null;
        OnTowerDeselect?.Invoke();
        OnTowerPlaced?.Invoke();
    }

    public void SwitchTowers(int towerIndex)
    {
        if (towerPrefabs[towerIndex].GetComponent<Tower>().GetTowerPrize() > currentMoneyAmount) return;

        currentTowerPrefab = Instantiate(towerPrefabs[towerIndex]);
        currentTower = currentTowerPrefab.GetComponent<Tower>();
        OnTowerSelect?.Invoke();
    }

    public void HandleWaveEnd(int amount)
    {
        currentMoneyAmount += amount;
        moneyAmountText.text = currentMoneyAmount.ToString();
    }

    private void HandleBaseDestruction()
    {
        foreach (GameObject tower in towersPlaced)
        {
            Destroy(tower);
        }

        EndLevel();
    }

    private void EndLevel()
    {
        towersPlaced.Clear();
        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString();
    }

    public void SpawnNextWave()
    {
        OnNextWaveButtonClicked?.Invoke();
    }

    public void CancelButtonClick()
    {
        Destroy(currentTowerPrefab);
        OnTowerDeselect?.Invoke();
    }
}
