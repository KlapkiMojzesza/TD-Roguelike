using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;
        WaveManager.OnMiniWaveStart += HandleStartWave;
    }

    private void OnDestroy()
    {
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
        WaveManager.OnMiniWaveStart -= HandleStartWave;
    }

    private void HandleStartWave(EnemyHealth enemy)
    {
        _audioSource.PlayOneShot(enemy.StartWaveSound);
    }

    private void HandleEnemyDeath(EnemyHealth enemy)
    {
        _audioSource.PlayOneShot(enemy.DeathSound);
    }

}
