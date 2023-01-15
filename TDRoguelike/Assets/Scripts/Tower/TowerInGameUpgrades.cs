using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class TowerInGameUpgrades : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private TMP_Text _upgradeInfo;
    [SerializeField] private TMP_Text _towerBonusStatsText;
    [SerializeField] private AudioClip _upgradeSound;
    [Header("To Attach: LEFT")]
    [SerializeField] UpgradeScriptableObject[] _towerUpgradesLeft;
    [SerializeField] private TMP_Text _upgradeNameLeft;
    [SerializeField] private TMP_Text _upgradePriceLeft;
    [SerializeField] private RawImage _upgradeIconImageLeft;
    [SerializeField] private Image _leftProgresImage;
    [Header("To Attach RIGHT")]
    [SerializeField] UpgradeScriptableObject[] _towerUpgradesRight;
    [SerializeField] private TMP_Text _upgradeNameRight;
    [SerializeField] private TMP_Text _upgradePriceRight;
    [SerializeField] private RawImage _upgradeIconImageRight;
    [SerializeField] private Image _rightProgresImage;

    UpgradeScriptableObject currentLeftUpgrade;
    UpgradeScriptableObject currentRightUpgrade;
    private AudioSource _audioSource;
    private TowerManager _towerManager;
    private Tower _tower;

    private float _bonusTowerDamage = 0;
    private float _bonusTowerRange = 0;
    private float _bonusFireRate = 0;
    private int _bonusEnemyPierce = 0;

    private int _leftUpgradesPurchased = 0;
    private int _rightUpgradesPurchased = 0;

    public enum ChosenUpgradeSide { Left = 0, Right = 1 };
    private ChosenUpgradeSide _currentUpgradeChosen = ChosenUpgradeSide.Left;


    protected virtual void Start()
    {
        _towerManager = GameObject.FindGameObjectWithTag("TowerManager").GetComponent<TowerManager>();
        _audioSource = GetComponent<AudioSource>();
        _tower = GetComponent<Tower>();

        SetUpgradeInfo(_currentUpgradeChosen);
        UpdateTowerStatsText();

        _leftProgresImage.fillAmount = _leftUpgradesPurchased / _towerUpgradesLeft.Length;
        _rightProgresImage.fillAmount = _rightUpgradesPurchased / _towerUpgradesRight.Length;

        Tower.OnTowerInfoShow += UpdateInfoCanvas;
        TowerManager.OnMoneyAmountChanged += CheckIfHaveEnoughMoney;
    }

    private void OnDestroy()
    {
        Tower.OnTowerInfoShow -= UpdateInfoCanvas;
        TowerManager.OnMoneyAmountChanged -= CheckIfHaveEnoughMoney;
    }

    private void CheckIfHaveEnoughMoney(int moneyAmount)
    {
        UpdateInfoCanvas();
    }

    private void UpdateInfoCanvas()
    {
        switch (_currentUpgradeChosen)
        {
            case ChosenUpgradeSide.Left:

                if (_leftUpgradesPurchased < _towerUpgradesLeft.Length)
                {
                    currentLeftUpgrade = _towerUpgradesLeft[_leftUpgradesPurchased];
                    if (_towerManager.GetCurrentMoneyAmount() < currentLeftUpgrade.UpgradePrice)
                    {
                        _upgradeButton.SetActive(false);
                        return;
                    }
                }
                else
                {
                    _upgradeButton.SetActive(false);
                    return;
                }

                break;

            case ChosenUpgradeSide.Right:

                if (_rightUpgradesPurchased < _towerUpgradesRight.Length)
                {
                    currentRightUpgrade = _towerUpgradesRight[_rightUpgradesPurchased];
                    if (_towerManager.GetCurrentMoneyAmount() < currentRightUpgrade.UpgradePrice)
                    {
                        _upgradeButton.SetActive(false);
                        return;
                    }
                } 
                else
                {
                    _upgradeButton.SetActive(false);
                    return;
                }

                break;
        }

        _upgradeButton.SetActive(true);       
    }

    private void SetUpgradeInfo(ChosenUpgradeSide chosenSide)
    {
        if (_leftUpgradesPurchased < _towerUpgradesLeft.Length)
        {
            _upgradeNameLeft.text = _towerUpgradesLeft[_leftUpgradesPurchased].UpgradeName;
            _upgradeIconImageLeft.texture = _towerUpgradesLeft[_leftUpgradesPurchased].UpgradeIcon;
            _upgradePriceLeft.text = _towerUpgradesLeft[_leftUpgradesPurchased].UpgradePrice.ToString();
        }
        if (_rightUpgradesPurchased < _towerUpgradesRight.Length)
        {
            _upgradeNameRight.text = _towerUpgradesRight[_rightUpgradesPurchased].UpgradeName;
            _upgradeIconImageRight.texture = _towerUpgradesRight[_rightUpgradesPurchased].UpgradeIcon;
            _upgradePriceRight.text = _towerUpgradesRight[_rightUpgradesPurchased].UpgradePrice.ToString();
        }

        if (chosenSide == ChosenUpgradeSide.Left) _upgradeInfo.text = _towerUpgradesLeft[_leftUpgradesPurchased].UpgradeInfo;
        if (chosenSide == ChosenUpgradeSide.Right) _upgradeInfo.text = _towerUpgradesRight[_rightUpgradesPurchased].UpgradeInfo;
    }

    public void SwitchUpgradeButton(int chosenUpgradeIndex)
    {
        _currentUpgradeChosen = (ChosenUpgradeSide)chosenUpgradeIndex;

        if (_currentUpgradeChosen == ChosenUpgradeSide.Left &&
            _leftUpgradesPurchased < _towerUpgradesLeft.Length)
        {
            //this happends when upgrade exist
            currentLeftUpgrade = _towerUpgradesLeft[_leftUpgradesPurchased];

            if (_towerManager.GetCurrentMoneyAmount() >= currentLeftUpgrade.UpgradePrice) _upgradeButton.SetActive(true);
            else _upgradeButton.SetActive(false);

            SetUpgradeInfo(ChosenUpgradeSide.Left);
            return;
        }
        else if (_currentUpgradeChosen == ChosenUpgradeSide.Right &&
            _rightUpgradesPurchased < _towerUpgradesRight.Length)
        {
            //this happends when upgrade exist
            currentRightUpgrade = _towerUpgradesRight[_rightUpgradesPurchased];

            if (_towerManager.GetCurrentMoneyAmount() >= currentRightUpgrade.UpgradePrice) _upgradeButton.SetActive(true);
            else _upgradeButton.SetActive(false);

            SetUpgradeInfo(ChosenUpgradeSide.Right);
            return;
        }
        //this happends when there are no upgrades left
        _upgradeButton.SetActive(false);
        _upgradeInfo.text = "MAX LEVEL";
    }

    public void UpgradeButton()
    {
        if (_currentUpgradeChosen == ChosenUpgradeSide.Left)
        {
            currentLeftUpgrade = _towerUpgradesLeft[_leftUpgradesPurchased];

            //if (_towerManager.GetCurrentMoneyAmount() < currentLeftUpgrade.UpgradePrice) return;

            _towerManager.BuyUpgrade(currentLeftUpgrade.UpgradePrice);
            UpgradeTower(currentLeftUpgrade);
            _leftUpgradesPurchased++;
            _leftProgresImage.fillAmount = (float)_leftUpgradesPurchased / (float)_towerUpgradesLeft.Length;
            UpgradeVisual(_leftUpgradesPurchased, _rightUpgradesPurchased);

            if (_leftUpgradesPurchased < _towerUpgradesLeft.Length)
            {
                //this happends when next upgrade exist
                currentLeftUpgrade = _towerUpgradesLeft[_leftUpgradesPurchased];
                SetUpgradeInfo(ChosenUpgradeSide.Left);
                if (_towerManager.GetCurrentMoneyAmount() < currentLeftUpgrade.UpgradePrice) _upgradeButton.SetActive(false);
                return;
            }
        }
        else if (_currentUpgradeChosen == ChosenUpgradeSide.Right)
        {
            currentRightUpgrade = _towerUpgradesRight[_rightUpgradesPurchased];

            //if (_towerManager.GetCurrentMoneyAmount() < currentRightUpgrade.UpgradePrice) return;

            _towerManager.BuyUpgrade(currentRightUpgrade.UpgradePrice);
            UpgradeTower(currentRightUpgrade);
            _rightUpgradesPurchased++;
            _rightProgresImage.fillAmount = (float)_rightUpgradesPurchased / (float)_towerUpgradesRight.Length;
            UpgradeVisual(_leftUpgradesPurchased, _rightUpgradesPurchased);

            if (_rightUpgradesPurchased < _towerUpgradesRight.Length)
            {
                //this happends when next upgrade exist
                currentRightUpgrade = _towerUpgradesRight[_rightUpgradesPurchased];
                SetUpgradeInfo(ChosenUpgradeSide.Right);
                if (_towerManager.GetCurrentMoneyAmount() < currentRightUpgrade.UpgradePrice) _upgradeButton.SetActive(false);
                return;
            }
        }
        _upgradeButton.SetActive(false);
        _upgradeInfo.text = "MAX LEVEL";
    }

    //for overriding
    public virtual void UpgradeVisual(int leftUpgradesPurchased, int rightUpgradesPurchased)
    {
        //do something different for each tower
    }

    private void UpgradeTower(UpgradeScriptableObject upgradeData)
    {
        UpgradeType upgradeType = upgradeData.UpgradeType;
        switch (upgradeType)
        {
            case UpgradeType.TowerDamage:
                _bonusTowerDamage += upgradeData.Value;
                break;

            case UpgradeType.TowerRange:
                _bonusTowerRange += upgradeData.Value;
                _tower.UpdateTowerRangeVisual(_bonusTowerRange);
                break;

            case UpgradeType.TowerFireRate:
                _bonusFireRate += upgradeData.Value;
                break;

            case UpgradeType.TowerEnemyPierce:
                _bonusEnemyPierce += (int)upgradeData.Value;
                break;

            case UpgradeType.TowerCustom:

                break;
        }

        _audioSource.PlayOneShot(_upgradeSound);
        UpdateTowerStatsText();
    }

    private void UpdateTowerStatsText()
    {
        string bonusDamageText = "";
        string bonusRangeText = "";
        string bonusFireRateText = "";
        string bonusPierceText = "";

        if (_bonusTowerDamage > 0) bonusDamageText = $"+ {_bonusTowerDamage.ToString()}";
        if (_bonusTowerRange > 0) bonusRangeText = $"+ {_bonusTowerRange.ToString()}";
        if (_bonusFireRate > 0) bonusFireRateText = $"+ {_bonusFireRate.ToString()}";
        if (_bonusEnemyPierce > 0) bonusPierceText = $"+ {_bonusEnemyPierce.ToString()}";

        _towerBonusStatsText.text = $"{bonusDamageText}\n" +
                                    $"{bonusRangeText}\n" +
                                    $"{bonusFireRateText}\n" +
                                    $"{bonusPierceText}";
    }

    //Getters for TowerShooting
    public float GetBonusDamage()
    {
        return _bonusTowerDamage;
    }

    public float GetBonusFireRate()
    {
        return _bonusFireRate;
    }

    public int GetBonusPierce()
    {
        return _bonusEnemyPierce;
    }

    public float GetBonusRange()
    {
        return _bonusTowerRange;
    }
}
