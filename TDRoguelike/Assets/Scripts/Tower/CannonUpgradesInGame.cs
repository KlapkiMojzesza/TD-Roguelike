using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradesInGame : TowerInGameUpgrades
{
    [Space(20)]
    [SerializeField] private LeftUpgrades[] _leftUpgrades;
    [SerializeField] private Projectile[] _rightUpgradesProjectiles;

    private TowerShooting _towerShooting;
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();

        _towerShooting = GetComponent<TowerShooting>();
    }

    [System.Serializable]
    private class LeftUpgrades
    {
        public GameObject UpgradeVisual;
        public AnimatorOverrideController VisualOverride;
        public ParticleSystem ShootParticle;
    }

    public override void UpgradeVisual(int leftUpgradesPurchased, int rightUpgradesPurchased)
    {
        base.UpgradeVisual(leftUpgradesPurchased, rightUpgradesPurchased);
        for (int i = 0; i < _leftUpgrades.Length ; i++)           
        {
            if (i == leftUpgradesPurchased)
            {
                _leftUpgrades[i].UpgradeVisual.SetActive(true);
                _animator.runtimeAnimatorController = _leftUpgrades[i].VisualOverride;             
            } 
            else _leftUpgrades[i].UpgradeVisual.SetActive(false);          
        }

        _towerShooting.ChangeProjectile(_rightUpgradesProjectiles[rightUpgradesPurchased], null);
        _towerShooting.ChangeShootParticle(_leftUpgrades[leftUpgradesPurchased].ShootParticle);
    }
}
