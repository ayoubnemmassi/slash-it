using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteProjectile : Projectile
{

    [SerializeField] private float explosionRadius = 3;
    [SerializeField] private float explodeDelay = 1;

    private void EnablePhysicCollisions()
    {
        Collider col = gameObject.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = false;
        }
        else
        {
            Debug.LogError("DYNAMITE: Collider not found");
        }
    }
    protected override void OnCollision(GameObject hitObject)
    {
        EnablePhysicCollisions();
        StartCoroutine(ExplodeDelayedly(explodeDelay));
    }

    private void Explode()
    {
        GetComponent<AreaDamage>().DamageEnemiesInArea(transform.position, explosionRadius, damage);
        audioSrc.Play();
        particleHolder.PlayParticles("Explosion");
    }

    private IEnumerator ExplodeDelayedly(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        particleHolder.StopParticles("Smoke");
        Explode();
        yield return DestroyDelayedly(1);
    }

}
