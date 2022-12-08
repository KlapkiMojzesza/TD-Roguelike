using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "New Obstacle Type/Create Obstacle", order = 2)]
public class ObstacleScriptableObject : ScriptableObject
{
    public string ObstacleName = "Obstacle";
    public int RemovePrice = 10;
    public Texture ObstacleIcon;
}
