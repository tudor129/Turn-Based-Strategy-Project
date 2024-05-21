using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyBombExploded;
    
    
    [SerializeField] Transform _projectileHitPrefab;
    [SerializeField] TrailRenderer _trailRenderer;
    [SerializeField] AnimationCurve _arcYAnimationCurve;
    
    Vector3 _targetPosition;
    Vector3 _positionXZ;
    Action OnGrenadeBehaviourComplete;
    float totalDistance;

    void Update()
    {
        Vector3 moveDir = (_targetPosition - _positionXZ).normalized;
        float moveSpeed = 15f;
        _positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);


        
        float reachedTargetDistance = 0.2f;
        if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
        {
           
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
                
                if (collider.TryGetComponent<DestructibleProp>(out DestructibleProp destructibleProp))
                {
                    destructibleProp.Damage();
                }
                
                
            }
            Vector3 hitEffectPosition = _targetPosition;
            hitEffectPosition.y = hitEffectPosition.y * 2;

            _trailRenderer.transform.parent = null;
            Instantiate(_projectileHitPrefab, hitEffectPosition, Quaternion.identity);
            OnAnyBombExploded?.Invoke(this,EventArgs.Empty);
            Destroy(gameObject);
            OnGrenadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action OnGrenadeBehaviourComplete)
    {
        this.OnGrenadeBehaviourComplete = OnGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        _positionXZ = transform.position;
        _positionXZ.y = 0;
        
        totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
    }
    
    /*void OnTriggerEnter(Collider other)
    {
        

        var healthSystem = other.attachedRigidbody.gameObject.GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            Debug.Log("bomb hit");
            healthSystem.Damage(_bombAction.GetDamageAmount());
            ScreenShake.Instance.Shake();
        }
        
    }*/
}
