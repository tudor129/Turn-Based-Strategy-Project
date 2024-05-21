using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    
    void Start()
    {
        DestructibleProp.OnAnyDestroyed += DestructibleStatue_OnAnyDestroyed;
    }
    void DestructibleStatue_OnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleProp destructibleStatue = sender as DestructibleProp;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleStatue.GetGridPosition(), true);
    }

   
}
