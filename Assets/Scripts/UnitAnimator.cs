using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class UnitAnimator : MonoBehaviour
{
    Action OnProjectileBehaviourComplete;
    
    [SerializeField] Animator _animator;
    [SerializeField] Transform _projTrailPrefab;
    [SerializeField] Transform _projRedPrefab;
    [SerializeField] Transform _projStartPointTransform;
    [SerializeField] ProjectileType _typeOfProjectile;
    [SerializeField] Transform _rangeWeaponTransform;
    [SerializeField] Transform _meleeWeaponTransform;
   
    
    void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;  
        }
        if (TryGetComponent<MeleeAction>(out MeleeAction meleeAction))
        {
            meleeAction.OnMeleeActionStarted += MeleeAction_OnMeleeActionStarted;
            meleeAction.OnMeleeActionCompleted += MeleeAction_OnMeleeActionCompleted;
        }
    }

    void Start()
    {
        EquipRangeWeapon();
    }

    void MeleeAction_OnMeleeActionStarted(object sender, EventArgs e)
    {
        EquipMeleeWeapon();
        _animator.SetTrigger("MeleeHit");
    }
    void MeleeAction_OnMeleeActionCompleted(object sender, EventArgs e)
    {
        EquipRangeWeapon();   
    }
    
    void ShootAction_OnShoot(object sender, EventArgs e)
    {
        _animator.SetTrigger("Shoot");
    }

    void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsRunning", false);
    }
    void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsRunning", true);
    }

    void AnimationEvent()
    {
        ShootAction target = GetComponent<ShootAction>();
        InstantiateProjectilePrefab(target);
    }
    void InstantiateProjectilePrefab(ShootAction target)
    {
         Transform projectileTransform = Instantiate(GetProjectilePrefab(), _projStartPointTransform.position, Quaternion.identity);
         Projectile projectile = projectileTransform.GetComponent<Projectile>();

         Vector3 targetUnitShootAtPosition = target.GetTargetUnit().GetWorldPosition();

         targetUnitShootAtPosition.y = _projStartPointTransform.position.y / 2;
            
         projectile.Setup(targetUnitShootAtPosition);
        
    }
    
    Transform GetProjectilePrefab()
    {
        if (_typeOfProjectile == ProjectileType.ProjectileYellow)
        {
            return _projTrailPrefab;
        }
        else if (_typeOfProjectile == ProjectileType.ProjectileRed)
        {
            return _projRedPrefab;
        }
        return null;
    }

    void EquipMeleeWeapon()
    {
        _meleeWeaponTransform.gameObject.SetActive(true);
        _rangeWeaponTransform.gameObject.SetActive(false);
    }
    void EquipRangeWeapon()
    {
        _rangeWeaponTransform.gameObject.SetActive(true);
        _meleeWeaponTransform.gameObject.SetActive(false);
    }

    
}
