using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private float _rotationOffset = -90f;
    [SerializeField] private float _animationSmoothFactor = 0.07f;
    [SerializeField] private float _animationSmoothMultiplier = 1.15f;

    [Header("To Attach")]
    [SerializeField] private LayerMask _groundLayer;

    private Animator _animator;
    private Vector3 _playerInput;
    private Vector3 _mousePosition;
    private CharacterController _controller;
    private Controls _controls;
    private float _animAngle;
    private float _angle;
    private float _animatorVelocityX = 0f;
    private float _animatorVelocityY = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controls = new Controls();
        _controls.Player.Enable();  
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        RotatePlayer();
        MovePlayer();
    }

    private void ReadInput()
    {
        Vector2 playerInputRaw = _controls.Player.Movement.ReadValue<Vector2>();
        _playerInput = new Vector3(playerInputRaw.x, 0f, playerInputRaw.y).normalized;

        _animAngle = AngleBetweenVectors((_mousePosition - transform.position).normalized, _playerInput, Vector3.up);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit groundHit;
        if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, _groundLayer))
        {
            _mousePosition = groundHit.point;
        }

        Vector3 lookDirection = _mousePosition - transform.position;
        _angle = Mathf.Atan2(lookDirection.x, lookDirection.z) *
                             Mathf.Rad2Deg + _rotationOffset;
    }

    private void MovePlayer()
    {
        if (_playerInput.magnitude > 0.1f)
        {
            //-1 left; 1 right
            _animatorVelocityX = Mathf.Lerp(_animatorVelocityX, Mathf.Sin(_animAngle * Mathf.PI / 180), _animationSmoothFactor * Time.deltaTime);
            //-1 backward; 1 forward
            _animatorVelocityY = Mathf.Lerp(_animatorVelocityY, Mathf.Cos(_animAngle * Mathf.PI / 180), _animationSmoothFactor * Time.deltaTime);

            _animator.SetFloat("velocityX", _animatorVelocityX * _animationSmoothMultiplier);
            _animator.SetFloat("velocityY", _animatorVelocityY * _animationSmoothMultiplier);
        }
        else
        {
            _animator.SetFloat("velocityX", 0f);
            _animator.SetFloat("velocityY", 0f);
        }
        //add gravity
        _playerInput.y = -1f;

        _controller.Move(_playerInput * _playerSpeed * Time.deltaTime);
    }

    private void RotatePlayer()
    {      
        transform.rotation = Quaternion.Euler(0, _angle, 0);
    }

    private float AngleBetweenVectors(Vector3 lookingDirection, Vector3 movingDirection, Vector3 upwardsDirection)
    {
        float rawAngle = Vector3.Angle(lookingDirection, movingDirection);
        float sign = Mathf.Sign(Vector3.Dot(upwardsDirection, Vector3.Cross(lookingDirection, movingDirection)));

        //(-180f; 180f)
        float angle = rawAngle * sign;
        return angle;
    }
}
