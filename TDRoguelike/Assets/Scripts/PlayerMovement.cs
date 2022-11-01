using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] private float rotationOffset = -90f;

    [Header("To Attach")]
    [SerializeField] private LayerMask groundLayer;

    Vector3 mousePosition;
    CharacterController controller;
    Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.Enable();  
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector2 playerInputRaw = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 playerInput = new Vector3(playerInputRaw.x, 0f, playerInputRaw.y);
        controller.Move(playerInput * playerSpeed * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit groundHit;
        if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, groundLayer))
        {
            mousePosition = groundHit.point;
        }

        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) *
                      Mathf.Rad2Deg + rotationOffset;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
