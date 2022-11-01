using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    NavMeshAgent navMeshAgent;
    GameObject target;
    private Health health;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("WIZ: Health not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //  print(navMeshAgent.remainingDistance);
        if (!health.IsDead())
        {
            if (target != null)
            {
                if (navMeshAgent.remainingDistance < 2.7) { animator.SetBool("TargetLocated", true); }
                else { animator.SetBool("TargetLocated", false); }
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player");
                if (target == null)
                {
                    Debug.LogError("Player not found");
                }
                animator.SetBool("TargetLocated", false);
            }
        }

    }

    private void SwiningSwordSound() { SoundManager.PlaySound("Warrior_SwingingSword_V1"); }
}
