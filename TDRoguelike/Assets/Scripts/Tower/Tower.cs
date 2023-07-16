using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _towerLayer;
    [SerializeField] private Color _canBePlacedColor = new Color(0.003921569f, 0.5568628f, 0.09411765f); //nice looking green
    [SerializeField] private Color _canNotBePlacedColor = new Color(0.7019608f, 0.05098039f, 0.05098039f); //nice looking red

    [Header("To Attach")]
    public TowerScriptableObject TowerData;
    [SerializeField] private Transform _towerRangeSprite;
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private RawImage _iconImage;
    [SerializeField] private GameObject _towerHitBox;
    [SerializeField] private GameObject _towerInfoCanvas;
    [SerializeField] protected TMP_Text _towerStatsText;
    [SerializeField] protected TMP_Text _towerNameText;
    [SerializeField] private AudioClip _showUISound;
    [SerializeField] private AudioClip _hideUISound;
    [SerializeField] private AudioClip _towerPlaceSound; 

    public static event Action OnTowerInfoShow;

    protected AudioSource _audioSource;
    private int _collisionsAmount = 0;
    private bool _isPlaced = false;

    private Animator _canvasAnimator;
    private Controls _controls;
    private Dictionary<Material, Color> _allObjects = new Dictionary<Material, Color>();

    protected virtual void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Info.performed += HandlePlayerMouseInfo;

        SetAllBuildingMaterials();

        _iconImage.texture = TowerData.TowerIcon;
        _towerRangeSprite.localScale = new Vector3(TowerData.TowerRange, TowerData.TowerRange, 1f);

        _audioSource = GetComponent<AudioSource>();
        _canvasAnimator = _towerInfoCanvas.GetComponent<Animator>();

        TowerManager.OnNextWaveButtonClicked += HandleStartWave;
        TowerManager.OnTowerSelectedToPlace += HandleAnotherTowerSelected;
        TowerUIManager.OnTowerSelectionMenuShow += HideInfoUI;
        PauseManager.OnGamePaused += HideTowerAndDeactiveUpgradesMenu;
        PauseManager.OnGameResumed += ShowAndActiveTowerUpgradesMenu;
        PlayerUpgradesManager.OnUpgradeMenuShow += HideTowerAndDeactiveUpgradesMenu;
        PlayerUpgradesManager.OnUpgradeMenuHide += ShowAndActiveTowerUpgradesMenu;
        PlayerBase.OnBaseDestroyed += HandleGameOver;
        PlayerHealth.OnPlayerDeath += HandleGameOver;
    }

    protected virtual void OnDestroy()
    {
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
        TowerManager.OnNextWaveButtonClicked -= HandleStartWave;
        TowerManager.OnTowerSelectedToPlace -= HandleAnotherTowerSelected;
        TowerUIManager.OnTowerSelectionMenuShow -= HideInfoUI;
        PauseManager.OnGamePaused -= HideTowerAndDeactiveUpgradesMenu;
        PauseManager.OnGameResumed -= ShowAndActiveTowerUpgradesMenu;
        PlayerUpgradesManager.OnUpgradeMenuShow -= HideTowerAndDeactiveUpgradesMenu;
        PlayerUpgradesManager.OnUpgradeMenuHide -= ShowAndActiveTowerUpgradesMenu;
        PlayerBase.OnBaseDestroyed -= HandleGameOver;
        PlayerHealth.OnPlayerDeath -= HandleGameOver;
    }

    private void HideTowerAndDeactiveUpgradesMenu()
    {
        if (!_canvasAnimator.GetBool("shown")) return;

        _towerInfoCanvas.SetActive(false);
        _towerRangeSprite.gameObject.SetActive(false);
    }

    private void ShowAndActiveTowerUpgradesMenu()
    {
        if (!_towerInfoCanvas.activeSelf)
        {
            _towerInfoCanvas.SetActive(true);
            _canvasAnimator.SetBool("shown", true);
            _audioSource.PlayOneShot(_showUISound);
            _towerRangeSprite.gameObject.SetActive(true);
        }
    }

    public void SetAllBuildingMaterials()
    {
        foreach (Renderer render in _renderers)
        {
            Material[] materials = render.materials;
            foreach (Material material in materials)
            {
                if (!_allObjects.ContainsKey(material))
                {
                    _allObjects.Add(material, material.color);
                }
            }
        }
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0) return;
        if (!_isPlaced) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit towerHit;

        if (!Physics.Raycast(ray, out towerHit, Mathf.Infinity, _towerLayer))
        {
            HideInfoUI();        
            return;
        }

        GameObject towerHitGameObject = towerHit.transform.gameObject;
        if (towerHitGameObject != this.gameObject)
        {
            _canvasAnimator.SetBool("shown", false);
            _towerRangeSprite.gameObject.SetActive(false);
            return;
        }

        _towerRangeSprite.gameObject.SetActive(true);

        if (_canvasAnimator.GetBool("shown")) return;
        _canvasAnimator.SetBool("shown", true);
        _audioSource.PlayOneShot(_showUISound);
        OnTowerInfoShow?.Invoke();
    }

    private void HandleStartWave()
    {
        HideInfoUI();
        _towerRangeSprite.gameObject.SetActive(false);
    }

    //another tower button was clicked
    private void HandleAnotherTowerSelected(Tower selectedTower)
    {
        if (selectedTower != this)
        {
            HideInfoUI();
            return;
        }
    }

    public bool CanBePlaced()
    {
        if (_collisionsAmount == 0) return true;

        return false;
    }

    public void PlaceTower()
    {
        _towerHitBox.SetActive(true);
        _isPlaced = true;
        UpdateTowerStatsUI();

        _towerInfoCanvas.SetActive(true);
        _canvasAnimator.SetBool("shown", true);
        _audioSource.PlayOneShot(_towerPlaceSound);
        OnTowerInfoShow?.Invoke();
    }

    protected virtual void UpdateTowerStatsUI()
    {
        _towerStatsText.text = $"Damage: {TowerData.TowerDamage.ToString()}\n" +
                               $"Range: {TowerData.TowerRange.ToString()}\n" +
                               $"FireRate: {TowerData.TowerFireRate.ToString()}\n" +
                               $"Pierce: {TowerData.TowerEnemyPierce.ToString()}";

        _towerNameText.text = TowerData.TowerName;
    }

    public void SetTowerColor()
    {
        Color color = CanBePlaced() ? _canBePlacedColor : _canNotBePlacedColor;

        foreach(KeyValuePair<Material, Color> objectT in _allObjects)
        {
            objectT.Key.color = color;
        }
    }

    public void SetOrginalColor()
    {
        foreach (KeyValuePair<Material, Color> objectT in _allObjects)
        {
            objectT.Key.color = objectT.Value;
        }
    }

    public void HideInfoUI()
    {
        if (_canvasAnimator.GetBool("shown"))
        {
            _canvasAnimator.SetBool("shown", false);
            _audioSource.PlayOneShot(_hideUISound);
        }
        _towerRangeSprite.gameObject.SetActive(false);
    }

    public Texture GetTowerIcon()
    {
        return TowerData.TowerIcon;
    }

    public void UpdateTowerRangeVisual(float bonusTowerRange)
    {
        _towerRangeSprite.localScale = new Vector3(TowerData.TowerRange + bonusTowerRange, 
                                                   TowerData.TowerRange + bonusTowerRange,
                                                   1f);
    }

    public void CloseTowerInfoButton()
    {
        if (_canvasAnimator.GetBool("shown"))
        {
            _canvasAnimator.SetBool("shown", false);
            _audioSource.PlayOneShot(_hideUISound);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Tower"))
        {
            _collisionsAmount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Tower"))
        {
            _collisionsAmount--;
        }
    }

    public void HideTower()
    {
        _collisionsAmount = 0;
        SetOrginalColor();
    }

    protected virtual void OnDisable()
    {
        _towerRangeSprite.gameObject.SetActive(true);
    }

    private void HandleGameOver()
    {
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
        _canvasAnimator.SetBool("shown", false);
        _towerRangeSprite.gameObject.SetActive(false);
    }
}
