using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradesManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject[] _iconsSelection;
    [SerializeField] private AudioClip _switchTypeSound;

    [Space(20)]
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private GameObject _upgradeButtonBlank;
    [SerializeField] private GameObject _upgradeButtonBlocked;
    [SerializeField] private GameObject _upgradeButtonPurchased;

    public static event Action<PlayerUpgradeButton> OnUpgradePurchased;

    private AudioSource _audioSource;
    private PlayerUpgradeButton _currentUpgradeButtonLogic;
    public List<PlayerUpgradeButton> _upgradesPurchased;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        PlayerUpgradeButton.OnUpgradeChoose += HandleUpgradeChoose;
    }

    private void OnDestroy()
    {
        PlayerUpgradeButton.OnUpgradeChoose -= HandleUpgradeChoose;
    }

    public void UpgradeButton()
    {
        if (_upgradesPurchased.Contains(_currentUpgradeButtonLogic)) return;


        _currentUpgradeButtonLogic.IsPurchased = true;

        TurnOffAllUpgradeButons();
        _upgradeButtonPurchased.SetActive(true);

        _upgradesPurchased.Add(_currentUpgradeButtonLogic);
        OnUpgradePurchased?.Invoke(_currentUpgradeButtonLogic);

    }

    private void HandleUpgradeChoose(PlayerUpgradeButton upgradeButtonLogic)
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

        //_upgradeNameText.text = upgradeData.UpgradeName;
        //_upgradeInfoText.text = upgradeData.UpgradeInfo;
        //_upgradeIcon.texture = upgradeData.UpgradeIcon;
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

        _audioSource.PlayOneShot(_switchTypeSound);
    }
}
