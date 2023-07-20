using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonUpgradesInGame : TowerInGameUpgrades
{
    [Space(20)]
    [SerializeField] private LeftUpgrades[] _leftUpgrades;
    [SerializeField] private RightUpgrades[] _rightUpgrades;
    [SerializeField] private Animator _crystalsAnimator;

    private TowerShooting _towerShooting;
    private Animator _animator;

    [System.Serializable]
    private class LeftUpgrades
    {
        public GameObject UpgradeVisual;
        public AnimatorOverrideController VisualOverride;
        public ParticleSystem ShootParticle;
    }

    [System.Serializable]
    private class RightUpgrades
    {
        public GameObject UpgradeVisual;
        public AnimatorOverrideController VisualOverride;
        public Projectile Projectile;
    }

    protected override void Start()
    {
        base.Start();

        _animator = GetComponent<Animator>();
        _towerShooting = GetComponent<TowerShooting>();
    }

    public override void UpgradeVisual(int leftUpgradesPurchased, int rightUpgradesPurchased)
    {
        base.UpgradeVisual(leftUpgradesPurchased, rightUpgradesPurchased);
        for (int i = 0; i < _leftUpgrades.Length; i++)
        {
            if (i == leftUpgradesPurchased)
            {
                _leftUpgrades[i].UpgradeVisual.SetActive(true);
                _animator.runtimeAnimatorController = _leftUpgrades[i].VisualOverride;
            }
            else _leftUpgrades[i].UpgradeVisual.SetActive(false);
        }

        for (int j = 0; j < _rightUpgrades.Length; j++)
        {
            if (j == rightUpgradesPurchased)
            {
                _rightUpgrades[j].UpgradeVisual.SetActive(true);              
            }
            else _rightUpgrades[j].UpgradeVisual.SetActive(false);
        }

        _crystalsAnimator.runtimeAnimatorController = _rightUpgrades[rightUpgradesPurchased].VisualOverride;
        _towerShooting.ChangeShootParticle(_leftUpgrades[leftUpgradesPurchased].ShootParticle);
        _towerShooting.ChangeProjectile(_rightUpgrades[rightUpgradesPurchased].Projectile, null);
    }

    protected override void UpdateTowerStatsText()
    {
        string bonusDamageText = "";
        string bonusRangeText = "";
        string bonusFireRateText = "";
        string _bonusSlowText = "";

        if (_bonusTowerDamage > 0) bonusDamageText = $"+ {_bonusTowerDamage.ToString()}";
        if (_bonusTowerRange > 0) bonusRangeText = $"+ {_bonusTowerRange.ToString()}";
        if (_bonusFireRate > 0) bonusFireRateText = $"+ {_bonusFireRate.ToString()}";
        if (_bonusSlowPercentage > 0) _bonusSlowText = $"+ {_bonusSlowPercentage.ToString()}";

        _towerBonusStatsText.text = $"{bonusDamageText}\n" +
                                    $"{bonusRangeText}\n" +
                                    $"{bonusFireRateText}\n" +
                                    $"{_bonusSlowText}";
    }
}
