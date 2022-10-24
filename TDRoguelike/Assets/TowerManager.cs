using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private LayerMask groundLayer;

    private GameObject currentTowerPrefab;

    void Update()
    {
        //HandleNewObjectHotKey();

        if (currentTowerPrefab != null)
        {
            MoveTowerPrefab();

            if (Input.GetMouseButtonDown(1))
            {
                currentTowerPrefab = null;
            }
        }
    }

    private void HandleNewObjectHotKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchTowers(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchTowers(1);
        }
    }

    public void SwitchTowers(int towerIndex)
    {
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
