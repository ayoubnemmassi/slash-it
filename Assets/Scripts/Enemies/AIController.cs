using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    [SerializeField] GameObject target;
    NavMeshAgent navMeshAgent;
    Animator animator;
    bool Islocated;
    [SerializeField] int rotationSpeed;
    private Transform myTransform;
    private Transform targetTransform;
    float timer;
    [SerializeField] float cooldown = 15;
    [SerializeField] int throwForce = 10;
    [SerializeField] bool stop;
    private bool targetfixed;

    private Health health;

    void Start()
    {
        myTransform = transform;
        if (target == null) { target = GameObject.FindGameObjectWithTag("Player"); }
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (target != null)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.transform.position - myTransform.position), rotationSpeed * Time.deltaTime);
        }
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("AICONT: Health not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //timer = Time.deltaTime;
        //cooldown -= timer;
        //if (cooldown <= 0 ) { cooldown = 15; timer = 0; while (stop||timer<3) { Chase(); } targetfixed = true; }

        //if (GameObject.FindGameObjectsWithTag("Player")!=null) { animator.SetBool("TargetLocated", true); }
        //else { animator.SetBool("TargetLocated", false); }
        // print("rdistance"+ navMeshAgent.remainingDistance);
        if (!health.IsDead())
        {
            if (target != null)
            {
                animator.SetFloat("ForwardSpeed", navMeshAgent.velocity.magnitude);
                if (target != null && cooldown > 0)
                {

                    myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.transform.position - myTransform.position), rotationSpeed * Time.deltaTime);
                }

                Chase();
            }
            else
            {
                target = GameObject.FindGameObjectWithTag("Player");
                if (target == null)
                {
                    Debug.LogError("Player not found");
                }
            }

        }

    }
    void Chase()

    {
        if (target != null) { navMeshAgent.SetDestination(target.transform.position); }
        // GetComponent<Rigidbody>().AddForce(  transform.forward * throwForce, ForceMode.Impulse);

    }

    void search() { }


}
