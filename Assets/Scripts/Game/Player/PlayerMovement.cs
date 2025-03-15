using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Animator _animator;
    private PhotonView _photonView;
    private CameraController _cameraController;

    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 8f;
    [SerializeField] private float _acceleration = 6f;

    private Vector2 _movementInput;
    private bool _runInput;

    private float _speed;
    private Vector2 _movementVector;
    private Vector3 _velocity;
    private float _gravity = -9.81f;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();

        _cameraController = Camera.main.GetComponent<CameraController>();

        if (_photonView.IsMine)
            _cameraController.SetTarget(transform);

        _playerInput.KeyboardGameplayInput.Move.started += OnMovePerformed;
        _playerInput.KeyboardGameplayInput.Move.performed += OnMovePerformed;
        _playerInput.KeyboardGameplayInput.Move.canceled += OnMovePerformed;
        _playerInput.KeyboardGameplayInput.Run.started += OnRunChanged;
        _playerInput.KeyboardGameplayInput.Run.canceled += OnRunChanged;

        _speed = _walkSpeed;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
        _movementInput.Normalize();
    }

    private void OnRunChanged(InputAction.CallbackContext context)
    {
        _runInput = context.ReadValueAsButton();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        _speed = _runInput ? _runSpeed : _walkSpeed;
        Vector2 targetMovementVector = _movementInput * _speed;
        _movementVector = Vector2.Lerp(_movementVector, targetMovementVector, Time.deltaTime * _acceleration);

        _animator.SetFloat("Speed", _movementVector.magnitude);

        _velocity = new Vector3(_movementVector.x, _velocity.y + _gravity * Time.deltaTime, _movementVector.y);

        if (_characterController.isGrounded)
        {
            _velocity.y = 0;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }
    private void Rotate()
    {
        if (_movementInput.magnitude > 0)
        {
            Vector3 movementWithoutVertical = new Vector3(_velocity.x, 0, _velocity.z);
            Quaternion targetRotation = Quaternion.LookRotation(movementWithoutVertical, Vector3.up);
            Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRotation, _acceleration * Time.deltaTime);
            transform.rotation = rotation;
        }
    }
}
