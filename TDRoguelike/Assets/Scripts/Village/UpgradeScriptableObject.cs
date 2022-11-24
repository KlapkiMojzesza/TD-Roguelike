using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "New Upgrade/Create Upgrade", order = 2)]
public class UpgradeScriptableObject : ScriptableObject
{
    public string UpgradeName;
    public string UpgradeInfo;
    public UpgradeType UpgradeType;
    public float Value;
    public Texture UpgradeIcon;
}

public enum UpgradeType { TowerDamage, TowerRange, TowerFireRate, TowerEnemyPierce, TowerCustom }
