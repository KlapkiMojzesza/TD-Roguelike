using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [SerializeField] GameObject towersCanvas;
    [SerializeField] GameObject placingCanvas;
    [SerializeField] GameObject levelEndCanvas;
    [SerializeField] RawImage[] iconsImage;
    [SerializeField] TMP_Text[] towersPriceText;

    private void Start()
    {
        TowerManager.OnTowerPlaced += HandleTowerDeselect;
        TowerManager.OnTowerDeselect += HandleTowerDeselect;
        TowerManager.OnTowerSelect += HandleTowerSelect;
        PlayerBase.OnBaseDestroyed += HandleBaseDestoryed;
        WaveManager.OnWaveEnd += HandleWaveEnd;
       
        SetTowerUI();
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerPlaced -= HandleTowerDeselect;
        TowerManager.OnTowerDeselect -= HandleTowerDeselect;
        TowerManager.OnTowerSelect -= HandleTowerSelect;
        PlayerBase.OnBaseDestroyed -= HandleBaseDestoryed;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
    }

    private void SetTowerUI()
    {
        TowerManager towermanager = GetComponent<TowerManager>();
        for (int i = 0; i < towermanager.towerPrefabs.Length; i++)
        {
            Tower tower = towermanager.towerPrefabs[i].GetComponent<Tower>();
            iconsImage[i].texture = tower.towerData.towerIcon;
            towersPriceText[i].text = tower.towerData.towerPrice.ToString();
        }
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
