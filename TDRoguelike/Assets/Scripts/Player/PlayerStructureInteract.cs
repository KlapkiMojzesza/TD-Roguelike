using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStructureInteract : MonoBehaviour
{

    [SerializeField] float interactRange = 10f;

    [SerializeField] LayerMask buildingLayer;

    public static event Action<GameObject> OnPlayerInteract;

    Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Enable();
        controls.Player.Interact.performed += HandlePlayerInteract;
    }

    private void HandlePlayerInteract(InputAction.CallbackContext context)
    {
        Collider[] hitBuildings = Physics.OverlapSphere(transform.position, interactRange, buildingLayer);
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
