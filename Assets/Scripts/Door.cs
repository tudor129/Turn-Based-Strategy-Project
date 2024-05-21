using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool _isOpen;

    Animator _animator;
    Action _onInteractionComplete;
    GridPosition _gridPosition;
    bool _isActive;
    float _timer;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        
       
    }

    void Start()
    {
        
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);

        if (_isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _isActive = false;
            _onInteractionComplete();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        _onInteractionComplete = onInteractionComplete;
        _isActive = true;
        _timer = 0.5f;
        
        if (!_isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("IsOpen", _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, true);
    }
    void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("IsOpen", _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, false);
    }
}
