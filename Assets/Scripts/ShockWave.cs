using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int radius;
    [SerializeField] int power;
    [SerializeField] private float maxDamage = 20;


    public void ShockWaveAttack()
    {
        //  print("wave");
        Vector3 shockWavePos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(shockWavePos, radius);
        foreach (Collider collider in colliders)
        {
            // print("ShockWave: col name " + collider.name);
            if ("Player".Equals(collider.tag))
            {
                Health health = collider.GetComponent<Health>();
                if (health == null)
                {
                    Debug.LogError("Player health compo not found");
                }
                else
                {
                    float dist = Vector3.Distance(shockWavePos, collider.transform.position);
                    float damage = Mathf.Max(0, maxDamage - dist);
                    health.LP -= damage;
                    Debug.Log("ShockWave: Distance center-hit: " + dist + ", damage: " + damage + ", plr health: " + health.LP);
                    collider.GetComponent<Leap>().doLeap((collider.transform.position - shockWavePos).normalized);
                }
                // if (collider.name == "2Handed Warrior" || collider.name== "Cube") 
                // {
                // collider.GetComponent<>
                //collider.GetComponent<Animator>().Play("Blown Away");
                //collider.gameObject.GetComponentInParent<Rigidbody>().AddExplosionForce(power, shockWavePos, radius, 10f);

            }
        }
    }
}
