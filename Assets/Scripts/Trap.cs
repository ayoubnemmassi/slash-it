using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trap : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if ("Enemy".Equals(other.gameObject.tag))
        {
            GameObject enemy = other.gameObject;
            NavMeshAgent navMesh = enemy.GetComponentInParent<NavMeshAgent>();
            if (navMesh != null)
            {
                navMesh.speed = 0; // Freeze
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("TRAP: enemy: " + enemy.name + ", nav mesh = null ");
            }
        }
    }

}
