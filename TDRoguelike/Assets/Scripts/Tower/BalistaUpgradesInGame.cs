using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalistaUpgradesInGame : TowerInGameUpgrades
{
    [SerializeField] private LeftUpgrades[] _leftUpgrades;
    //[SerializeField] private GameObject[]

    [System.Serializable]
    private class RightUpgrades
    {
        public GameObject[] TowerProjectile;
    }

    [System.Serializable]
    private class LeftUpgrades
    {
        public GameObject UpgradeVisual;
        public AnimatorOverrideController AnimatorOverride;
    }

    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = _leftUpgrades[0].AnimatorOverride;
    }

    public override void UpgradeVisual(int leftUpgradesPurchased, int rightUpgradesPurchased)
    {
        for (int i = 0; i < _leftUpgrades.Length; i++)
        {
            if (i == leftUpgradesPurchased)
            {
                _leftUpgrades[i].UpgradeVisual.SetActive(true);
                _animator.runtimeAnimatorController = _leftUpgrades[i].AnimatorOverride;
            }
            else _leftUpgrades[i].UpgradeVisual.SetActive(false);
        }
    }

    public void ShowProjectileModel()
    {

    }

    public void HideProjectileModel()
    {

    }
}
