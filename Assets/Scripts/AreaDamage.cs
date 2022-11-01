using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{

    public GameObject[] DamageEnemiesInArea(Vector3 center, float radius, float damage)
    {
        List<GameObject> enemies = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(center, radius);
        foreach (Collider col in colliders)
        {
            if ("Enemy".Equals(col.tag) || (col.transform.parent != null && "Enemy".Equals(col.transform.parent.tag)))
            {
                Health health = col.GetComponentInParent<Health>();
                // EnemyHealth enemyHealth = col.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    Debug.LogError("### Hit enemy: " + col.transform.name + ", lp: " + health.LP);
                    // enemyHealth.GetHit(damage);
                    health.LP -= damage;
                    enemies.Add(col.gameObject);
                }
                else
                {
                    Debug.LogError("### Hit enemy " + col.transform.name + ", Health component = null");
                }
            }
            // else
            // {
            //     Debug.LogError("### Hit not enemy: " + col.transform.name + ", " + col.transform.position);
            // }
        }
        // Debug.LogError("### SWORD hits: " + colliders.Length);

        // RaycastHit[] hits;
        // // Cast character controller shape 10 meters forward, to see if it is about to hit anything
        // hits = Physics.SphereCastAll(playerRigidbody.position, 1f, playerRigidbody.transform.forward, 3);

        // // Change the material of all hit colliders
        // // to use a transparent Shader
        // for (int i = 0; i < hits.Length; i++)
        // {
        //     RaycastHit hit = hits[i];
        //     if ("Enemy".Equals(hit.transform.tag) || (hit.transform.parent != null && "Enemy".Equals(hit.transform.parent.tag)))
        //     {

        //         Debug.LogError("Hit enemy: " + hit.transform.name);
        //     }
        //     else
        //     {
        //         Debug.LogError("Hit not enemy: " + hit.transform.name + ", " + hit.transform.position);
        //     }
        // }
        // Debug.LogError("SWORD hits: " + hits.Length);
        return enemies.ToArray();
    }

}
