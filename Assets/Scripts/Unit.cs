using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IAttackable
{
    [SerializeField] int _maxActionPoints = 2;
    [SerializeField] bool _isEnemy;
    [SerializeField] int _meleeDamage = 100;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    
    
    GridPosition _gridPosition;
    BaseAction[] _baseActionArray;
    HealthSystem _healthSystem;
    

    int _actionPoints;
    void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _actionPoints = _maxActionPoints;
        
        _healthSystem.OnDead += HealthSystem_OnDead;
        
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    

    void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in _baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    
    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    
    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (_actionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetActionPoints()
    {
        return _actionPoints;
    }
    
    void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            ResetActionPoints();
        }
    }
    void ResetActionPoints()
    {
        _actionPoints = _maxActionPoints;
        
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool IsEnemy()
    {
        return _isEnemy;
    }
    public void Damage(int damageAmount)
    {
        Debug.Log(transform + "Took" + damageAmount + "damage");
        _healthSystem.Damage(damageAmount);
    }
    public void ReceiveAttack(Action onAttackComplete)
    {
        Damage(_meleeDamage);
    }
    public Vector3 GetWorldPosition()
    {
        if (transform.position != null)
        {
            return transform.position;
        }
        return transform.position;
    }
    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);
        
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        
        //gameObject.SetActive(false);
        //StartCoroutine(WaitForProjectile());
        Destroy(gameObject);
     
    }

    IEnumerator WaitForProjectile()
    {
        yield return new WaitForSeconds(2.1f);
    }


    public float GetHealthNormalized()
    {
        return _healthSystem.GetHealthNormalized();
    }
   
}
