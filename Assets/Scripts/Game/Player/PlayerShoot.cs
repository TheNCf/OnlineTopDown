using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float _shootDistance = 8.0f;
    [SerializeField] private int _damage = 5;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletOriginTransform;
    [SerializeField] private Transform _aimTrailTransform;

    private PlayerInput _playerInput;
    private PhotonView _photonView;
    private Camera _camera;

    private bool _isAiming = false;
    private TargetMarker _currentTarget;

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        _playerInput = new PlayerInput();

        if (_photonView.IsMine)
        {
            _playerInput.KeyboardGameplayInput.Attack.started += OnInputAttack;
            _playerInput.KeyboardGameplayInput.Aim.started += OnInputAim;
            _playerInput.KeyboardGameplayInput.Aim.canceled += OnInputAim;
        }
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isAiming)
        {
            Aim();
        }
        else if (_currentTarget != null)
        {
            _currentTarget.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnInputAttack(InputAction.CallbackContext context)
    {
        Attack();
    }

    private void OnInputAim(InputAction.CallbackContext context)
    {
        _isAiming = context.ReadValueAsButton();
        _aimTrailTransform.gameObject.SetActive(_isAiming);
    }

    private void Attack()
    {
        if (_isAiming)
        {
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", _bulletOriginTransform.position, Quaternion.identity);

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.Setup(_aimTrailTransform.forward);
            Collider collider = bullet.GetComponent<Collider>();
            collider.enabled = true;
        }
    }

    private void Aim()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            _aimTrailTransform.LookAt(hitInfo.point);
            _aimTrailTransform.localEulerAngles = new Vector3(0, _aimTrailTransform.localEulerAngles.y, 0);
            Ray detectionRay = new Ray(transform.position, _aimTrailTransform.forward);

            if (Physics.Raycast(detectionRay, out RaycastHit detectionHit, 8.0f) && detectionHit.collider.TryGetComponent(out TargetMarker targetMarker))
            {
                if (targetMarker != _currentTarget && _currentTarget != null)
                    _currentTarget.GetComponent<MeshRenderer>().enabled = false;

                _currentTarget = targetMarker;
                _currentTarget.GetComponent<MeshRenderer>().enabled = true;
            }
            else if (_currentTarget != null)
            {
                _currentTarget.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}