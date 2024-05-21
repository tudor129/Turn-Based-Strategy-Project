using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : BaseAction
{
    [SerializeField] Transform _bombProjectilePrefab;
    
    [SerializeField] int _damage = 60;
    
    int _maxThrowDistance = 7;
    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        
        
    }
    public override string GetActionName()
    {
        return "Bomb";
    }
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            _gridPosition = gridPosition, _actionValue = 0,
        };
    }
    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        
        Transform bombProjectileTransform = Instantiate(_bombProjectilePrefab, _unit.GetWorldPosition() + Vector3.up * 1.5f + Vector3.forward * 0.5f , Quaternion.identity);
        BombProjectile bombProjectile = bombProjectileTransform.GetComponent<BombProjectile>();
        bombProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        Debug.Log("called bomb action");
        ActionStart(onActionComplete);
    }
    void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }

    public int GetDamageAmount()
    {
        return _damage;
    }
}
