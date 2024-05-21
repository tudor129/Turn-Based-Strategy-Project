using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    static MouseWorld _instance;
    
    [SerializeField] LayerMask _mousePlaneLayerMask;
    

    MeshRenderer _sphere;

    void Awake()
    {
        _sphere = GetComponentInChildren<MeshRenderer>();
        _instance = this;
        CameraController.Instance.OnMiddleMousePress += CameraControllerInstance_OnMiddleMousePress;
        CameraController.Instance.OnMiddleMouseRelease += CameraControllerInstance_OnMiddleMouseRelease;
    }
    void CameraControllerInstance_OnMiddleMouseRelease(object sender, EventArgs e)
    {
        _sphere.gameObject.SetActive(true);
    }
    void CameraControllerInstance_OnMiddleMousePress(object sender, EventArgs e)
    {
        _sphere.gameObject.SetActive(false);
    }

    void Update()
    {
        transform.position = MouseWorld.GetPosition();
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _instance._mousePlaneLayerMask);
        return raycastHit.point;
    }
    
}
