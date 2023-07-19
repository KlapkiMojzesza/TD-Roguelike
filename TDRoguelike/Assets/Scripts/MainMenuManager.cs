using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    [Header("To Attach")]
    [SerializeField] private Animator _optionsCnavasAnimator;
    [SerializeField] private GameObject _startCanvas;
    [SerializeField] private GameObject _optionsCanvas;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioClip _showOptionsSound;
    [SerializeField] private AudioClip _hideOptionsSound;

    private AudioSource _audioSource;
    private LevelLoaderManager _levelLoader;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _levelLoader = (LevelLoaderManager)FindObjectOfType(typeof(LevelLoaderManager));

        _optionsCnavasAnimator.SetBool("shown", false);
        _startCanvas.SetActive(true);
    }

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

    public void PlayGameButton()
    {
        _levelLoader.LoadNextScene();
    }

    public void ShowOptionsButton()
    {
        _optionsCnavasAnimator.SetBool("shown", true);
        _startCanvas.SetActive(false);
        _audioSource.PlayOneShot(_showOptionsSound);
    }

    public void OptionsBackButton()
    {
        _optionsCnavasAnimator.SetBool("shown", false);
        _startCanvas.SetActive(true);
        _audioSource.PlayOneShot(_hideOptionsSound);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
