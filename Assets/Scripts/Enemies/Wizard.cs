using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : MonoBehaviour
{
    //[SerializeField] GameObject target;
    NavMeshAgent navMeshAgent;
    Animator animator;
    [SerializeField] GameObject bolt;
    bool Islocated;
    LightningBoltScript lightning;
    private float timer;
    GameObject target;
    public static int boltDamage = 5;
    private Health health;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        if (!health.IsDead())
        {
            if (target != null)
            {
                if (navMeshAgent.velocity.magnitude > 0) { animator.SetBool("idle_normal", false); animator.SetBool("move_forward", true); animator.SetBool("attack", false); animator.SetBool("idle_combat", false); }
                else { animator.SetBool("move_forward", false); animator.SetBool("idle_normal", true); }

                //if (GameObject.FindGameObjectsWithTag("Player")!=null) { animator.SetBool("TargetLocated", true); }
                //else { animator.SetBool("TargetLocated", false); }



                if (navMeshAgent.remainingDistance < 9.3 && navMeshAgent.remainingDistance > 3.5)
                {
                    // timer += Time.deltaTime;
                    animator.SetBool("attack_short_001", false);
                    animator.SetBool("move_forward", false); animator.SetBool("idle_combat", true); animator.SetBool("idle_normal", false); animator.SetBool("attack", true);

                    //bolt.GE.Trigger();

                    //print("long");
                }
                else if (navMeshAgent.remainingDistance < 3.5) { /*print("short");*/ ResetLightning(); animator.SetBool("attack", false); animator.SetBool("attack_short_001", true); }
                else if (navMeshAgent.remainingDistance >= 9.3) { /*print("unreachable");*/ ResetLightning(); animator.SetBool("attack", false); }
            }
            else
            {

                target = GameObject.FindGameObjectWithTag("Player");
                if (target == null)
                {
                    Debug.LogError("Player not found");
                }
                ResetLightning(); animator.SetBool("attack", false); animator.SetBool("attack_short_001", false);
            }
        }

        //Chase();
    }

    private void BoltTrigger()
    {
        SoundManager.PlaySound("Magic Spell_Electricity Spell_1");

        bolt.GetComponent<LightningBoltScript>().Trigger();
        bolt.GetComponent<LightningBoltScript>().ManualMode = false;
        Damage();

    }
    private void ShortAttack() { SoundManager.PlaySound("Magic Spell_Simple Swoosh_6"); }
    void ResetLightning() { bolt.GetComponent<LightningBoltScript>().ManualMode = true; }

    //void Chase()

    //{
    //    navMeshAgent.SetDestination(target.transform.position);

    //}
    void Damage()
    {
        Health tHealth = target.GetComponentInParent<Health>();
        if (tHealth != null)
        {
            tHealth.LP -= boltDamage;
            print("boltdamage" + boltDamage);
        }
        else
        {
            Debug.LogError("WIZARD: health not found for " + target);
        }
    }
}
