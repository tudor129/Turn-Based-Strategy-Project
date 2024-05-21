using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
   public static GridSystemVisual Instance { get; private set; }

   [Serializable]
   public struct GridVisualTypeMaterial
   {
      public GridVisualType _gridVisualType;
      public Material _material;
   }
   public enum GridVisualType
   {
      White,
      Blue,
      Red,
      Yellow,
      RedSoft,
      Green
   }

   [SerializeField] List<GridVisualTypeMaterial> _gridVisualTypeMaterialList;
   [SerializeField] Transform _gridSystemVisualSinglePrefab;

   GridSystemVisualSingle[,] _gridSystemVisualSingleArray;
   Unit _unit;
   
   void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("There's more than one UnitActionSystem!" + transform + "-" + Instance);
         Destroy(gameObject);
         return;
      }
      Instance = this;
   }
   void Start()
   {
      _gridSystemVisualSingleArray = new GridSystemVisualSingle[
         LevelGrid.Instance.GetWidth(),
         LevelGrid.Instance.GetHeight()];
      for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
      {
         for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
         {
            GridPosition gridPosition = new GridPosition(x, z);

            Transform gridSystemVisualSingleTransform =
               Instantiate(_gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

            _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
         }
      }
      UpdateGridVisual();
      UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
      LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
   }
   void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
   {
      UpdateGridVisual();
   }
   void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
   {
      UpdateGridVisual();
   }

   Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
   {
      foreach (GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterialList)
      {
         if (gridVisualTypeMaterial._gridVisualType == gridVisualType)
         {
            return gridVisualTypeMaterial._material;
         }
      }
      Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
      return null;
   }

   void HideAllGridPositions()
   {
     
      for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
      {
         for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
         {

            _gridSystemVisualSingleArray[x, z].Hide();
         }
      }
   }

   void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
   {

      List<GridPosition> gridPositionList = new List<GridPosition>();
      for (int x = -range; x <= range; x++)
      {
         for (int z = -range; z <= range; z++)
         {
            GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
            
            if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
               continue;
            }
            
            int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
            if (testDistance > range)
            {
               continue;
            }
            gridPositionList.Add(testGridPosition);
            
         }
      }
      ShowGridPositionList(gridPositionList, gridVisualType);
   }
   
   void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
   {

      List<GridPosition> gridPositionList = new List<GridPosition>();
      for (int x = -range; x <= range; x++)
      {
         for (int z = -range; z <= range; z++)
         {
            GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
            
            if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
            {
               continue;
            }
            
            gridPositionList.Add(testGridPosition);
            
         }
      }
      ShowGridPositionList(gridPositionList, gridVisualType);
   }
   void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
   {
      foreach (GridPosition gridPosition in gridPositionList)
      {
         _gridSystemVisualSingleArray[gridPosition._x, gridPosition._z].Show(GetGridVisualTypeMaterial(gridVisualType));
      }
   }

   void UpdateGridVisual()
   {
      HideAllGridPositions();

      BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
      Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

      GridVisualType gridVisualType;

      switch (selectedAction)
      {
         default:
         case MoveAction moveAction:
            gridVisualType = GridVisualType.White;
            break;
         case ShootAction shootAction:
            gridVisualType = GridVisualType.Red;
            
            ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
            break;
         case MeleeAction meleeAction:
            gridVisualType = GridVisualType.Red;
            
            ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), meleeAction.GetMaxMeleeDistance(), GridVisualType.RedSoft);
            break;
         case BombAction bombAction:
            gridVisualType = GridVisualType.Yellow;
            break;
         case SpinAction spinAction:
            gridVisualType = GridVisualType.Blue;
            // asasaasfafafsa
            break;
         case InteractAction interactAction:
            gridVisualType = GridVisualType.Green;
            break;
         
      }
      
      ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
   }
}
