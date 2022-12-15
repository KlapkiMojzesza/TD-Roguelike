using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalistaUpgradesInGame : MonoBehaviour, IUpgradeable
{
    [Header("To Attach")]
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private TMP_Text _upgradeInfo;
    [SerializeField] private TMP_Text _towerBonusStatsText;
    [Header("To Attach: LEFT")]
    [SerializeField] UpgradeScriptableObject[] _towerUpgradesLeft;
    [SerializeField] private TMP_Text _upgradePriceLeft;
    [SerializeField] private RawImage _upgradeIconImageLeft;
    [Header("To Attach RIGHT")]
    [SerializeField] UpgradeScriptableObject[] _towerUpgradesRight;
    [SerializeField] private TMP_Text _upgradePriceRight;
    [SerializeField] private RawImage _upgradeIconImageRight;

    private TowerManager _towerManager;

    private float _bonusTowerDamage = 0;
    private float _bonusTowerRange = 0;
    private float _bonusFireRate = 0;
    private int _bonusEnemyPierce = 0;

    private int _leftUpgradesPurchased = 0;
    private int _rightUpgradesPurchased = 0;

    public enum ChosenUpgradeSide { Left = 0, Right = 1};
    private ChosenUpgradeSide _currentUpgradeChosen = ChosenUpgradeSide.Left;


    private void Start()
    {
        _towerManager = GameObject.FindGameObjectWithTag("TowerManager").GetComponent<TowerManager>();
        SetUpgradeInfo();
    }

    private void SetUpgradeInfo()
    {
        _upgradeIconImageLeft.texture = _towerUpgradesLeft[0].UpgradeIcon;
        _upgradePriceLeft.text = _towerUpgradesLeft[0].UpgradePrice.ToString();
        _upgradeInfo.text = _towerUpgradesLeft[0].UpgradeInfo;

        _upgradeIconImageRight.texture = _towerUpgradesRight[0].UpgradeIcon;
        _upgradePriceRight.text = _towerUpgradesRight[0].UpgradePrice.ToString();
    }

    public void SwitchUpgradeButton(int chosenUpgradeIndex)
    {
        _currentUpgradeChosen = (ChosenUpgradeSide)chosenUpgradeIndex;
        if (_currentUpgradeChosen == ChosenUpgradeSide.Left && _leftUpgradesPurchased < _towerUpgradesLeft.Length)
        {
            //this happends when upgrade exist
            _upgradeButton.SetActive(true);
            UpgradeScriptableObject upgradeData = _towerUpgradesLeft[_leftUpgradesPurchased];
            _upgradeInfo.text = upgradeData.UpgradeInfo;
            return;
        }
        else if (_currentUpgradeChosen == ChosenUpgradeSide.Right && _rightUpgradesPurchased < _towerUpgradesRight.Length)
        {
            //this happends when upgrade exist
            _upgradeButton.SetActive(true);
            UpgradeScriptableObject upgradeData = _towerUpgradesRight[_rightUpgradesPurchased];
            _upgradeInfo.text = upgradeData.UpgradeInfo;
            return;
        }
        _upgradeButton.SetActive(false);
        _upgradeInfo.text = "MAX LEVEL";
    }

    public void UpgradeButton()
    {
        //update upgrade visual
        //take money from player
        

        if (_currentUpgradeChosen == ChosenUpgradeSide.Left)
        {
            UpgradeScriptableObject currentLeftUpgrade =_towerUpgradesLeft[_leftUpgradesPurchased];

            if (_towerManager.GetCurrentMoneyAmount() < currentLeftUpgrade.UpgradePrice) return;

            _towerManager.BuyUpgrade(currentLeftUpgrade.UpgradePrice);
            UpgradeTower(currentLeftUpgrade);
            _leftUpgradesPurchased++;

            if (_leftUpgradesPurchased < _towerUpgradesLeft.Length)
            {
                //this happends when next upgrade exist
                currentLeftUpgrade = _towerUpgradesLeft[_leftUpgradesPurchased];
                _upgradeIconImageLeft.texture = currentLeftUpgrade.UpgradeIcon;
                _upgradePriceLeft.text = currentLeftUpgrade.UpgradePrice.ToString();
                _upgradeInfo.text = currentLeftUpgrade.UpgradeInfo;
                return;
            }
        }
        else if (_currentUpgradeChosen == ChosenUpgradeSide.Right)
        {
            UpgradeScriptableObject currentRightUpgrade = _towerUpgradesRight[_rightUpgradesPurchased];

            if (_towerManager.GetCurrentMoneyAmount() < currentRightUpgrade.UpgradePrice) return;

            _towerManager.BuyUpgrade(currentRightUpgrade.UpgradePrice);
            UpgradeTower(currentRightUpgrade);
            _rightUpgradesPurchased++;

            if (_rightUpgradesPurchased < _towerUpgradesRight.Length)
            {
                //this happends when next upgrade exist
                currentRightUpgrade = _towerUpgradesRight[_rightUpgradesPurchased];
                _upgradeIconImageRight.texture = currentRightUpgrade.UpgradeIcon;
                _upgradePriceRight.text = currentRightUpgrade.UpgradePrice.ToString();
                _upgradeInfo.text = currentRightUpgrade.UpgradeInfo;
                return;
            }
        }
        _upgradeButton.SetActive(false);
        _upgradeInfo.text = "MAX LEVEL";
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

        UpdateTowerStatsText();
    }

    private void UpdateTowerStatsText()
    {
        _towerBonusStatsText.text = $"+ {_bonusTowerDamage.ToString()}\n" +
                                    $"+ {_bonusTowerRange.ToString()}\n" +
                                    $"+ {_bonusFireRate.ToString()}\n" +
                                    $"+ {_bonusEnemyPierce.ToString()}";
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
