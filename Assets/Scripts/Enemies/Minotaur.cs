using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minotaur : MonoBehaviour
{

    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public GameObject target;

    private Health health;
    private float coolDown;
    private float timer;
    [SerializeField] int rotationSpeed;
    private float timer2;
    private Transform myTransform;
    private Transform targetTransform;
    EnemyHealth enemyHealth;
    [SerializeField] float cooldown = 5;
    [SerializeField] float velocity;
    private bool lookfor = true;
    private bool firsttime = true;
    private bool inChase;

    public ShockWave shockWave;
    public ParticleHolder particleHolder;
    private MinotaurBehaviour[] behaviours;
    private MinotaurBehaviour behaviour;
    private int behaviourIndex = 1;


    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("MINAU: Health not found");
        }
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        myTransform = transform;

        shockWave = GetComponent<ShockWave>();
        particleHolder = GetComponent<ParticleHolder>();
        health.lifeChangeEvent.AddListener(OnLifeChangeListener);
        UpdateBehaviour();
        // behaviours = new MinotaurBehaviour[3];
        // behaviours.SetValue(new MinotaurB1(), 0);
        // behaviours.SetValue(new MinotaurB2(), 1);
        // behaviours.SetValue(new MinotaurB3(), 2);
    }

    void UpdateBehaviour()
    {
        float lifeRatio = health.GetLPRatio();
        int prevBeIndex = behaviourIndex;
        if (lifeRatio < 0.25f)
        {
            behaviourIndex = 3;
        }
        else if (lifeRatio < 0.5f)
        {
            behaviourIndex = 2;
        }
        else
        {
            behaviourIndex = 1;
        }

        if (prevBeIndex != behaviourIndex || behaviour == null)
        {
            if (behaviourIndex == 3)
            {
                behaviour = new MinotaurB3(this);
            }
            else if (behaviourIndex == 2)
            {
                behaviour = new MinotaurB2(this);
            }
            else
            {
                behaviour = new MinotaurB1(this);
            }
        }
        Debug.LogError("MIN: behaviour index: " + behaviourIndex);
    }

    void OnLifeChangeListener(float previousLp)
    {
        UpdateBehaviour();
    }

    // Update is called once per frame
    void Update()
    {

        // timer = Time.deltaTime;
        // timer2 = Time.deltaTime;
        // cooldown -= timer;

        //if ((enemyHealth.health * 100) / maxHealth <= 100 && (enemyHealth.health * 100) / maxHealth > 50) { FirstBehaviour(); }
        //else if ((enemyHealth.health * 100) / maxHealth <= 50 && (enemyHealth.health * 100) / maxHealth > 25) { SecondBehaviour(); }
        //else if ((enemyHealth.health * 100) / maxHealth <= 25) { ThirdBehaviour(); }
        // if (!health.IsDead())
        // {
        //     if (cooldown <= 0) { lookfor = false; }
        //     animator.SetFloat("ForwardSpeed", navMeshAgent.velocity.magnitude);
        //     if (target != null && lookfor)
        //     {
        //         inChase = false;
        //         myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.transform.position - myTransform.position), rotationSpeed * Time.deltaTime);
        //         targetTransform = target.transform;
        //     }
        //     else if (!lookfor && navMeshAgent.velocity.magnitude < 0.5) { Chase(targetTransform); }

        //     if (navMeshAgent.velocity.magnitude <= 0 && inChase) { cooldown = 5; timer = 0; lookfor = true; }

        // }
        // velocity = navMeshAgent.velocity.magnitude;

        behaviour.RunAction();

    }

    void FirstBehaviour()
    {

        if (!health.IsDead())
        {
            if (timer > 10) { timer = 0; animator.SetBool("Attack 2", true); }
            else
            {
                if (target != null)
                {
                    // print(navMeshAgent.remainingDistance);
                    if (navMeshAgent.remainingDistance < 5.2) { animator.SetBool("Attack", true); }
                    else { animator.SetBool("Attack", false); }
                }
                else { animator.SetBool("Attack", false); }
            }
        }

    }
    void SecondBehaviour()
    {

        if (!health.IsDead())
        {
            coolDown -= timer;
            if (coolDown <= 0)
            {
                coolDown = 5; timer = 0; if (target != null)
                {
                    print(navMeshAgent.remainingDistance);
                    if (navMeshAgent.remainingDistance < 5.2) { animator.SetBool("Attack 2", true); }
                    else { animator.SetBool("Attack 2", false); }
                }
                else { animator.SetBool("Attack 2", false); }
            }
            else if (timer >= 15 && coolDown > 0)
            {
                timer = 0;
                animator.SetBool("Attack 3", true);
            }
        }
    }
    void ThirdBehaviour() { }

    void Chase(Transform transform)

    {
        inChase = true;
        firsttime = false;
        if (target != null) { navMeshAgent.SetDestination(transform.position); }

        // GetComponent<Rigidbody>().AddForce(  transform.forward * throwForce, ForceMode.Impulse);

    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.lifeChangeEvent.RemoveListener(OnLifeChangeListener);
        }
    }
}

public abstract class MinotaurBehaviour
{
    public Minotaur minotaur;

    public float firstActionCooldown;
    public float secondActionCooldown;
    private float firstActionLastTime = 0;
    private float secondActionLastTime = 0;

    public MinotaurBehaviour(float firstActionCooldown, float secondActionCooldown, Minotaur minotaur)
    {
        this.firstActionCooldown = firstActionCooldown;
        this.secondActionCooldown = secondActionCooldown;
        this.minotaur = minotaur;
    }

    bool IsCooldownFinished(float lastTime, float cooldown)
    {
        return Time.time - lastTime >= cooldown;
    }

    public void RunAction()
    {
        if (IsCooldownFinished(firstActionLastTime, firstActionCooldown))
        {
            Debug.Log("MIN: first act");
            if (RunFirstAction())
            {
                firstActionLastTime = Time.time;
            }
        }
        else if (IsCooldownFinished(secondActionLastTime, secondActionCooldown))
        {
            Debug.Log("MIN: second act");
            if (RunSecondAction())
            {
                secondActionLastTime = Time.time;
            }
        }
    }

    public abstract bool RunFirstAction();
    public abstract bool RunSecondAction();
}

public class MinotaurB1 : MinotaurBehaviour
{

    public MinotaurB1(Minotaur minotaur) : base(5, 10, minotaur)
    {
    }

    public override bool RunFirstAction()
    {
        minotaur.animator.Play("Jump");
        Debug.LogError("MIN: ShockWave attack");
        minotaur.shockWave.ShockWaveAttack();
        
        minotaur.particleHolder.PlayParticles("ShockWave");
        return true;
    }

    public override bool RunSecondAction()
    {
        if (minotaur.target != null)
        {
            // print(navMeshAgent.remainingDistance);
            if (minotaur.navMeshAgent.remainingDistance < 5.2) { minotaur.animator.SetBool("Attack", true); }
            else { minotaur.animator.SetBool("Attack", false); }
        }
        else { minotaur.animator.SetBool("Attack", false); }
        return true;
    }
}

public class MinotaurB2 : MinotaurBehaviour
{
    public MinotaurB2(Minotaur minotaur) : base(15, 5, minotaur)
    {
    }

    public override bool RunFirstAction()
    {
        return true;
    }

    public override bool RunSecondAction()
    {
        return true;
    }
}

public class MinotaurB3 : MinotaurBehaviour
{
    public MinotaurB3(Minotaur minotaur) : base(0, 10, minotaur)
    {
    }
    public override bool RunFirstAction()
    {
        return true;
    }

    public override bool RunSecondAction()
    {
        return true;
    }
}
