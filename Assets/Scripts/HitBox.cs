using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] string tag;
    [SerializeField] public int damage;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if ("Player".Equals(other.tag))
        {
            Health health = other.GetComponentInParent<Health>();
            if (health != null)
            {
                health.LP -= damage;
            }
            else
            {
                Debug.LogError("HITBOX: " + other + " health compo not found");
            }
        }
    }
}
