using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradeData", menuName = "New Player Upgrade/Create Upgrade", order = 4)]
public class PlayerUpgradeScriptableObject : ScriptableObject
{
    public string UpgradeDescription = "Very Good Upgrade";
    public Texture UpgradeIcon;
    public int UpgradePrice = 1;
}