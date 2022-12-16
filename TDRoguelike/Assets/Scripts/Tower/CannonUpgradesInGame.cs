using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradesInGame : TowerInGameUpgrades
{



    public override void UpgradeVisual(int upgradePurchasedIndex, ChosenUpgradeSide upgradeSide)
    {
        Debug.Log("index: " + upgradePurchasedIndex + " side: " + upgradeSide);
    }
}
