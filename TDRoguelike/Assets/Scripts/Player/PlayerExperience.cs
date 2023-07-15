using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerExperience : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _requiredExperienceIncrement = 10;
    [SerializeField] private int _experienceToNextLevel = 100;

    [Header("To Attach")]
    [SerializeField] private Transform _healthbarCanvas;
    [SerializeField] private Image _experiencebarImage;
    [SerializeField] private TMP_Text _currentLevelText;

    public int _currentLevel = 1;
    public int _currentExperience = 0;

    public static event Action<int> OnLevelUp;

    private Camera _camera;
    private int _levelUpPoints = 0;

    void Start()
    {
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        EnemyHealth.OnEnemyDeath += HandleEnemyDeath;

        _camera = Camera.main;

        _experiencebarImage.fillAmount = _currentExperience / _experienceToNextLevel;
        _currentLevelText.text = _currentLevel.ToString();
    }

    private void OnDestroy()
    {
        if (PlayerHealth.PlayerInstance != this.gameObject) return;

        SceneManager.activeSceneChanged -= ActiveSceneChanged;
        EnemyHealth.OnEnemyDeath -= HandleEnemyDeath;
    }

    private void ActiveSceneChanged(Scene currentScene, Scene nextScene)
    {
        _camera = Camera.main;
    }

    private void HandleEnemyDeath(EnemyHealth enemy)
    {
        if (!enemy.DamagedByPlayer) return;

        _currentExperience += enemy._experienceDrop;
        if (_currentExperience >= _experienceToNextLevel)
        {
            _currentExperience -= _experienceToNextLevel;
            _currentLevel++;
            _levelUpPoints++;
            OnLevelUp?.Invoke(_currentLevel);
            _experienceToNextLevel += _requiredExperienceIncrement;
        }

        _experiencebarImage.fillAmount = (float)_currentExperience / (float)_experienceToNextLevel;
        _currentLevelText.text = _currentLevel.ToString();
    }

    private void Update()
    {
        RotateHealthbarToCamera();
    }

    private void RotateHealthbarToCamera()
    {
        Vector3 healthbarLookRotation = _healthbarCanvas.position - _camera.transform.position;
        healthbarLookRotation.x = 0;
        _healthbarCanvas.rotation = Quaternion.LookRotation(healthbarLookRotation);
    }

    public int GetLevelUpPoints()
    {
        return _levelUpPoints;
    }

}
