using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
   [SerializeField] Transform _ragdollRootBone;


   public void Setup(Transform originalRootBone)
   {
      MatchAllChildTransforms(originalRootBone, _ragdollRootBone);

      Vector3 ranomDir = new Vector3(Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f));
      ApplyExplosionToRagdoll(_ragdollRootBone, 500f, transform.position + ranomDir, 10f, -.1f);
   }

   void MatchAllChildTransforms(Transform root, Transform clone)
   {
      foreach (Transform child in root)
      {
         Transform cloneChild = clone.Find(child.name);
         if (cloneChild != null)
         {
            cloneChild.position = child.position;
            cloneChild.rotation = child.rotation;

            MatchAllChildTransforms(child, cloneChild);
         }
      } 
   }
   
   void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange, float upwardsModifier)
   {
      foreach (Transform child in root)
      {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRb))
            {
               childRb.AddExplosionForce(explosionForce, explosionPosition, explosionRange, upwardsModifier);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange, upwardsModifier);
      }
   }
}
