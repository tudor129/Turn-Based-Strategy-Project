using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
   void Start()
   {
      Hide();
      UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;     
   }
   void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
   {
      if (isBusy)
      {
         Show();
      }
      else
      {
         Hide();
      }
   }

   void Show()
   {
      gameObject.SetActive(true);
   }
   void Hide()
   {
      gameObject.SetActive(false);
   }
   
}
