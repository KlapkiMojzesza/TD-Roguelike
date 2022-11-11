using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "New Tower/Create Tower", order = 1)]
public class TowerScriptableObject : ScriptableObject
{
    public string towerName;
    public int towerPrice;
    public float towerFireRate = 1f;
    public float towerRange = 50f;
    public float towerDamage = 10f;
    public Texture towerIcon;
}

