using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8;
    private int _damage = 5;

    private GameObject _origin;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        Destroy(gameObject, 1f);
    }

    public void Setup(Vector3 direction)
    {
        transform.forward = direction;
        _rigidbody.AddForce(direction.normalized * _speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (_origin != null)
            {
                damageable.TakeDamage(_damage);
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                _origin = other.gameObject;
            }
        }
    }
}
