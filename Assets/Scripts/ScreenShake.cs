using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    
    CinemachineImpulseSource _cinemachineImpulseSource;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScreenShake!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        _cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
