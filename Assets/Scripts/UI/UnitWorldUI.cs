using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI _actionPointsText;
   [SerializeField] Unit _unit;
   [SerializeField] Image _healthBarImage;
   [SerializeField] HealthSystem _healthSystem;


   void Start()
   {
      Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
      _healthSystem.OnDamaged += HealthSystem_OnDamaged;
      StartCoroutine(LazyUpdateActionPointsText());
      UpdateHealthBar();

   }
   void HealthSystem_OnDamaged(object sender, EventArgs e)
   {
      UpdateHealthBar();
   }
   void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
   {
      Unit unit = sender as Unit;
      //Debug.Log($"{unit}'s action points have changed.");
      UpdateActionPointsText();
   }

   void UpdateActionPointsText()
   {
      _actionPointsText.text = _unit.GetActionPoints().ToString();
      //Debug.Log("Points have been updated");
   }
   IEnumerator LazyUpdateActionPointsText()
   {
      yield return new WaitForSeconds(0f);
      UpdateActionPointsText();
   }

   void UpdateHealthBar()
   {
      _healthBarImage.fillAmount = _healthSystem.GetHealthNormalized();
   }
}
