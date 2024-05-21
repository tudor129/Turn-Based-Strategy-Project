using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour 
{
    
    
    [SerializeField] Transform _ragdollPrefab;
    [SerializeField] Transform _originalRootBone;
    [SerializeField] Transform _projRedPrefab;
    [SerializeField] Transform _projYellowPrefab;
    

    [SerializeField] Projectile _projectile;
    HealthSystem _healthSystem;

    void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(_ragdollPrefab, transform.position, Quaternion.identity);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_originalRootBone);
        // Projectile targetName = GetComponent<Projectile>();
        // targetName.ReturnTargetChildName();
        // Debug.Log(targetName.ReturnTargetChildName());

    }
    
   
}
