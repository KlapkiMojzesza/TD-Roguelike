using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Wave[] waves;
    [SerializeField] float timeBeforeFirstWave = 2f;

    [Header("To Attach")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject waveEndCanvas;

    [SerializeField] List<GameObject> aliveEnemies = new List<GameObject>();

    int currentWaveIndex = 0;
    int currentMiniWaveIndex = 0;
    bool waveCompleated = false;

    [System.Serializable]
    public class Wave
    {
        public MiniWave[] miniWaves;
        public int goldForWaveCompleated = 10;
    }

    [System.Serializable]
    public class MiniWave
    {
        public GameObject enemyPrefab;
        public int amount;
        public float spawnRate;
        public float timeAfterWave;
    }

    private void Start()
    {
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
    }

    public void SpawnNextWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
            return;
        }

        waveEndCanvas.SetActive(false);
        waveCompleated = false;
        StartCoroutine(spawnWave(timeBeforeFirstWave));
    }

    private IEnumerator spawnWave(float timeBeforeSpawn)
    {
        if (currentMiniWaveIndex < waves[currentWaveIndex].miniWaves.Length)
        { 
            yield return new WaitForSeconds(timeBeforeSpawn);
            MiniWave currentMiniWave = waves[currentWaveIndex].miniWaves[currentMiniWaveIndex];
            StartCoroutine(spawnMiniWave(currentMiniWave.spawnRate, currentMiniWave));
            currentMiniWaveIndex++;
        }
        else
        {
            //Debug.Log("end of big wave");
            waveCompleated = true;
            currentMiniWaveIndex = 0;
            currentWaveIndex++;
        }
    }

    private IEnumerator spawnMiniWave(float spawnRate, MiniWave miniWave)
    {
        for (int i = 0; i < miniWave.amount; i++)
        {
            yield return new WaitForSeconds(1f / miniWave.spawnRate);
            SpawnEnemy(miniWave.enemyPrefab);
        }
        //Debug.Log("end of miniWave");
        StartCoroutine(spawnWave(miniWave.timeAfterWave));
    }

    private void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        aliveEnemies.Add(newEnemy);
        //Debug.Log(enemy.name);
    }

    private void HandleEnemyDeath(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
        if (!waveCompleated) return;
        if (aliveEnemies.Count != 0) return;

        //move somewhere GiveMoney() somewhere else later
        GameObject.FindGameObjectWithTag("TowerManager").GetComponent<TowerManager>()
                  .GiveMoney(waves[currentWaveIndex - 1].goldForWaveCompleated);

        waveEndCanvas.SetActive(true);
    }

}
