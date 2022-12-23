using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "New Wave/Create Wave", order = 3)]
public class WaveScriptableObject : ScriptableObject
{
    public MiniWave[] MiniWaves;
    public int GoldForWaveCompleated = 10; 
}

[System.Serializable]
public class MiniWave
{
    public GameObject EnemyPrefab;
    public int Amount;
    public float SpawnRate;
    public float TimeAfterWave;
}
