using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int towerPrize = 10;
    [SerializeField] private string towerName = "TOWER";
    [SerializeField] private float towerRangeVisualFactor = 0.041666f;

    [Header("To Attach")]
    [SerializeField] Renderer renderer;
    [SerializeField] GameObject towerHitBox;
    [SerializeField] GameObject towerInfoCanvas;
    [SerializeField] TMP_Text towerStatsText;
    [SerializeField] TMP_Text towernNameText;
    [SerializeField] GameObject towerRangeVisual;

    TowerShooting towerShooting;
    Material myMaterial;
    Color orginalColor;
    int collisionsAmount = 0;
    bool canBePlaced = true;

    private void Awake()
    {
        myMaterial = renderer.material;
        orginalColor = myMaterial.color;
        towerShooting = GetComponent<TowerShooting>();
        TowerManager.OnNextWaveButtonClicked += HandleStartWave;
        TowerManager.OnTowerSelect += HandleAnotherTowerSelected;
        SetTowerRangeVisual();
    }

    private void OnDestroy()
    {
        TowerManager.OnNextWaveButtonClicked -= HandleStartWave;
        TowerManager.OnTowerSelect -= HandleAnotherTowerSelected;
    }

    private void HandleStartWave()
    {
        towerInfoCanvas.SetActive(false);
    }

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

    public void SetTowerColor()
    {
        Color color = CanBePlaced() ? Color.green : Color.red;
        myMaterial.color = color;
    }

    public void SetOrginalColor()
    {
        myMaterial.color = orginalColor;
    }

    public void PlaceTower()
    {
        towerHitBox.SetActive(true);

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
