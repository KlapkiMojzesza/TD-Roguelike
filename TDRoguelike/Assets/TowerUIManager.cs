using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUIManager : MonoBehaviour
{
    [SerializeField] GameObject towersCanvas;
    [SerializeField] GameObject placingCanvas;
    [SerializeField] GameObject levelEndCanvas;

    private void Start()
    {
        TowerManager.OnTowerPlaced += HandleTowerDeselect;
        TowerManager.OnTowerDeselect += HandleTowerDeselect;
        TowerManager.OnTowerSelect += HandleTowerSelect;
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerPlaced -= HandleTowerDeselect;
        TowerManager.OnTowerDeselect -= HandleTowerDeselect;
        TowerManager.OnTowerSelect -= HandleTowerSelect;
    }

    private void HandleTowerSelect()
    {
        towersCanvas.SetActive(false);
        placingCanvas.SetActive(true);
    }

    private void HandleTowerDeselect()
    {
        towersCanvas.SetActive(true);
        placingCanvas.SetActive(false);
    }
}
