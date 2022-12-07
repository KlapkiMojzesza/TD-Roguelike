using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _towersCanvas;
    [SerializeField] private GameObject _placingCanvas;
    [SerializeField] private GameObject _levelEndCanvas;
    [SerializeField] private RawImage[] _iconsImage;
    [SerializeField] private TMP_Text[] _towersPriceText;

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
        for (int i = 0; i < towermanager.TowerPrefabs.Length; i++)
        {
            Tower tower = towermanager.TowerPrefabs[i].GetComponent<Tower>();
            _iconsImage[i].texture = tower.TowerData.TowerIcon;
            _towersPriceText[i].text = tower.TowerData.TowerPrice.ToString();
        }
    }

    private void HandleWaveEnd(int enmpy)
    {
        _towersCanvas.SetActive(true);
    }

    private void HandleTowerSelect(Tower selectedTower)
    {
        _towersCanvas.SetActive(false);
        _placingCanvas.SetActive(true);
    }

    private void HandleTowerDeselect()
    {
        _towersCanvas.SetActive(true);
        _placingCanvas.SetActive(false);
    }

    private void HandleBaseDestoryed()
    {
        _towersCanvas.SetActive(false);
        _placingCanvas.SetActive(false);
        _levelEndCanvas.SetActive(true);
    }

    public void ReturnToVillageButton()
    {
        _towersCanvas.SetActive(false);
        _placingCanvas.SetActive(false);
        _levelEndCanvas.SetActive(false);

        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        
    }
}
