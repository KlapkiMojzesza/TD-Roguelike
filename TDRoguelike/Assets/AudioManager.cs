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

        EnemyHealth.OnEnemyKilled += HandleEnemyDeath;
        WaveManager.OnMiniWaveStart += HandleStartWave;
        CannonProjectile.OnExplosion += PlayBoomSound;
    }

    private void OnDestroy()
    {
        EnemyHealth.OnEnemyKilled -= HandleEnemyDeath;
        WaveManager.OnMiniWaveStart -= HandleStartWave;
        CannonProjectile.OnExplosion -= PlayBoomSound;
    }
    private void PlayBoomSound(AudioClip boomSound)
    {
        _audioSource.PlayOneShot(boomSound);
    }

    private void HandleStartWave(EnemyHealth enemy)
    {
        _audioSource.PlayOneShot(enemy.StartWaveSound);
    }

    private void HandleEnemyDeath(EnemyHealth enemy)
    {
        _audioSource.PlayOneShot(enemy.HitSound);
        _audioSource.PlayOneShot(enemy.DeathSound);
    }

}
