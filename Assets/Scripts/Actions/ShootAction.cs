using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    
    public event EventHandler OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit _targetUnit;
        public Unit _attackerUnit;
    }
    
    [SerializeField] int _damage = 40;
    enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    [SerializeField] LayerMask _obstaclesLayerMask;
    
    State _state;
    int _maxShootDistance = 7;
    float _stateTimer;
    Unit _targetUnit;
    bool _canShoot;

    

    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case State.Aiming:
                Vector3 aimDir = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                
                float rotSpeed = 5;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotSpeed);
                break;
            case State.Shooting:
                if (_canShoot)
                {
                    Shoot();
                    _canShoot = false;
                    
                }
                break;
            case State.Cooloff:
                break;
        }

       if (_stateTimer <= 0f)
        {
            NextState();
        }
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return _maxShootDistance;
    }
    
    public int GetDamageAmount()
    {
        return _damage;
    }
    void Shoot()
    {
        OnShoot?.Invoke(this, EventArgs.Empty);
        
        OnShoot?.Invoke(this, new OnShootEventArgs()
        {
            _targetUnit = _targetUnit,
            _attackerUnit = _unit
        });
        
        //_targetUnit.Damage(_damage);
    }

   

    void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTime = 0.1f;
                _stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                _state = State.Cooloff;
                float cooloffStateTime = 2.5f;
                _stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

   
    
    public override string GetActionName()
    {
        return "Shoot";
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = State.Aiming;
        float aimingStateTime = 1f;
        _stateTimer = aimingStateTime;

        _canShoot = true;
        
        ActionStart(onActionComplete);
    }

    void OnProjectileActionComplete()
    {
        ActionComplete();
    }


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        
        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootDistance)
                {
                    continue;
                }
                
                // validGridPositionList.Add(testGridPosition);
                // continue; this visually shows the code above
                
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 direction = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                
                float unitShoulderHeight = 1.7f;
                
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight,
                    direction,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    _obstaclesLayerMask))
                {
                    // blocked by obstacle
                    continue;
                }

                validGridPositionList.Add(testGridPosition);

            }
        }
        return validGridPositionList;
    }
    
    
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        
        return new EnemyAIAction()
        {
            
            _gridPosition = gridPosition, _actionValue = 120 + Mathf.RoundToInt( 1 - targetUnit.GetHealthNormalized() * 100f),
        };
        
        
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        //Debug.Log(GetValidActionGridPositionList(gridPosition).Count);
       return GetValidActionGridPositionList(gridPosition).Count;
    }

    
}
