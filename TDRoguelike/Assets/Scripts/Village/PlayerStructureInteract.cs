using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStructureInteract : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _interactRange = 10f;
    [SerializeField] private LayerMask _buildingLayer;

    public static event Action<GameObject> OnPlayerInteract;

    private Controls _controls;

    private void Start()
    {
        _controls = new Controls();
        _controls.Enable();
        _controls.Player.Interact.performed += HandlePlayerInteract;
    }

    private void OnDestroy()
    {
        _controls.Player.Interact.performed -= HandlePlayerInteract;
    }

    private void HandlePlayerInteract(InputAction.CallbackContext context)
    {
        Collider[] hitBuildings = Physics.OverlapSphere(transform.position, _interactRange, _buildingLayer);
        GameObject closestBuilding = null;

        if (hitBuildings.Length == 0)
        {
            OnPlayerInteract?.Invoke(closestBuilding);
            return;
        }

        float shortestDistance = Mathf.Infinity;

        foreach (Collider building in hitBuildings)
        {
            float distanceToBuilding = Vector3.Distance(transform.position, building.transform.position);
            if (distanceToBuilding < shortestDistance)
            {
                shortestDistance = distanceToBuilding;
                closestBuilding = building.gameObject;
            }
        }

        OnPlayerInteract?.Invoke(closestBuilding);
    }
}
