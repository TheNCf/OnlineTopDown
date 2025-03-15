using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITurnToCamera : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.forward = _camera.transform.forward;
    }
}
