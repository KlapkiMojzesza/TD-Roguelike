using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] GameObject _pauseCanvas;
    [SerializeField] GameObject _optionsCanvas;
    [SerializeField] AudioMixer _audioMixer;

    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    private bool isPaused = false;
    private Controls _controls;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Pause.performed += HandlePause;
    }

    private void OnDestroy()
    {
        _controls.Player.Pause.performed -= HandlePause;
    }

    private void HandlePause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused) PauseGame();
        else ResumeGame();   
    }

    private void PauseGame()
    {
        OnGamePaused?.Invoke();
        _pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        OnGameResumed?.Invoke();
        _pauseCanvas.SetActive(false);
        _optionsCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    #region UI

    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSoundsVolume(float volume)
    {
        _audioMixer.SetFloat("SoundsVolume", volume);
    }

    public void ResumeGameButton()
    {
        OnGameResumed?.Invoke();
        _pauseCanvas.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void ShowOptionsButton()
    {
        _pauseCanvas.SetActive(false);
        _optionsCanvas.SetActive(true);
    }

    public void OptionsBackButton()
    {
        _optionsCanvas.SetActive(false);
        _pauseCanvas.SetActive(true);
    }

    public void ResetLevelButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
    #endregion
}
