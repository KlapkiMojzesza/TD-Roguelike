using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int currentMoneyAmount = 100;
    [SerializeField] TMP_Text moneyAmountText;

    private GameObject currentTowerPrefab;

    private void Start()
    {
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
    }

    void Update()
    {
        if (currentTowerPrefab != null)
        {
            MoveTowerPrefab();

            if (Input.GetMouseButtonDown(1) && currentTowerPrefab.GetComponent<Tower>().CanBePlaced())
            {
                PlaceTower();
            }
        }
    }

    private void PlaceTower()
    {
        currentMoneyAmount -= currentTowerPrefab.GetComponent<Tower>().GetTowerPrize();
        moneyAmountText.text = currentMoneyAmount.ToString() + "$";
        currentTowerPrefab = null;
    }

    public void SwitchTowers(int towerIndex)
    {
        if (towerPrefabs[towerIndex].GetComponent<Tower>().GetTowerPrize() > currentMoneyAmount) return;

        if (currentTowerPrefab != towerPrefabs[towerIndex])
        {
            Destroy(currentTowerPrefab);
            currentTowerPrefab = Instantiate(towerPrefabs[towerIndex]);
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
    }
}
