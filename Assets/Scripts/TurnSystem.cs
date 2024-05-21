using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    int _turnNumber = 1;
    bool _isPlayerTurn = true;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

   

    public void NextTurn()
    { 
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;
        OnTurnChanged?.Invoke(this,EventArgs.Empty);
    }
    public int GetTurnNumber()
    {
        return _turnNumber;
    }
    public bool IsPlayerTurn()
    {
        return _isPlayerTurn;
    }
}
