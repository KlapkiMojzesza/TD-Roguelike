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
    [SerializeField] private int towerPrize = 10;
    [SerializeField] private string towerName = "TOWER";
    [SerializeField] private Texture towerIcon;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private float towerRangeVisualFactor = 0.041666f;

    [Header("To Attach")]
    [SerializeField] Renderer renderer;
    [SerializeField] RawImage iconImage;
    [SerializeField] GameObject towerHitBox;
    [SerializeField] GameObject towerInfoCanvas;
    [SerializeField] GameObject towerRangeVisual;
    [SerializeField] TMP_Text towerStatsText;
    [SerializeField] TMP_Text towernNameText;

    TowerShooting towerShooting;
    Material myMaterial;
    Color orginalColor;
    int collisionsAmount = 0;
    bool canBePlaced = true;
    bool isPlaced = false;

    Controls controls;

    private void Awake()
    {
        TowerManager.OnNextWaveButtonClicked += HandleStartWave;
        TowerManager.OnTowerSelect += HandleAnotherTowerSelected;

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.Info.performed += HandlePlayerMouseInfo;

        myMaterial = renderer.material;
        orginalColor = myMaterial.color;
        towerShooting = GetComponent<TowerShooting>();
        iconImage.texture = towerIcon;
        SetTowerRangeVisual();
    }

    private void OnDestroy()
    {
        TowerManager.OnNextWaveButtonClicked -= HandleStartWave;
        TowerManager.OnTowerSelect -= HandleAnotherTowerSelected;
        controls.Player.Info.performed -= HandlePlayerMouseInfo;
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

    //prototype change later (not work)
    private void SetTowerRangeVisual()
    {
        towerRangeVisual.transform.localScale = new Vector3(towerShooting.towerRange * towerRangeVisualFactor,
                                                            towerShooting.towerRange * towerRangeVisualFactor,
                                                            0f);
    }

    private void HandleAnotherTowerSelected()
    {
        towerInfoCanvas.SetActive(false);
    }

    public int GetTowerPrize()
    {
        return towerPrize;
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
        towerStatsText.text = $"Damage: {towerShooting.towerDamage.ToString()}\n" +
                              $"Range: {towerShooting.towerRange.ToString()}\n" +
                              $"FireRate: {towerShooting.towerFireRate.ToString()}";

        towernNameText.text = towerName;
    }

    public void SetTowerColor()
    {
        Color color = CanBePlaced() ? Color.green : Color.red;
        myMaterial.color = color;
    }

    public void SetOrginalColor()
    {
        myMaterial.color = orginalColor;
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
