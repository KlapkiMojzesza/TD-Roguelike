using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject _towersCanvas;
    [SerializeField] private GameObject _placingCanvas;
    [SerializeField] private GameObject _levelEndCanvas;
    [SerializeField] private RawImage[] _iconsImage;
    [SerializeField] private TMP_Text[] _towersPriceText;
    [SerializeField] private Animator _towersCanvasAnimator;

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
        //_towersCanvas.SetActive(true);
        ShowTowersUI();
    }

    private void HandleTowerSelect(Tower selectedTower)
    {
        HideTowersUI();
        //_towersCanvas.SetActive(false);
        _placingCanvas.SetActive(true);
    }

    private void HandleTowerDeselect()
    {
        //_towersCanvas.SetActive(true);
        ShowTowersUI();
        _placingCanvas.SetActive(false);
    }

    private void HandleBaseDestoryed()
    {
        HideTowersUI();
        //_towersCanvas.SetActive(false);
        _placingCanvas.SetActive(false);
        _levelEndCanvas.SetActive(true);
    }

    public void ReturnToVillageButton()
    {
        HideTowersUI();
        //_towersCanvas.SetActive(false);
        _placingCanvas.SetActive(false);
        _levelEndCanvas.SetActive(false);

        SceneManager.LoadScene(0);
    }

    private void ShowTowersUI()
    {
        _towersCanvasAnimator.SetBool("shown", true);
    }

    private void HideTowersUI()
    {
        _towersCanvasAnimator.SetBool("shown", false);
    }

    public void ManageTowersUI()
    {
        if (_towersCanvasAnimator.GetBool("shown") == true) _towersCanvasAnimator.SetBool("shown", false);
        else _towersCanvasAnimator.SetBool("shown", true);
    }

    public void ExitGame()
    {
        
    }
}
