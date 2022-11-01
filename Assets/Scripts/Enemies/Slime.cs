using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    GameObject target;
    GameObject pulse;
    private Health health;
    // [SerializeField] bool die;
    [SerializeField] GameObject smilePrefab;
    [SerializeField] GameObject mudPrefab;
    [SerializeField] GameObject attackStartPos;
    [SerializeField] float jumpForce = 2;

    Vector3 scale;
    int number = 0;


    // Start is called before the first frame update
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("SLI: Health not found");
        }
        // die = false;

        scale = transform.localScale;

        target = GameObject.FindGameObjectWithTag("Player");
        pulse = GameObject.FindGameObjectWithTag("pulse");
        // print(pulse.name);
        pulse.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.DrawLine(target.transform.position, transform.position, Color.green);
        if (!health.IsDead())
        {
            if (target != null)
            {
                if (navMeshAgent.remainingDistance < 5)
                {

                    animator.SetBool("Attack", true);
                }
                else
                {

                    animator.SetBool("Attack", false);
                }
            }
            else
            {

                target = GameObject.FindGameObjectWithTag("Player");
                if (target == null)
                {
                    Debug.LogError("Player not found");
                }
                animator.SetBool("Attack", false);
            }
        }
        // else { die = health.IsDead(); }

    }

    void Die() // Called by die animation
    {
        if (health.IsDead())
        {
            if (transform.localScale.x > 29.4)
            {
                while (number < 2)
                {
                    //pulse.SetActive(true);
                    // animator.SetBool("die", die);
                    smilePrefab.transform.localScale = scale * 0.7f;
                    GameObject slime = Instantiate(smilePrefab, transform.position, transform.rotation);
                    slime.transform.position += new Vector3(0, 0, number) * Time.deltaTime;
                    slime.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(jumpForce, 0, 0) * Vector3.up * jumpForce, ForceMode.Impulse);



                    number++;
                }
                Destroy(gameObject);
            }
            else if (transform.localScale.x < 29.4) { print("hmmmmmmmmmmmmmmmm");/*animator.SetBool("die", die);*/ Destroy(gameObject); }

        }
    }

    void Attack()
    {
        if (target != null)
        { GameObject mud = Instantiate(mudPrefab, attackStartPos.transform.position, transform.rotation); mud.GetComponent<Mud>().SetupTarget(target, transform); }
    }
}
