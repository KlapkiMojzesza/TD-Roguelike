using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private WaveScriptableObject[] _waves;
    [SerializeField] private float _timeBeforeFirstWave = 2f;

    [Header("To Attach")]
    [SerializeField] private Transform _spawnPoint;

    public static event Action<int> OnWaveEnd;

    private List<GameObject> _aliveEnemies = new List<GameObject>();
    private int _currentWaveIndex = 0;
    private int _currentMiniWaveIndex = 0;
    private bool _waveCompleated = false;

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
        if (_currentWaveIndex >= _waves.Length) return;
        _waveCompleated = false;
        StartCoroutine(spawnWave(_timeBeforeFirstWave));
    }

    private IEnumerator spawnWave(float timeBeforeSpawn)
    {
        if (_currentMiniWaveIndex < _waves[_currentWaveIndex].MiniWaves.Length)
        { 
            yield return new WaitForSeconds(timeBeforeSpawn);
            MiniWave currentMiniWave = _waves[_currentWaveIndex].MiniWaves[_currentMiniWaveIndex];
            StartCoroutine(spawnMiniWave(currentMiniWave.SpawnRate, currentMiniWave));
            _currentMiniWaveIndex++;
        }
        else
        {
            //Debug.Log("end of big wave");
            _waveCompleated = true;
            _currentMiniWaveIndex = 0;
            _currentWaveIndex++;
        }
    }

    private IEnumerator spawnMiniWave(float spawnRate, MiniWave miniWave)
    {
        for (int i = 0; i < miniWave.Amount; i++)
        {
            yield return new WaitForSeconds(1f / miniWave.SpawnRate);
            SpawnEnemy(miniWave.EnemyPrefab);
        }
        //Debug.Log("end of miniWave");
        StartCoroutine(spawnWave(miniWave.TimeAfterWave));
    }

    private void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, _spawnPoint.position, Quaternion.identity);
        _aliveEnemies.Add(newEnemy);
        //Debug.Log(enemy.name);
    }

    private void HandleDeath(GameObject enemy)
    {
        if (_waveCompleated && _aliveEnemies.Count == 1)
        {
            //last enemy arrived
            _aliveEnemies.Remove(enemy);
            EndWave();
            return;
        }

        _aliveEnemies.Remove(enemy);
        if (!_waveCompleated) return;
        if (_aliveEnemies.Count != 0) return;
        //last enemy died from tower/player
        EndWave();
    }

    private void EndWave()
    {
        OnWaveEnd?.Invoke(_waves[_currentWaveIndex - 1].GoldForWaveCompleated);
    }

    private void HandleBaseDestruction()
    {
        //reset everything
        StopAllCoroutines();
        foreach (GameObject enemy in _aliveEnemies)
        {
            Destroy(enemy);
        }

        _aliveEnemies.Clear();

        _currentWaveIndex = 0;
        _currentMiniWaveIndex = 0;
        _waveCompleated = false;
    }
}
