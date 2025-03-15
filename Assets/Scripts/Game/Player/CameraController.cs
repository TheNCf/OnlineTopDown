using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _smooth = 0.5f;

    [SerializeField] private float _height = 5.0f;
    [SerializeField] private float _xOffset = 0.0f;
    [SerializeField] private float _zOffset = -4.0f;

    private Transform _target;
    private Vector3 _velocity;

    private void Update()
    {
        Work();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Work()
    {
        if (_target == null)
            return;

        Vector3 targetPosition = new Vector3(_target.position.x + _xOffset, _height, _target.position.z + _zOffset);
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smooth);
        transform.position = newPosition;
    }
}
