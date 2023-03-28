using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonTower : Tower
{
    private Animator _pylonAnimator;

    protected override void Awake()
    {
        base.Awake();
        TowerManager.OnTowerPlaced += HandleTOwerPlacement;
        _pylonAnimator = GetComponent<Animator>();
        _pylonAnimator.enabled = false;
    }

    //to prevent animator to change color before tower is placed
    override protected void OnDestroy()
    {
        base.OnDestroy();
        TowerManager.OnTowerPlaced -= HandleTOwerPlacement;
    }

    private void HandleTOwerPlacement(Tower tower)
    {
        _pylonAnimator.enabled = true;
        TowerManager.OnTowerPlaced -= HandleTOwerPlacement;
    }

    protected override void UpdateTowerStatsUI()
    {
        _towerStatsText.text = $"Damage: {TowerData.TowerDamage.ToString()}\n" +
                       $"Range: {TowerData.TowerRange.ToString()}\n" +
                       $"FireRate: {TowerData.TowerFireRate.ToString()}\n" +
                       $"Slow: {TowerData.TowerSlowPercentage.ToString()}";

        _towerNameText.text = TowerData.TowerName;
    }
}
