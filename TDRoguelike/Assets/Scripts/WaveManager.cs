using System;
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

    public static event Action<int> OnWaveEnd;

    List<GameObject> aliveEnemies = new List<GameObject>();
    int currentEnemySpawnIndex = 0;
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
        PlayerBase.OnBaseDestroyed += HandleBaseDestruction;
        EnemyHealth.OnEnemyDeath += HandleDeath;
        TowerManager.OnNextWaveButtonClicked += SpawnNextWave;
    }

    private void OnDestroy()
    {
        PlayerBase.OnBaseDestroyed -= HandleBaseDestruction;
        EnemyHealth.OnEnemyDeath -= HandleDeath;
        TowerManager.OnNextWaveButtonClicked -= SpawnNextWave;
    }

    public void SpawnNextWave()
    {
        if (currentWaveIndex >= waves.Length) return;
        waveCompleated = false;
        currentEnemySpawnIndex = 0;
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
        newEnemy.GetComponent<EnemyHealth>().enemyID = currentEnemySpawnIndex;
        aliveEnemies.Add(newEnemy);
        currentEnemySpawnIndex++;
        //Debug.Log(enemy.name);
    }

    private void HandleDeath(GameObject enemy)
    {
        if (waveCompleated && aliveEnemies.Count == 1)
        {
            //last enemy arrived
            aliveEnemies.Remove(enemy);
            EndWave();
            return;
        }

        aliveEnemies.Remove(enemy);
        if (!waveCompleated) return;
        if (aliveEnemies.Count != 0) return;
        //last enemy died from tower/player
        EndWave();
    }

    private void EndWave()
    {
        OnWaveEnd?.Invoke(waves[currentWaveIndex - 1].goldForWaveCompleated);
    }

    private void HandleBaseDestruction()
    {
        //reset everything
        StopAllCoroutines();
        foreach (GameObject enemy in aliveEnemies)
        {
            Destroy(enemy);
        }

        aliveEnemies.Clear();

        currentWaveIndex = 0;
        currentMiniWaveIndex = 0;
        waveCompleated = false;
    }
}
