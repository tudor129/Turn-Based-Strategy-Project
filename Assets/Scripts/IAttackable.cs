using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void ReceiveAttack(Action onAttackComplete);
    Vector3 GetWorldPosition();
}
