using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject _towersCanvas;
    [SerializeField] private GameObject _towerSelectionCanvas;
    [SerializeField] private GameObject _playerMoneyCanvas;
    [SerializeField] private GameObject _placingCanvas;
    [SerializeField] private GameObject _levelEndCanvas;
    [SerializeField] private GameObject _showTowerMenuButton;
    [SerializeField] private GameObject[] _towerIconsSelected;
    [SerializeField] private TMP_Text _towerDescriptionText;

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


    private void HandleWaveEnd(int empty, bool isLastWave)
    {
        if (isLastWave) Debug.Log("Last Wave");
        _showTowerMenuButton.SetActive(true);
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
        _showTowerMenuButton.SetActive(false);
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
        SceneManager.LoadScene(0);
    }

    public void NextScene()
    {
        _towersCanvasAnimator.SetBool("shown", false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void HandleGameOver()
    {
        _placingCanvas.SetActive(false);
        _towersCanvas.SetActive(false);
        _towerSelectionCanvas.SetActive(false);

        _levelEndCanvas.SetActive(true);
    }
}
