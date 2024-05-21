using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class  LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    public event EventHandler OnAnyUnitMovedGridPosition;
    
    [SerializeField] Transform _gridDebugObjectPrefab;
    [SerializeField] int _width = 10;
    [SerializeField] int _height = 10;
    [SerializeField] float _cellSize = 2f;
    
    GridSystem<GridObject> _gridSystem;
    
    void Awake()
    {
        _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
        
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Pathfinding.Instance.Setup(_width, _height, _cellSize);
    }

    public void AddPropAtGridPosition(GridPosition gridPosition, DestructibleProp destructibleProp)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddProp(destructibleProp);
    }
    public void RemovePropAtGridPosition(GridPosition gridPosition, DestructibleProp destructibleProp)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveProp(destructibleProp);
    }
    public List<DestructibleProp> GetPropListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetPropList();
    }

    public void PropMovedGridPosition(DestructibleProp destructibleProp, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemovePropAtGridPosition(fromGridPosition, destructibleProp);
        AddPropAtGridPosition(toGridPosition, destructibleProp);
        
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }
    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => _gridSystem.GetWidth();
    public int GetHeight() => _gridSystem.GetHeight();

    public bool HasAnyPropOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
            return gridObject.HasAnyProp();
    }
    public DestructibleProp GetPropAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetProp();
    }
    
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }
    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
    public IAttackable GetAttackableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetAttackable();
    }

    public void SetAttackableAtGridPosition(GridPosition gridPosition, IAttackable attackable)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        gridObject.SetAttackable(attackable);
    }

    public Vector3 GetAttackableWorldPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystem.GetGridObject(gridPosition);
        return gridObject.GetAttackable().GetWorldPosition();
    }

   
    
}
