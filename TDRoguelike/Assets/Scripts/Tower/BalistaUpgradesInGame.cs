using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalistaUpgradesInGame : TowerInGameUpgrades
{
    [Space(20)]
    [SerializeField] private LeftUpgrades[] _leftUpgrades;
    [SerializeField] private RightUpgrades[] _rightUpgrades;

    private TowerShooting _towerShooting;

    [System.Serializable]
    private class RightUpgrades
    {
        public Projectile[] TowerProjectile;
        public GameObject[] ProjectileVisual;
    }

    [System.Serializable]
    private class LeftUpgrades
    {
        public GameObject UpgradeVisual;
        public AnimatorOverrideController AnimatorOverride;
        public Transform FirePoint;
    }

    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _leftUpgrades[0].AnimatorOverride;
        _rightUpgrades[0].ProjectileVisual[0].SetActive(true);

        _towerShooting = GetComponent<TowerShooting>();
    }

    public override void UpgradeVisual(int leftUpgradesPurchased, int rightUpgradesPurchased)
    {
        for (int i = 0; i < _leftUpgrades.Length; i++)
        {
            if (i == leftUpgradesPurchased)
            {
                _leftUpgrades[i].UpgradeVisual.SetActive(true);
                _animator.runtimeAnimatorController = _leftUpgrades[i].AnimatorOverride;
                for (int j = 0; j < _rightUpgrades[i].ProjectileVisual.Length; j++)
                {
                    if (j == rightUpgradesPurchased)
                    {
                        _rightUpgrades[i].ProjectileVisual[j].SetActive(true);
                        _towerShooting.ChangeProjectile(_rightUpgrades[i].TowerProjectile[j], _leftUpgrades[i].FirePoint);
                       
                    }
                    else _rightUpgrades[i].ProjectileVisual[j].SetActive(false);
                }
               
            }
            else _leftUpgrades[i].UpgradeVisual.SetActive(false);
        }
    }

    
}
