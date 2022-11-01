using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingGround : MonoBehaviour
{
    private void ProcessCollision(GameObject go)
    {
        Health health = go.GetComponent<Health>();
        if (health != null)
        {
            health.LP = 0;
            Debug.Log("KILLINGGROUND: " + go + " killed by ground");
        }
        else
        {
            Debug.Log("KILLINGGROUND: " + go + " health compo not found");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        ProcessCollision(other.gameObject);
    }

    void OnCollisionStay(Collision other)
    {
        ProcessCollision(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        ProcessCollision(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessCollision(other.gameObject);
    }
}
