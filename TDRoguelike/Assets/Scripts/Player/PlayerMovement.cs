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
    private Vector3 _mousePosition;
    private CharacterController _controller;
    private Controls _controls;
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
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector2 playerInputRaw = _controls.Player.Movement.ReadValue<Vector2>();
        Vector3 playerInput = new Vector3(playerInputRaw.x, 0f, playerInputRaw.y).normalized;

        float animAngle = SignedAngleBetween((_mousePosition - transform.position).normalized, playerInput, Vector3.up);


        if (playerInput.magnitude > 0.1f)
        {
            //-1 left; 1 right
            _animatorVelocityX = Mathf.Lerp(_animatorVelocityX, Mathf.Sin(animAngle * Mathf.PI / 180), _animationSmoothFactor);
            //-1 backward; 1 forward
            _animatorVelocityY = Mathf.Lerp(_animatorVelocityY, Mathf.Cos(animAngle * Mathf.PI / 180), _animationSmoothFactor);

            _animator.SetFloat("velocityX", _animatorVelocityX * _animationSmoothMultiplier);
            _animator.SetFloat("velocityY", _animatorVelocityY * _animationSmoothMultiplier);
        }
        else
        {
            _animator.SetFloat("velocityX", 0f);
            _animator.SetFloat("velocityY", 0f);
        }
        //add gravity
        playerInput.y = -1f;

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

    private float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        float signed_angle = angle * sign;

        return signed_angle;
    }
}
