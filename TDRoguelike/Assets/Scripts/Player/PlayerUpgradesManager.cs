using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUpgradesManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject[] _iconsSelection;
    [SerializeField] private AudioClip[] _switchTypeSound;
    [SerializeField] private AudioClip _choseUpgradeSound;
    [SerializeField] private AudioClip _buyUpgradeSound;

    [Space(20)]
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _upgradeButtonBlank;
    [SerializeField] private GameObject _upgradeButtonBlocked;
    [SerializeField] private GameObject _upgradeButtonPurchased;
    [SerializeField] private TMP_Text _upgradesTypeNameText;
    [SerializeField] private TMP_Text _upgradeDescriptionText;

    public static event Action<PlayerUpgradeButton> OnUpgradePurchased;
    public static event Action<EnemyHealth> OnUpgradeMenuShow;
    public static event Action<EnemyHealth> OnUpgradeMenuHide;

    private AudioSource _audioSource;
    private PlayerUpgradeButton _currentUpgradeButtonLogic;
    private Controls _controls;

    //private doesnt work for some reason :/
    public List<PlayerUpgradeButton> _upgradesPurchased;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();

        _audioSource = GetComponent<AudioSource>();

        _controls.Player.UpgradeMenu.performed += ShowUpgradesMenu;
        PlayerUpgradeButton.OnUpgradeChoose += HandleUpgradeChoose;
    }

    private void OnDestroy()
    {
        _controls.Player.UpgradeMenu.performed -= ShowUpgradesMenu;
        PlayerUpgradeButton.OnUpgradeChoose -= HandleUpgradeChoose;
    }

    private void ShowUpgradesMenu(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
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
}
