using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "New Upgrade/Create Upgrade", order = 2)]
public class UpgradeScriptableObject : ScriptableObject
{
    public string upgradeName;
    public string upgradeInfo;
    public UpgradeType upgradeType;
    public float value;
    public Texture upgradeIcon;
}

public enum UpgradeType { TowerDamage, TowerRange, TowerFireRate, TowerEnemyPierce, TowerCustom }
