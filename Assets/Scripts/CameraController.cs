using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    const float MIN_FOLLOW_Y_OFFSET = 2f;
    const float MAX_FOLLOW_Y_OFFSET = 12f;

    public event EventHandler OnMiddleMousePress;
    public event EventHandler OnMiddleMouseRelease;
    
    public static CameraController Instance { get; private set; }
    
    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] float _sensitivity = 1;
    CinemachineTransposer _cinemachineTransposer;

    Vector3 _targetFollowOffset;
    

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one CameraController!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }
    void Update()
    {
        HandleMovement();

        HandleRotationKeyboard();
        HandleRotationMouse();

        HandleZoom();
    }
    void HandleZoom()
    {
        
        float zoomIncreaseAmount = 0.5f;
        _targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        
        
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
    void HandleRotationKeyboard()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmountKeyboard();
       
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime; 
    }

    void HandleRotationMouse()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (InputManager.Instance.IsMiddleMouseButtonDown())
        {
            Cursor.lockState = CursorLockMode.Locked;
            OnMiddleMousePress.Invoke(this, EventArgs.Empty);
           

            rotationVector.y = InputManager.Instance.GetCameraRotateAmountMouse() * _sensitivity;
        } 
       
        else
        {
            OnMiddleMouseRelease.Invoke(this, EventArgs.Empty);
            Cursor.lockState = CursorLockMode.None;
        }
        
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime; 
    }
    
    void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
       
        float moveSpeed = 10f;
        Vector3 moveVector = inputMoveDir.y * transform.forward + inputMoveDir.x * transform.right;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
