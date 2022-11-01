using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rhino : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    GameObject target;
    private Health health;

    public bool Shout;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.IsDead())
        {
            if (target != null)
            {
                print(navMeshAgent.remainingDistance);
                if (navMeshAgent.remainingDistance < 5.2) { animator.SetBool("Attack", true); }
                else { animator.SetBool("Attack", false); }
            }
            else { animator.SetBool("Attack", false); }
        }

        if (Shout)
        {

        }
    }
}
