using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private bool mouseOverButton = true;

    private void Start()
    {
        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString();

        UIMouseHoverManager.OnMouseButtonEnter += mouseOverButtonEnter;
        UIMouseHoverManager.OnMouseButtonExit += mouseOverButtonExit;
        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    private void OnDestroy()
    {
        UIMouseHoverManager.OnMouseButtonEnter -= mouseOverButtonEnter;
        UIMouseHoverManager.OnMouseButtonExit -= mouseOverButtonExit;
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

        if (Input.GetMouseButtonDown(0) && currentTower.CanBePlaced() && !mouseOverButton)
        {
            PlaceTower();
        }

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
        mouseOverButton = false;

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
        mouseOverButton = false;
        OnNextWaveButtonClicked?.Invoke();
    }

    public void CancelButtonClick()
    {
        Destroy(currentTowerPrefab);
        OnTowerDeselect?.Invoke();
    }

    private void mouseOverButtonEnter()
    {
        mouseOverButton = true;
    }

    private void mouseOverButtonExit()
    {
        mouseOverButton = false;
    }
}
