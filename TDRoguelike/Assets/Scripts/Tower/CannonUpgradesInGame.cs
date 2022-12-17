using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonUpgradesInGame : TowerInGameUpgrades
{
    [SerializeField] private Stripes[] _rightUpgradesStripes;

    [System.Serializable]
    private class Stripes
    {
        public GameObject[] stripes;
    }

    [SerializeField] private GameObject[] _leftUpgradesGameObjects;
    [SerializeField] private GameObject[] _rightUpgradesGameObjects;

    public override void UpgradeVisual(int upgradePurchasedIndex, ChosenUpgradeSide upgradeSide)
    {
        if (upgradeSide == ChosenUpgradeSide.Left)
        {
            for (int i = 0; i < _leftUpgradesGameObjects.Length ; i++)
            {
                if (i == upgradePurchasedIndex + 1) _leftUpgradesGameObjects[i].SetActive(true);
                else _leftUpgradesGameObjects[i].SetActive(false);
            }
        }
        else if (upgradeSide == ChosenUpgradeSide.Right)
        {
            for (int i = 0; i < _rightUpgradesStripes.Length; i++)
            {
                //if ()
                for (int j = 0; j < _rightUpgradesStripes.Length; j++)
                {

                }
            }
        }

        Debug.Log("index: " + upgradePurchasedIndex + " side: " + upgradeSide);
    }
}
