using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
   void Start()
   {
      ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
      BombProjectile.OnAnyBombExploded += BombProjectile_OnAnyBombExploded;
      MeleeAction.OnAnyMeleeHit += MeleeAction_OnAnyMeleeHit;
   }
   void MeleeAction_OnAnyMeleeHit(object sender, EventArgs e)
   {
      ScreenShake.Instance.Shake(0.5f);
   }
   void BombProjectile_OnAnyBombExploded(object sender, EventArgs e)
   {
      ScreenShake.Instance.Shake(2.5f);
   }
   void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
   {
      ScreenShake.Instance.Shake(0.3f);
   }
}
