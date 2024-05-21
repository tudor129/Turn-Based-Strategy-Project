using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridPosition : IEquatable<GridPosition>
{
    public int _x;
    public int _z;

    public GridPosition(int x, int z)
    {
        _x = x;
        _z = z;
    }

    public override string ToString()
    {
        //return "x: " + _x + ": z:" + _z;
        return $"x: {_x}; z: {_z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a._x == b._x && a._z == b._z;
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a._x + b._x, a._z + b._z);
    }
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a._x - b._x, a._z - b._z);
    }
    
    public bool Equals(GridPosition other)
    {
        return _x == other._x && _z == other._z;
    }
    public override bool Equals(object obj)
    {
        return obj is GridPosition other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(_x, _z);
    }
}
    