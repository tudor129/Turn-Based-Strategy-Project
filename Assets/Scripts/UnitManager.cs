using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
   public static UnitManager Instance { get; private set; }
   
   List<Unit> _unitList;
   List<Unit> _friendlyUnitList;
   List<Unit> _enemyUnitList;

   void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("There's more than one UnitManager!" + transform + "-" + Instance);
         Destroy(gameObject);
         return;
      }
      Instance = this;
      
      _unitList = new List<Unit>();
      _friendlyUnitList = new List<Unit>();
      _enemyUnitList = new List<Unit>();
   }
   void Start()
   {
      Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
      Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
      
   }
   
   void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
   {
      Unit unit = sender as Unit;
      
      Debug.Log(unit + "Spawned");
      
      _unitList.Add(unit);

      if (unit.IsEnemy())
      {
         _enemyUnitList.Add(unit);
      }
      else
      {
         _friendlyUnitList.Add(unit);
      }
   }
   
   void Unit_OnAnyUnitDead(object sender, EventArgs e)
   {
      Unit unit = sender as Unit;
      
      Debug.Log(unit + "Died");
      
      _unitList.Remove(unit);

      if (unit.IsEnemy())
      {
         _enemyUnitList.Remove(unit);
      }
      else
      {
         _friendlyUnitList.Remove(unit);
      }

     
   }
   
   

   public List<Unit> GetUnitList()
   {
      return _unitList;
   }
   public List<Unit> GetFriendlyUnitList()
   {
      return _friendlyUnitList;
   }
   public List<Unit> GetEnemyUnitList()
   {
      return _enemyUnitList;
   }
}
