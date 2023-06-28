using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWaveManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxGroupAmount = 4;
    [SerializeField] private int _enemySpawnRate = 3;
    [SerializeField] private float _timeBetweenWaves = 20f;

    [Header("To Attach")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject[] _possibleEnemies;

    private float _timer = 3;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _timeBetweenWaves) return;

        _timer = 0;
        StartCoroutine(spawnMiniWave());
    }

    private IEnumerator spawnMiniWave()
    {
        int randomEnemy = Random.Range(0, _possibleEnemies.Length);
        int randomEnemyAmount = Random.Range(1, _maxGroupAmount + 1);

        for (int i = 0; i < randomEnemyAmount; i++)
        {
            yield return new WaitForSeconds(1f / _enemySpawnRate);
            SpawnEnemy(_possibleEnemies[randomEnemy]);
        }

    }

    private void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, _spawnPoint.position, Quaternion.identity);
    }
}
