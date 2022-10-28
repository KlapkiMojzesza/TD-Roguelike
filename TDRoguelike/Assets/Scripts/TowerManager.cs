using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int currentMoneyAmount = 100;
    [SerializeField] private LayerMask groundLayer;

    [Header("To Attach")]
    [SerializeField] TMP_Text moneyAmountText;
    [SerializeField] private GameObject[] towerPrefabs;

    private GameObject currentTowerPrefab;
    private Tower currentTower;

    private void Start()
    {
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
    }

    void Update()
    {
        if (currentTowerPrefab != null)
        {
            MoveTowerPrefab();

            if (Input.GetMouseButtonDown(1) && currentTower.CanBePlaced())
            {
                PlaceTower();
            }
        }
    }

    private void PlaceTower()
    {
        currentMoneyAmount -= currentTower.GetTowerPrize();
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
        currentTower.SetOrginalColor();
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
}
