using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradesInGame : TowerInGameUpgrades
{
    [SerializeField] private RightUpgrades[] _rightUpgradesStripes;
    [SerializeField] private LeftUpgrades[] _leftUpgrades;

    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    [System.Serializable]
    private class RightUpgrades
    {
        public GameObject[] UpgradeStripes;
    }

    [System.Serializable]
    private class LeftUpgrades
    {
        public GameObject UpgradeVisual;
        public AnimatorOverrideController visualOverride;
    }

    public override void UpgradeVisual(int leftUpgradesPurchased, int rightUpgradesPurchased)
    {          
        for (int i = 0; i < _leftUpgrades.Length ; i++)           
        {
            if (i == leftUpgradesPurchased)
            {
                _leftUpgrades[i].UpgradeVisual.SetActive(true);
                _animator.runtimeAnimatorController = _leftUpgrades[i].visualOverride;

                for (int j = 0; j < rightUpgradesPurchased; j++)
                {
                    _rightUpgradesStripes[i].UpgradeStripes[j].SetActive(true);
                }
            } 
            else _leftUpgrades[i].UpgradeVisual.SetActive(false);          
        }       

    }
}
