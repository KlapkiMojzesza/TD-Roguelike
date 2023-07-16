using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private Animator _optionsCanvasAnimator;
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private AudioClip _showOptionSound;
    [SerializeField] private AudioClip _hideOptionSound;

    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    private AudioSource _audioSource;
    private bool isPaused = false;
    private Controls _controls;
    private float _musicVolume;
    private float _soundVolume;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Pause.performed += HandlePause;

        _audioSource = GetComponent<AudioSource>();

        _audioMixer.GetFloat("MusicVolume", out _musicVolume);
        _audioMixer.GetFloat("SoundsVolume", out _soundVolume);

        _musicVolumeSlider.value = _musicVolume;
        _soundVolumeSlider.value = _soundVolume;
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
        if (_optionsCanvasAnimator.GetBool("shown")) _audioSource.PlayOneShot(_hideOptionSound);
        _optionsCanvasAnimator.SetBool("shown", false);
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
        _optionsCanvasAnimator.SetBool("shown", true);
        _audioSource.PlayOneShot(_showOptionSound);
    }

    public void OptionsBackButton()
    {
        _audioSource.PlayOneShot(_hideOptionSound);
        _optionsCanvasAnimator.SetBool("shown", false);
        _pauseCanvas.SetActive(true);
    }

    public void ResetLevelButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
    #endregion
}
