using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAction : BaseAction
{
    public static event EventHandler OnAnyMeleeHit;
    
    public event EventHandler OnMeleeActionStarted;
    public event EventHandler OnMeleeActionCompleted;
    enum State
    {
        SwingingWeaponBeforeHit,
        SwingingWeaponAfterHit,
    }

    [SerializeField] int _damage = 100;
    int _maxMeleeDistance = 1;
    State _state;
    float _stateTimer;
    Unit _targetUnit;
    IAttackable _attackable;
    DestructibleProp _targetDestructibleProp;

    GridPosition _gridPosition;
  
    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        
        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            
            case State.SwingingWeaponBeforeHit:

                Vector3 aimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                
                float rotSpeed = 5;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotSpeed);
                break;
            case State.SwingingWeaponAfterHit:
                
                break;
        }

        if (_stateTimer <= 0f)
        {
            NextState();
        }
    }
    
    void NextState()
    {
        switch (_state)
        {
            case State.SwingingWeaponBeforeHit:
                _state = State.SwingingWeaponAfterHit;
                float afterHitStateTime = 0.5f;
                _stateTimer = afterHitStateTime;
                _targetUnit.Damage(_damage);
                OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingWeaponAfterHit:
                OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
           
        }
    }
    public override string GetActionName()
    {
        return "Melee";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            _gridPosition = gridPosition, _actionValue = 200,
        };
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        
        for (int x = -_maxMeleeDistance; x <= _maxMeleeDistance; x++)
        {
            for (int z = -_maxMeleeDistance; z <= _maxMeleeDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
               
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // grid position is empty, no unit
                    continue;
                }
               

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);


                if (targetUnit.IsEnemy() == _unit.IsEnemy())
                {
                    // both Units on same team
                    continue;
                }
                
                validGridPositionList.Add(testGridPosition);
                
            }
        }

        return validGridPositionList;
        
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _state = State.SwingingWeaponBeforeHit;
        float beforeHitStateTime = 0.7f;
        _stateTimer = beforeHitStateTime;
        
        OnMeleeActionStarted?.Invoke(this, EventArgs.Empty);
        
        ActionStart(onActionComplete);
    }

    void OnActionComplete()
    {
        ActionComplete();
    }
 
    public int GetMaxMeleeDistance()
    {
        return _maxMeleeDistance;
    }
   
    
}
