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
    [SerializeField] private TMP_Text _towerStatsText;
    [SerializeField] private TMP_Text _towerNameText;

    private Animator _canvasAnimator;
    private int _collisionsAmount = 0;
    private bool _isPlaced = false;

    private Controls _controls;
    private Dictionary<Material, Color> _allObjects = new Dictionary<Material, Color>();

    private void Awake()
    {
        TowerManager.OnNextWaveButtonClicked += HandleStartWave;
        TowerManager.OnTowerSelect += HandleAnotherTowerSelected;

        _controls = new Controls();
        _controls.Player.Enable();
        _controls.Player.Info.performed += HandlePlayerMouseInfo;

        SetAllBuildingMaterials();

        _iconImage.texture = TowerData.TowerIcon;
        _towerRangeSprite.localScale = new Vector3(TowerData.TowerRange, TowerData.TowerRange, 1f);

        _canvasAnimator = _towerInfoCanvas.GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        TowerManager.OnNextWaveButtonClicked -= HandleStartWave;
        TowerManager.OnTowerSelect -= HandleAnotherTowerSelected;
        _controls.Player.Info.performed -= HandlePlayerMouseInfo;
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
        if (!_isPlaced) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit towerHit;

        if (!Physics.Raycast(ray, out towerHit, Mathf.Infinity, _towerLayer))
        {
            _canvasAnimator.SetBool("shown", false);
            _towerRangeSprite.gameObject.SetActive(false);
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
        _canvasAnimator.SetBool("shown", true);
        //here upgrade canvas needs to update money amount
    }

    private void HandleStartWave()
    {
        _canvasAnimator.SetBool("shown", false);
        _towerRangeSprite.gameObject.SetActive(false);
    }

    //another tower button was clicked
    private void HandleAnotherTowerSelected(Tower selectedTower)
    {
        if (selectedTower != this)
        {
            _towerRangeSprite.gameObject.SetActive(false);
            _canvasAnimator.SetBool("shown", false);
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
    }

    private void UpdateTowerStatsUI()
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
        _canvasAnimator.SetBool("shown", false);
    }

    public Texture GetTowerIcon()
    {
        return TowerData.TowerIcon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _collisionsAmount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _collisionsAmount--;
        }
    }
}
