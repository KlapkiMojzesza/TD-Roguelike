using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "New Upgrade/Create Upgrade", order = 2)]
public class UpgradeScriptableObject : ScriptableObject
{
    public string UpgradeName = "Good Upgrade";
    public string UpgradeInfo = "Very Good Upgrade";
    public UpgradeType UpgradeType;
    public float Value = 1;
    public Texture UpgradeIcon;
}

public enum UpgradeType { TowerDamage, TowerRange, TowerFireRate, TowerEnemyPierce, TowerCustom }
