using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUpgradesManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject _playerUpgradesCanvas;
    [SerializeField] private GameObject[] _iconsSelection;
    [SerializeField] private AudioClip[] _switchTypeSound;
    [SerializeField] private AudioClip _choseUpgradeSound;
    [SerializeField] private AudioClip _buyUpgradeSound;
    [SerializeField] private AudioClip _showUISound;
    [SerializeField] private AudioClip _hideUISound;

    [Space(20)]
    [Header("To Attach UI")]
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _upgradeButtonBlank;
    [SerializeField] private GameObject _upgradeButtonBlocked;
    [SerializeField] private GameObject _upgradeButtonPurchased;
    [SerializeField] private TMP_Text _upgradesTypeNameText;
    [SerializeField] private TMP_Text _upgradeDescriptionText;
    [SerializeField] private TMP_Text _levelPointsAmountText;

    public static event Action<PlayerUpgradeButton> OnUpgradePurchased;
    public static event Action OnUpgradeMenuShow;
    public static event Action OnUpgradeMenuHide;

    private Animator _upgradeCanvasAnimator;
    private AudioSource _audioSource;
    private PlayerUpgradeButton _currentUpgradeButtonLogic;
    private Controls _controls;
    private int _currentLevelPointsAmount = 0;
    private bool _isShown = false;
    private bool gameIsPaused = false;

    //private doesnt work for some reason :/
    public List<PlayerUpgradeButton> _upgradesPurchased;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();

        _upgradeCanvasAnimator = _playerUpgradesCanvas.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _levelPointsAmountText.text = _currentLevelPointsAmount.ToString();

        _controls.Player.UpgradeMenu.performed += SwitchUpgradesMenuVisibility;
        PlayerUpgradeButton.OnUpgradeChoose += HandleUpgradeChoose;
        PlayerExperience.OnLevelUp += HandlePlayerLevelUp;
        PauseManager.OnGamePaused += HandleGamePause;
        PauseManager.OnGameResumed += HandleGameResume;
    }

    private void OnDestroy()
    {
        _controls.Player.UpgradeMenu.performed -= SwitchUpgradesMenuVisibility;
        PlayerUpgradeButton.OnUpgradeChoose -= HandleUpgradeChoose;
        PlayerExperience.OnLevelUp -= HandlePlayerLevelUp;
        PauseManager.OnGamePaused -= HandleGamePause;
        PauseManager.OnGameResumed -= HandleGameResume;
    }

    private void SwitchUpgradesMenuVisibility(InputAction.CallbackContext context)
    {
        if (gameIsPaused) return;

        if (_isShown)
        {
            _upgradeCanvasAnimator.SetBool("shown", false);
            _isShown = false;
            OnUpgradeMenuHide?.Invoke();
            _audioSource.PlayOneShot(_hideUISound);
            Time.timeScale = 1f;
        }
        else
        {
            _upgradeCanvasAnimator.SetBool("shown", true);
            _isShown = true;
            OnUpgradeMenuShow?.Invoke();
            _audioSource.PlayOneShot(_showUISound);
            Time.timeScale = 0f;
        }
    }

    private void HandleUpgradeChoose(PlayerUpgradeButton upgradeButtonLogic, PlayerUpgradeScriptableObject upgradeData)
    {
        TurnOffAllUpgradeButons();

        if (_upgradesPurchased.Contains(upgradeButtonLogic))
        {
            _upgradeButtonPurchased.SetActive(true);
        }
        else if (upgradeButtonLogic.UpgradePossible())
        {
            _upgradeButton.SetActive(true);
        }
        else
        {
            _upgradeButtonBlocked.SetActive(true);
        }

        _currentUpgradeButtonLogic = upgradeButtonLogic;
        _upgradeDescriptionText.text = upgradeData.UpgradeDescription;

        _audioSource.PlayOneShot(_choseUpgradeSound);
    }

    private void HandlePlayerLevelUp(int currentPlayerLevel)
    {
        _currentLevelPointsAmount++;
        _levelPointsAmountText.text = _currentLevelPointsAmount.ToString();
    }

    private void HandleGamePause()
    {
        gameIsPaused = true;
        _upgradeCanvasAnimator.SetBool("shown", false);
        _isShown = false;
        _playerUpgradesCanvas.SetActive(false);
    }

    private void HandleGameResume()
    {
        gameIsPaused = false;
        _playerUpgradesCanvas.SetActive(true);
    }

    public void UpgradeButton()
    {
        if (_upgradesPurchased.Contains(_currentUpgradeButtonLogic)) return;

        _currentUpgradeButtonLogic.IsPurchased = true;

        TurnOffAllUpgradeButons();
        _upgradeButtonPurchased.SetActive(true);
        _audioSource.PlayOneShot(_buyUpgradeSound);

        _upgradesPurchased.Add(_currentUpgradeButtonLogic);
        OnUpgradePurchased?.Invoke(_currentUpgradeButtonLogic);

    }

    private void TurnOffAllUpgradeButons()
    {
        _upgradeButtonBlank.SetActive(false);
        _upgradeButton.SetActive(false);
        _upgradeButtonBlocked.SetActive(false);
        _upgradeButtonPurchased.SetActive(false);
    }

    public void SwitchUpgradeType(int iconIndex)
    {
        for (int i = 0; i < _iconsSelection.Length; i++)
        {
            if (_iconsSelection[i] == _iconsSelection[iconIndex])
            {
                _iconsSelection[i].SetActive(true);
            }
            else
            {
                _iconsSelection[i].SetActive(false);
            }
        }

        TurnOffAllUpgradeButons();
        _upgradeButtonBlank.SetActive(true);

        _upgradeDescriptionText.text = "";

        _audioSource.PlayOneShot(_switchTypeSound[UnityEngine.Random.Range(0, _switchTypeSound.Length)]);
    }

    public void CloseButton()
    {
        _upgradeCanvasAnimator.SetBool("shown", false);
        _isShown = false;
        OnUpgradeMenuHide?.Invoke();
        _audioSource.PlayOneShot(_hideUISound);
        Time.timeScale = 1f;
    }
}
