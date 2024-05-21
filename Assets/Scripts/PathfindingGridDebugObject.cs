using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
   [SerializeField] TextMeshPro _gCostText;
   [SerializeField] TextMeshPro _hCostText;
   [SerializeField] TextMeshPro _fCostText;
   [SerializeField] SpriteRenderer _isWalkableSpriteRenderer;

   PathNode _pathNode;
   public override void SetGridObject(object gridObject)
   {
      base.SetGridObject(gridObject);
      _pathNode = (PathNode)gridObject;
   }

   protected override void Update()
   {
      base.Update();
      _gCostText.text = _pathNode.GetGCost().ToString();
      _hCostText.text = _pathNode.GetHCost().ToString();
      _fCostText.text = _pathNode.GetFCost().ToString();
      _isWalkableSpriteRenderer.color = _pathNode.IsWalkable() ? Color.green : Color.red;
   }

}
