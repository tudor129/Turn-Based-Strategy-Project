using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    GridPosition _gridPosition;
    PathNode _cameFromPathNode;
    
    int _gCost;
    int _hCost;
    int _fCost;
    bool _isWalkable = true;

    
    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }
    
    public override string ToString()
    {

        return _gridPosition.ToString();
    }

    public int GetGCost()
    {
        return _gCost;
    }
    public int GetHCost()
    {
        return _hCost;
    }
    public int GetFCost()
    {
        return _fCost;
    }

    public void SetGCost(int gCost)
    {
        _gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        _hCost = hCost;
    }

    public void CalculateFCost()
    {
        _fCost = _hCost + _gCost;
    }

    public void ResetCameFromPathNode()
    {
        _cameFromPathNode = null;
    }

    public void SetCameFromPathNode(PathNode pathNode)
    {
        _cameFromPathNode = pathNode;
    }
    public PathNode GetCameFromPathNode()
    {
        return _cameFromPathNode;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }
    public bool IsWalkable()
    {
        return _isWalkable;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        _isWalkable = isWalkable;
    }
    
}
