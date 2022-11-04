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

    //remove serialize
    [SerializeField] List<GameObject> towersPlaced = new List<GameObject>();

    private int currentMoneyAmount;
    private GameObject currentTowerPrefab;
    private Tower currentTower;
    private bool mouseOverButton = true;

    private void Start()
    {
        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        WaveManager.OnWaveEnd += GiveMoney;
    }

    private void OnDestroy()
    {
        WaveManager.OnWaveEnd -= GiveMoney;
        PlayerBase.OnBaseDestroyed -= HandleBaseDestruction;
    }

    private void HandleBaseDestruction()
    {
        foreach (GameObject tower in towersPlaced)
        {
            Destroy(tower);
        }

        towersPlaced.Clear();

        currentMoneyAmount = startMoneyAmount;
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
    }

    void Update()
    {
        if (currentTowerPrefab == null) return;

        MoveTowerPrefab();

        if (Input.GetMouseButtonDown(0) && currentTower.CanBePlaced() && !mouseOverButton)
        {
            PlaceTower();
        }

    }

    private void PlaceTower()
    {
        currentMoneyAmount -= currentTower.GetTowerPrize();
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";

        currentTower.SetOrginalColor();

        currentTower.PlaceTower();
        towersPlaced.Add(currentTowerPrefab);
        currentTowerPrefab = null;
    }

    public void SwitchTowers(int towerIndex)
    {
        if (towerPrefabs[towerIndex].GetComponent<Tower>().GetTowerPrize() > currentMoneyAmount) return;

        if (currentTowerPrefab != towerPrefabs[towerIndex])
        {
            Destroy(currentTowerPrefab);
            currentTowerPrefab = Instantiate(towerPrefabs[towerIndex]);
            currentTower = currentTowerPrefab.GetComponent<Tower>();
        }
        else
        {
            Destroy(currentTowerPrefab);
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

    public void GiveMoney(int amount)
    {
        currentMoneyAmount += amount;
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
    }

    public void HideTowerPrefab()
    {
        Destroy(currentTowerPrefab);
    }

    public void mousceOverButtonEnter()
    {
        mouseOverButton = true;
    }

    public void mousceOverButtonExit()
    {
        mouseOverButton = false;
    }
}
