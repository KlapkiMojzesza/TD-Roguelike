using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject _towersCanvas;
    [SerializeField] private GameObject _towerSelectionCanvas;
    [SerializeField] private GameObject _playerMoneyCanvas;
    [SerializeField] private GameObject _placingCanvas;
    [SerializeField] private GameObject _levelEndCanvas;
    [SerializeField] private GameObject _showMenuToggleButton;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _mapCompleteButton;
    [SerializeField] private GameObject[] _towerIconsSelected;
    [SerializeField] private TMP_Text _towerDescriptionText;
    [SerializeField] private TMP_Text _waveProgressText;

    [Header("Sounds")]
    [SerializeField] private AudioClip _showUISound;
    [SerializeField] private AudioClip _hideUISound;
    [SerializeField] private AudioClip _startWaveSound;
    [SerializeField] private AudioClip _unlockTowerSound;
    [SerializeField] private AudioClip _pickTowerSound;
    [SerializeField] private AudioClip _switchTowerSound;
    [SerializeField] private AudioClip _selectTowerToPlaceSound;

    public static Action OnTowerSelectionMenuShow;

    private Animator _towersCanvasAnimator;
    private Animator _towersSelectionCanvasAnimator;
    private AudioSource _audioSource;
    private bool _shouldShowOnResume = false;
    private Controls _controls;

    private void Start()
    {
        LevelLoaderManager.OnSceneLoaded += RefreshTowersMenu;

        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Info.performed += HandlePlayerMouseInfo;

        TowerManager.OnTowerPlaced += HandleTowerPlaced;
        TowerManager.OnTowerDeselect += HandleTowerDeselect;
        TowerManager.OnTowerSelectedToPlace += HandleTowerSelect;
        TowerManager.OnTowerSelectionSwitch += PlaySwitchSound;
        TowerSlot.OnSelectTowerButtonClicked += ShowAvailableTowersMenu;
        TowerSlot.OnSlotUnlockedButtonClicked += HandleSlotUnlock;

        WaveManager.OnWaveEnd += HandleWaveEnd;
        PauseManager.OnGamePaused += HideTowerMenu;
        PauseManager.OnGameResumed += ShowTowerMenu;

        PlayerUpgradesManager.OnUpgradeMenuShow += HideTowerMenu;
        PlayerUpgradesManager.OnUpgradeMenuHide += ShowTowerMenu;

        PlayerBase.OnBaseDestroyed += HandleGameOver;

        _audioSource = GetComponent<AudioSource>();

        _towersSelectionCanvasAnimator = _towerSelectionCanvas.GetComponent<Animator>();
        _towersCanvasAnimator = _towersCanvas.GetComponent<Animator>();
        _towersCanvasAnimator.SetBool("shown", true);
    }

    private void OnDestroy()
    {
        if (TowerManager.TowerManagerInstance != this.gameObject) return;

        LevelLoaderManager.OnSceneLoaded -= RefreshTowersMenu;

        _controls.Player.Info.performed -= HandlePlayerMouseInfo;

        TowerManager.OnTowerPlaced -= HandleTowerPlaced;
        TowerManager.OnTowerDeselect -= HandleTowerDeselect;
        TowerManager.OnTowerSelectedToPlace -= HandleTowerSelect;
        TowerManager.OnTowerSelectionSwitch -= PlaySwitchSound;
        TowerSlot.OnSelectTowerButtonClicked -= ShowAvailableTowersMenu;
        TowerSlot.OnSlotUnlockedButtonClicked -= HandleSlotUnlock;

        WaveManager.OnWaveEnd -= HandleWaveEnd;
        PauseManager.OnGamePaused -= HideTowerMenu;
        PauseManager.OnGameResumed -= ShowTowerMenu;

        PlayerUpgradesManager.OnUpgradeMenuShow -= HideTowerMenu;
        PlayerUpgradesManager.OnUpgradeMenuHide -= ShowTowerMenu;

        PlayerBase.OnBaseDestroyed -= HandleGameOver;
    }

    private void RefreshTowersMenu()
    {
        _towersCanvasAnimator.SetBool("shown", false);
        _startButton.SetActive(true);
        _waveProgressText.text = "1/10";
        _mapCompleteButton.SetActive(false);
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;

        if (EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId)) return;

        _towersSelectionCanvasAnimator.SetBool("shown", false);
    }

    private void HandleTowerPlaced(Tower tower)
    {
        HandleTowerDeselect();
    }

    private void HandleTowerDeselect()
    {
        ShowTowersUI();
        _audioSource.PlayOneShot(_showUISound);
        _placingCanvas.SetActive(false);
    }

    private void HandleTowerSelect(Tower selectedTower)
    {
        //another tower selected
        HideTowersUI();
        _audioSource.PlayOneShot(_selectTowerToPlaceSound);
        _placingCanvas.SetActive(true);
        _towersSelectionCanvasAnimator.SetBool("shown", false);
    }

    private void PlaySwitchSound(int selectedTowerIndex)
    {
        _audioSource.PlayOneShot(_switchTowerSound);

        for (int i = 0; i < _towerIconsSelected.Length; i++)
        {
            if (i == selectedTowerIndex)
            {
                _towerIconsSelected[i].SetActive(true);
            }
            else
            {
                _towerIconsSelected[i].SetActive(false);
            }
        }

        _towerDescriptionText.text = "Tower " + (selectedTowerIndex + 1) + " decription here";
    }

    private void ShowAvailableTowersMenu(TowerSlot towerSlot)
    {
        _towersCanvasAnimator.SetBool("shown", false);
        _towersSelectionCanvasAnimator.SetBool("shown", true);
        OnTowerSelectionMenuShow?.Invoke();
        _audioSource.PlayOneShot(_pickTowerSound);
    }

    private void HandleSlotUnlock(int ulnockPrice)
    {
        _audioSource.PlayOneShot(_unlockTowerSound);
    }


    private void HandleWaveEnd(int moneyForWave, int nextWaveIndex, bool isLastWave)
    {
        _waveProgressText.text = (nextWaveIndex + 1).ToString() + "/10";

        if (isLastWave)
        {
            _startButton.SetActive(false);
            _mapCompleteButton.SetActive(true);
        }
        _showMenuToggleButton.SetActive(true);
        ShowTowersUI();
    }

    private void HideTowerMenu()
    {
        if (_towersCanvasAnimator.GetBool("shown")) _shouldShowOnResume = true;

        _towersCanvas.SetActive(false);
        _towerSelectionCanvas.SetActive(false);
        _playerMoneyCanvas.SetActive(false);
    }

    private void ShowTowerMenu()
    {
        _towersCanvas.SetActive(true);
        _towerSelectionCanvas.SetActive(true);
        _playerMoneyCanvas.SetActive(true);

        if (_shouldShowOnResume)
        {
            _towersCanvasAnimator.SetBool("shown", true);
            _audioSource.PlayOneShot(_showUISound);
        }
        _shouldShowOnResume = false;
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
        _showMenuToggleButton.SetActive(false);
        _towersCanvasAnimator.SetBool("shown", false);
    }

    public void ManageTowersUIButton()
    {
        if (_towersCanvasAnimator.GetBool("shown") == true)
        {
            _towersCanvasAnimator.SetBool("shown", false);
            _audioSource.PlayOneShot(_hideUISound);
        }
        else
        {
            _towersSelectionCanvasAnimator.SetBool("shown", false);
            _towersCanvasAnimator.SetBool("shown", true);
            _audioSource.PlayOneShot(_showUISound);
        }
    }

    public void TowerSelectionCloseButton()
    {
        _towersSelectionCanvasAnimator.SetBool("shown", false);
        _towersCanvasAnimator.SetBool("shown", true);
        _audioSource.PlayOneShot(_showUISound);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void RestartLevelButton()
    {
        LevelLoaderManager _levelLoader = (LevelLoaderManager)FindObjectOfType(typeof(LevelLoaderManager));
        _levelLoader.LoadMainMenu();
    }

    private void HandleGameOver()
    {
        _placingCanvas.SetActive(false);
        _towersCanvas.SetActive(false);
        _towerSelectionCanvas.SetActive(false);

        _levelEndCanvas.SetActive(true);
    }
}
