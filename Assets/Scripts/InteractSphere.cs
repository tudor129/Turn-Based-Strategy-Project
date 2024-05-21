using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
   [SerializeField] Material _greenMaterial;
   [SerializeField] Material _redMaterial;
   [SerializeField] MeshRenderer _meshRenderer;

   GridPosition _gridPosition; 
   Action _onInteractionComplete;
   bool _isActive;
   float _timer;
   
   bool _isGreen;
   void Start()
   {
      _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
      LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
      
      SetColorGreen();
   }

   void Update()
   {
      if (!_isActive)
      {
         return;
      }
      _timer -= Time.deltaTime;

      if (_timer <= 0f) 
      {
         _isActive = false;
         if (_onInteractionComplete != null)
         {
            _onInteractionComplete();
         }
      }
   }

   void SetColorGreen()
   {
      _isGreen = true;
      _meshRenderer.material = _greenMaterial;
   }
   void SetColorRed()
   {
      _isGreen = false;
      _meshRenderer.material = _redMaterial;
   }
   public void Interact(Action onInteractionComplete)
   {
      _onInteractionComplete = onInteractionComplete;
      _isActive = true;
      _timer = 0.5f;
      
      if (_isGreen)
      {
         SetColorRed();
      }
      else
      {
         SetColorGreen();
      }
   }
}
