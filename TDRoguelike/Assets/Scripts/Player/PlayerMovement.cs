using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private float _rotationOffset = -90f;

    [Header("To Attach")]
    [SerializeField] private LayerMask _groundLayer;

    private Vector3 _mousePosition;
    private CharacterController _controller;
    private Controls _controls;

    private void Start()
    {
        _controls = new Controls();
        _controls.Player.Enable();  
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector2 playerInputRaw = _controls.Player.Movement.ReadValue<Vector2>();
        Vector3 playerInput = new Vector3(playerInputRaw.x, -1f, playerInputRaw.y);
        _controller.Move(playerInput * _playerSpeed * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit groundHit;
        if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, _groundLayer))
        {
            _mousePosition = groundHit.point;
        }

        Vector3 lookDirection = _mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) *
                      Mathf.Rad2Deg + _rotationOffset;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
