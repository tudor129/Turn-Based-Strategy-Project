using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
   public static UnitActionSystem Instance { get; private set; }
   
   public event EventHandler OnSelectedUnitChanged;
   public event EventHandler OnSelectedActionChanged;
   public event EventHandler<bool> OnBusyChanged;
   public event EventHandler OnActionStarted;
 
   [SerializeField] Unit _selectedUnit;
   [SerializeField] LayerMask _unitLayerMask;

   BaseAction _selectedAction;
   bool _isBusy;

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
      SetSelectedUnit(_selectedUnit);
   }

   void Update()
   {
      if (_isBusy)
      {
         return;
      }
      if (!TurnSystem.Instance.IsPlayerTurn())
      {
         return;
      }
      if (EventSystem.current.IsPointerOverGameObject())
      {
         return;
      }
      if (TryHandleUnitSelection())
      {
         return;
      }
      
      HandleSelectedAction();
   }

   void HandleSelectedAction()
   {
      if (InputManager.Instance.IsMouseButtonDownThisFrame())
      {
         GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

         if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))
         {
            if (_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
            {
               SetBusy();
               _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
               OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
         }
      }
   }
  
   void SetBusy()
   {
      _isBusy = true;
      OnBusyChanged?.Invoke(this, _isBusy);
   }
   void ClearBusy()
   {
      _isBusy = false;
      OnBusyChanged?.Invoke(this, _isBusy);
   }

   bool TryHandleUnitSelection()
   {
      if (InputManager.Instance.IsMouseButtonDownThisFrame())
      {
         Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
         if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
         {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
               if (unit == _selectedUnit)
               {
                  return false;
               }
               if (unit.IsEnemy())
               {
                  // Clicked on enemy
                  return false;
               }
               SetSelectedUnit(unit);
               return true;
            }
         }
      }
      return false;
   }

   void SetSelectedUnit(Unit unit)
   {
      _selectedUnit = unit;
      SetSelectedAction(unit.GetAction<MoveAction>());
      
      OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
   }
   public void SetSelectedAction(BaseAction baseAction)
   {
      _selectedAction = baseAction;
      OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
   }
   public Unit GetSelectedUnit()
   {
      return _selectedUnit;
   }

   public BaseAction GetSelectedAction()
   {
      return _selectedAction;
   }
}


