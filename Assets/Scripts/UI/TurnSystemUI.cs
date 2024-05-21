using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button _endTurnButton;
    [SerializeField] TextMeshProUGUI _turnNumberText;
    [SerializeField] GameObject _enemyTurnVisualGameObject;

    
    
    void Start()
    {
        TurnSystem.Instance.OnTurnChanged += Instance_OnTurnChanged;
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });
        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }
    void Instance_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }
    void UpdateTurnNumber()
    {
        _turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }
    void UpdateEnemyTurnVisual()
    {
        _enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    void UpdateEndTurnButtonVisibility()
    {
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

    
}
