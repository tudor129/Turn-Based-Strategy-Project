using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI _textMeshPro;
   [SerializeField] Button _button;
   [SerializeField] GameObject _selectedGameObject;

   BaseAction _baseAction;


   public void SetBaseAction(BaseAction baseAction)
   {
      _baseAction = baseAction;
      _textMeshPro.text = baseAction.GetActionName().ToUpper();
      _button.onClick.AddListener(() =>
      {
         UnitActionSystem.Instance.SetSelectedAction(baseAction);
      });
   }

   public void UpdateSelectedVisual()
   {
      BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
      _selectedGameObject.SetActive(selectedBaseAction == _baseAction);
   }

}
