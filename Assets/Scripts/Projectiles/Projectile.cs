using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.UI;

public class Projectile : MonoBehaviour
{

    Action OnProjectileBehaviourComplete;
    
    [SerializeField] Transform _projectileHitPrefab;
    [SerializeField] ShootAction _shoot;

    
    Vector3 _targetPosition;

    static string _nameOfTheTarget;


    public void Setup(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }
   
    void Update()
    {
        //Debug.Log(_targetPosition);
        Vector3 moveDir = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        
        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            
            Vector3 hitEffectPosition = _targetPosition;

            hitEffectPosition.y = hitEffectPosition.y * 2;
           
            
            Instantiate(_projectileHitPrefab, hitEffectPosition, Quaternion.identity);
           
            Destroy(gameObject);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        // other.gameobject also works
        var healthSystem = other.attachedRigidbody.gameObject.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.Damage(_shoot.GetDamageAmount());
            ScreenShake.Instance.Shake();
        }
        
    }
    
   
}
