using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        PlayerBase.OnBaseDestroyed += HandleBaseDestoryed;
        WaveManager.OnWaveEnd += HandleWaveEnd;
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerPlaced -= HandleTowerDeselect;
        TowerManager.OnTowerDeselect -= HandleTowerDeselect;
        TowerManager.OnTowerSelect -= HandleTowerSelect;
        PlayerBase.OnBaseDestroyed -= HandleBaseDestoryed;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
    }

    private void HandleWaveEnd(int enmpy)
    {
        towersCanvas.SetActive(true);
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

    private void HandleBaseDestoryed()
    {
        towersCanvas.SetActive(false);
        placingCanvas.SetActive(false);
        levelEndCanvas.SetActive(true);
    }

    public void ReturnToVillageButton()
    {
        towersCanvas.SetActive(false);
        placingCanvas.SetActive(false);
        levelEndCanvas.SetActive(false);

        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        
    }
}
