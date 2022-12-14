using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalistaUpgradesInGame : MonoBehaviour, IUpgradeable
{
    private float _bonusTowerDamage = 0;
    private float _bonusTowerRange = 0;
    private float _bonusFireRate = 0;
    private int _bonusEnemyPierce = 0;


    /*
     * upgrade 1 
     * dmg 20
     * 
     * upgrade 2
     * range + visual
     * 
     * 
     * upgrade 3
     * firerate + dmg
     * 
     * upgrade 4...
     * 
     * */

    public void UpgradeButton()
    {
        //swittch(upgrades
        //get selected upgrade
        //boost stats from upgrade data
        //update upgrade visual
        //take money from player
    }


    public void UpgradeLeftOne(float boostAmount)
    {
        _bonusTowerDamage += boostAmount;
    }
    public void UpgradeLeftTwo(float boostAmount)
    {
        _bonusTowerDamage += boostAmount;
    }
    public void UpgradeLeftThree(float boostAmount)
    {
        _bonusTowerDamage += boostAmount;
    }

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
