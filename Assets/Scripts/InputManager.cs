#define USE_NEW_INPUT_SYSTEM
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
      public static InputManager Instance { get; private set; }

      PlayerInputActions _playerInputActions;

      void Awake()
      {
            if (Instance != null)
            {
                  Debug.LogError("There's more than one InputManager!" + transform + "-" + Instance);
                  Destroy(gameObject);
                  return;
            }
            Instance = this;

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
      }

      public Vector2 GetMouseScreenPosition()
      {
#if USE_NEW_INPUT_SYSTEM
            return Mouse.current.position.ReadValue();
#else
            return Input.mousePosition;
#endif
      }

      public bool IsMouseButtonDownThisFrame()
      {
#if USE_NEW_INPUT_SYSTEM
            return _playerInputActions.Player.Click.WasPressedThisFrame();
#else
            return Input.GetMouseButtonDown(0);
#endif
      }

      public Vector2 GetCameraMoveVector()
      {
#if USE_NEW_INPUT_SYSTEM
            return _playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
            Vector2 inputMoveDir = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                  inputMoveDir.y = +1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                  inputMoveDir.y = -1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                  inputMoveDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                  inputMoveDir.x = +1f;
            }

            return inputMoveDir;
#endif
      }

      public float GetCameraRotateAmountKeyboard()
      {
#if USE_NEW_INPUT_SYSTEM
            return _playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
            float rotateAmount = 0f;

            if (Input.GetKey(KeyCode.Q))
            {
                  rotateAmount = +1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                  rotateAmount = -1f;
            }

            return rotateAmount;
#endif
      }

      public bool IsMiddleMouseButtonDown()
      {
            return Input.GetKey(KeyCode.Mouse2);

      }

      public float GetCameraRotateAmountMouse()
      {
            float rotateAmount = 0f;

            if (Input.GetAxis("Mouse X") < 0)
            {
                  rotateAmount = -1f;
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                  rotateAmount = +1f;
            }
            
            return rotateAmount;
      }

      public float GetCameraZoomAmount()
      {
#if USE_NEW_INPUT_SYSTEM

            return _playerInputActions.Player.CameraZoom.ReadValue<float>();
#else

            float zoomAmount = 0;
            if (Input.mouseScrollDelta.y > 0)
            {
                  zoomAmount = -1;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                  zoomAmount = +1;
            }

            return zoomAmount;
#endif
      }
}
