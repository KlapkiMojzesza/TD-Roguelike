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
    [SerializeField] private LayerMask towerLayer;

    [Header("To Attach")]
    public TowerScriptableObject towerData;
    [SerializeField] Renderer[] renderers;
    [SerializeField] RawImage iconImage;
    [SerializeField] GameObject towerHitBox;
    [SerializeField] GameObject towerInfoCanvas;
    [SerializeField] TMP_Text towerStatsText;
    [SerializeField] TMP_Text towernNameText;

    int collisionsAmount = 0;
    bool canBePlaced = true;
    bool isPlaced = false;

    Controls controls;
    Dictionary<Material, Color> allObjects = new Dictionary<Material, Color>();

    private void Awake()
    {
        TowerManager.OnNextWaveButtonClicked += HandleStartWave;
        TowerManager.OnTowerSelect += HandleAnotherTowerSelected;

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Info.performed += HandlePlayerMouseInfo;

        SetAllBuildingMaterials();

        iconImage.texture = towerData.towerIcon;
    }

    private void OnDestroy()
    {
        TowerManager.OnNextWaveButtonClicked -= HandleStartWave;
        TowerManager.OnTowerSelect -= HandleAnotherTowerSelected;
        controls.Player.Info.performed -= HandlePlayerMouseInfo;
    }

    public void SetAllBuildingMaterials()
    {
        foreach (Renderer render in renderers)
        {
            Material[] materials = render.materials;
            foreach (Material material in materials)
            {
                if (!allObjects.ContainsKey(material))
                {
                    allObjects.Add(material, material.color);
                }
            }
        }
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext cpntext)
    {
        if (!isPlaced) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit towerHit;

        if (!Physics.Raycast(ray, out towerHit, Mathf.Infinity, towerLayer))
        {
            towerInfoCanvas.SetActive(false);
            return;
        }

        GameObject towerHitGameObject = towerHit.transform.gameObject;
        if (towerHitGameObject != this.gameObject)
        {
            towerInfoCanvas.SetActive(false);
            return;
        }

        towerInfoCanvas.SetActive(true);
    }

    private void HandleStartWave()
    {
        towerInfoCanvas.SetActive(false);
    }

    private void HandleAnotherTowerSelected()
    {
        towerInfoCanvas.SetActive(false);
    }

    public bool CanBePlaced()
    {
        if (collisionsAmount == 0 && canBePlaced) return true;

        return false;
    }

    public void PlaceTower()
    {
        towerHitBox.SetActive(true);
        isPlaced = true;
        UpdateTowerStatsUI();

        towerInfoCanvas.SetActive(true);
    }

    private void UpdateTowerStatsUI()
    {
        towerStatsText.text = $"Damage: {towerData.towerDamage.ToString()}\n" +
                              $"Range: {towerData.towerRange.ToString()}\n" +
                              $"FireRate: {towerData.towerFireRate.ToString()}\n" +
                              $"Pierce: {towerData.towerEnemyPierce.ToString()}";

        towernNameText.text = towerData.towerName;
    }

    public void SetTowerColor()
    {
        Color color = CanBePlaced() ? Color.green : Color.red;

        foreach(KeyValuePair<Material, Color> objectT in allObjects)
        {
            objectT.Key.color = color;
        }
    }

    public void SetOrginalColor()
    {
        foreach (KeyValuePair<Material, Color> objectT in allObjects)
        {
            objectT.Key.color = objectT.Value;
        }
    }

    public Texture GetTowerIcon()
    {
        return towerData.towerIcon;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            collisionsAmount--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            collisionsAmount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            canBePlaced = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            canBePlaced = true;
        }
    }
}
