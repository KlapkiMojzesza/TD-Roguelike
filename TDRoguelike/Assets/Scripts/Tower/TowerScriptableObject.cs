using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "New Tower/Create Tower", order = 1)]
public class TowerScriptableObject : ScriptableObject
{
    public string TowerName = "Tower";
    public string TowerInfo = "Very good tower";
    public int TowerPrice = 50;
    public int TowerEnemyPierce = 1;
    public float TowerFireRate = 1f;
    public float TowerRange = 50f;
    public float TowerDamage = 10f;
    public float ProjectileSpeed = 300f;
    public GameObject ProjectilePrefab;
    public Texture TowerIcon;
}

