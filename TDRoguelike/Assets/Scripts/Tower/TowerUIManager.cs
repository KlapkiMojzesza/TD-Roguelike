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
    [SerializeField] private GameObject _placingCanvas;
    [SerializeField] private GameObject _levelEndCanvas;
    [SerializeField] private GameObject _showTowerMenuButton;
    [SerializeField] private GameObject[] _towersBuyButtons;
    [SerializeField] private RawImage[] _iconsImage;
    [SerializeField] private TMP_Text[] _towersPriceText;
    [SerializeField] private Animator _towersCanvasAnimator;
    [SerializeField] private AudioClip _showUISound;
    [SerializeField] private AudioClip _hideUISound;
    [SerializeField] private AudioClip _startWaveSound;

    private TowerManager _towermanager;
    private AudioSource _audioSource;
    private bool _shouldShowOnResume = false;

    private void Start()
    {
        TowerManager.OnTowerPlaced += HandleTowerDeselect;
        TowerManager.OnTowerDeselect += HandleTowerDeselect;
        TowerManager.OnTowerSelect += HandleTowerSelect;
        TowerManager.OnMoneyAmountChanged += HandleMoneyAmountChanged;
        PlayerBase.OnBaseDestroyed += HandleBaseDestoryed;
        WaveManager.OnWaveEnd += HandleWaveEnd;
        PauseManager.OnGamePaused += OnPaused;
        PauseManager.OnGameResumed += OnResumed;

        _audioSource = GetComponent<AudioSource>();
        _towermanager = GetComponent<TowerManager>();

        SetTowerUI();
    }

    private void OnDestroy()
    {
        TowerManager.OnTowerPlaced -= HandleTowerDeselect;
        TowerManager.OnTowerDeselect -= HandleTowerDeselect;
        TowerManager.OnTowerSelect -= HandleTowerSelect;
        TowerManager.OnMoneyAmountChanged -= HandleMoneyAmountChanged;
        PlayerBase.OnBaseDestroyed -= HandleBaseDestoryed;
        WaveManager.OnWaveEnd -= HandleWaveEnd;
        PauseManager.OnGamePaused -= OnPaused;
        PauseManager.OnGameResumed -= OnResumed;
    }

    private void OnPaused()
    {
        if (_towersCanvasAnimator.GetBool("shown")) _shouldShowOnResume = true;
        this.gameObject.SetActive(false);
    }

    private void OnResumed()
    {
        this.gameObject.SetActive(true);

        if (_shouldShowOnResume)
        {
            _towersCanvasAnimator.SetBool("shown", true);
            _audioSource.PlayOneShot(_showUISound);
        }
        _shouldShowOnResume = false;
    }

    private void HandleMoneyAmountChanged(int newMoneyAmount)
    {
        for (int i = 0; i < _towermanager.TowerPrefabs.Length; i++)
        {
            Tower tower = _towermanager.TowerPrefabs[i].GetComponent<Tower>();
            if (newMoneyAmount < tower.TowerData.TowerPrice) _towersBuyButtons[i].SetActive(false);
            else _towersBuyButtons[i].SetActive(true);
        }
    }

    private void SetTowerUI()
    {
        for (int i = 0; i < _towermanager.TowerPrefabs.Length; i++)
        {
            Tower tower = _towermanager.TowerPrefabs[i].GetComponent<Tower>();
            _iconsImage[i].texture = tower.TowerData.TowerIcon;
            _towersPriceText[i].text = tower.TowerData.TowerPrice.ToString();
        }
        _towersCanvasAnimator.SetBool("shown", true);
    }

    private void HandleWaveEnd(int enmpy)
    {
        _showTowerMenuButton.SetActive(true);
        ShowTowersUI();
    }

    private void HandleTowerSelect(Tower selectedTower)
    {
        //another tower selected
        HideTowersUI();
        _placingCanvas.SetActive(true);
    }

    private void HandleTowerDeselect()
    {
        ShowTowersUI();
        _audioSource.PlayOneShot(_showUISound);
        _placingCanvas.SetActive(false);
    }

    private void HandleBaseDestoryed()
    {
        HideTowersUI();
        _placingCanvas.SetActive(false);
        _levelEndCanvas.SetActive(true);
    }

    public void ReturnToVillageButton()
    {
        HideTowersUI();
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

    public void NextWaveButton()
    {
        _audioSource.PlayOneShot(_startWaveSound);
        _showTowerMenuButton.SetActive(false);
        _towersCanvasAnimator.SetBool("shown", false);
    }

    public void ManageTowersUI()
    {
        if (_towersCanvasAnimator.GetBool("shown") == true)
        {
            _towersCanvasAnimator.SetBool("shown", false);
            _audioSource.PlayOneShot(_hideUISound);
        }
        else
        {
            _towersCanvasAnimator.SetBool("shown", true);
            _audioSource.PlayOneShot(_showUISound);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
