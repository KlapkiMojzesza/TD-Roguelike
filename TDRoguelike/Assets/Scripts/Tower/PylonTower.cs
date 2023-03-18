using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonTower : Tower
{
    protected override void UpdateTowerStatsUI()
    {
        _towerStatsText.text = $"Damage: {TowerData.TowerDamage.ToString()}\n" +
                       $"Range: {TowerData.TowerRange.ToString()}\n" +
                       $"FireRate: {TowerData.TowerFireRate.ToString()}\n" +
                       $"Slow: {TowerData.TowerSlowPercentage.ToString()}";

        _towerNameText.text = TowerData.TowerName;
    }
}
