using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VillageBuilding : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask buildingLayer;

    [Header("To Attach")]
    [SerializeField] GameObject buildingCanvas;
    [SerializeField] TowerScriptableObject towerData;

    Controls controls;

    private void Start()
    {
        /*controls = new Controls();
        controls.Player.Enable();
        controls.Player.Info.performed += HandlePlayerMouseInfo;*/

        PlayerStructureInteract.OnPlayerInteract += ShowUI;
    }

    private void ShowUI(GameObject wantedBuilding)
    {
        if (wantedBuilding == this.gameObject)
        {
            buildingCanvas.SetActive(true);
            return;
        }
        buildingCanvas.SetActive(false);       
    }

    private void OnDestroy()
    {
        //controls.Player.Info.performed -= HandlePlayerMouseInfo;
        PlayerStructureInteract.OnPlayerInteract -= ShowUI;
    }

    public void BoostDamage()
    {
        towerData.towerDamage += 50;
    }

    public void BoostRange()
    {
        towerData.towerRange += 20;
    }

    public void BoostFirerate()
    {
        towerData.towerFireRate += 2;
    }

    private void HandlePlayerMouseInfo(InputAction.CallbackContext cpntext)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit buildingHit;

        if (!Physics.Raycast(ray, out buildingHit, Mathf.Infinity, buildingLayer))
        {
            buildingCanvas.SetActive(false);
            return;
        }

        GameObject towerHitGameObject = buildingHit.transform.gameObject;
        if (towerHitGameObject != this.gameObject)
        {
            buildingCanvas.SetActive(false);
            return;
        }

        buildingCanvas.SetActive(true);
    }
}
