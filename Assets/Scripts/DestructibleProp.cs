using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleProp : MonoBehaviour, IAttackable
{

    public static event EventHandler OnAnyDestroyed;

    [SerializeField] Transform _propDestroyedPrefab;
    [SerializeField] bool _isDestroyable = true;

    Action _onAttackComplete;
    GridPosition _gridPosition;
    bool _isActive;
    float _timer;


    void Awake()
    {
    }

    void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetAttackableAtGridPosition(_gridPosition, this);
        
    }

    public bool IsDestroyable()
    {
        return _isDestroyable;
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _timer = Time.deltaTime;

        if (_timer <= 0f)
        {
            _isActive = false;
            _onAttackComplete();
        }
    }

    public void Damage()
    {
        if (_propDestroyedPrefab != null)
        {
           Transform propDestroyedTransform = Instantiate(_propDestroyedPrefab, transform.position, transform.rotation);
           
           ApplyExplosionToChildren(propDestroyedTransform, 500f, transform.position, 10f, -0.1f);
        }
        
        Destroy(gameObject);
        
        OnAnyDestroyed?.Invoke(this,EventArgs.Empty);
    }
    
    void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange, float upwardsModifier)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRb))
            {
                childRb.AddExplosionForce(explosionForce, explosionPosition, explosionRange, upwardsModifier);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange, upwardsModifier);
        }
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public void ReceiveAttack(Action onAttackComplete)
    {
        _onAttackComplete = onAttackComplete;
        _isActive = true;
        _timer = 0.5f;
        Damage();
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
