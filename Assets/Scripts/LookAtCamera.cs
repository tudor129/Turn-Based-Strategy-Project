using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform _cameraTransform;
    
    void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(_cameraTransform);
    }
}
