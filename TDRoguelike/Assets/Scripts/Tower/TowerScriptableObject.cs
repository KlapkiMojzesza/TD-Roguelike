using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "New Tower/Create Tower", order = 1)]
public class TowerScriptableObject : ScriptableObject
{
    public string towerName = "Tower";
    public string towerInfo = "Very good tower";
    public int towerPrice = 50;
    public int towerEnemyPierce = 1;
    public float towerFireRate = 1f;
    public float towerRange = 50f;
    public float towerDamage = 10f;
    public float projectileSpeed = 300f;
    public GameObject projectilePrefab;
    public Texture towerIcon;
}

