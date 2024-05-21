using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    GridSystem<GridObject> _gridSystem;
    GridPosition _gridPosition;
    List<Unit> _unitList;
    List<DestructibleProp> _destructiblePropsList;
    IInteractable _interactable;
    IAttackable _attackable;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        _gridSystem = gridSystem;
        _gridPosition = gridPosition;
        _unitList = new List<Unit>();
        _destructiblePropsList = new List<DestructibleProp>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (var unit in _unitList)
        {
            unitString += unit + "\n";
        }
        return  _gridPosition.ToString() + "\n" + unitString;
    }

    public void AddProp(DestructibleProp destructibleProp)
    {
        _destructiblePropsList.Add(destructibleProp);
    }
    public void RemoveProp(DestructibleProp destructibleProp)
    {
        _destructiblePropsList.Remove(destructibleProp);
    }
    public List<DestructibleProp> GetPropList()
    {
        return _destructiblePropsList;
    }
    public bool HasAnyProp()
    {
        return _destructiblePropsList.Count > 0;
    }

    public DestructibleProp GetProp()
    {
        if (HasAnyProp())
        {
            return _destructiblePropsList[0];
        }
        else
        {
            return null;
        }
    }
    
    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        _unitList.Remove(unit);
    }
    public List<Unit> GetUnitList()
    {
        return _unitList;
    }
    public bool HasAnyUnit()
    {
        return _unitList.Count > 0;
    }
    public Unit GetUnit()
    {
        if (HasAnyUnit())
        {
            return _unitList[0];
        }
        else
        {
            return null;
        }
    }
    
    public IInteractable GetInteractable()
    {
        return _interactable;
    }
    public void SetInteractable(IInteractable interactable)
    {
        _interactable = interactable;
    }

    public IAttackable GetAttackable()
    {
        return _attackable;
    }
    public void SetAttackable(IAttackable attackable)
    {
        _attackable = attackable;
    }

    
}
