using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalistaUpgradesInGame : TowerInGameUpgrades
{


    public override void UpgradeVisual(int upgradePurchasedIndex, ChosenUpgradeSide upgradeSide)
    {
        Debug.Log("index: " + upgradePurchasedIndex + " side: " + upgradeSide);
    }
}
