using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject _actionCameraGameObject;
    [SerializeField] Vector3 _cameraCharacterHeight;
    [SerializeField] Vector3 _shootDir;
    [SerializeField] float _characterHeightOffset = 1.7f;
    [SerializeField] float _shoulderOffsetAmount = 0.5f;
    [SerializeField] float shoulderOffsetX = 0f;
    [SerializeField] float shoulderOffsetY = 0f;
    [SerializeField] float shoulderOffsetZ = 0f;
    [SerializeField] float _distanceFromCharacter = -2f;


    void Start()
    {
        BaseAction.OnAnyActionStarted += BaseActionOnOnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseActionOnOnAnyActionCompleted;
        
        HideActionCamera();
    }
    void BaseActionOnOnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
    void BaseActionOnOnAnyActionStarted(object sender, EventArgs e)
    {
        // this is one method
        // if (sender is ShootAction)
        // {
        //     
        // }

        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();
                _cameraCharacterHeight = Vector3.up * _characterHeightOffset;
                _shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                
                Vector3 shoulderOffset = Quaternion.Euler(shoulderOffsetX, shoulderOffsetY, shoulderOffsetZ) * _shootDir * _shoulderOffsetAmount;
                
                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + _cameraCharacterHeight + shoulderOffset + _shootDir * -_distanceFromCharacter;
                
                _actionCameraGameObject.transform.position = actionCameraPosition;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition());
                    
                //ShowActionCamera();
                break;
        }
    }

    void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
    }
    
    void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
    }
}
