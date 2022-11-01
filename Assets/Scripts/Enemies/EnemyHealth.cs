using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // [SerializeField] public int health;

    [SerializeField] private Health health;

    [SerializeField] Animator animator;
    [SerializeField] string dieAnimation;
    [SerializeField] string hitAnimation;
    [SerializeField] string tag;
    //    public bool dead;
    [SerializeField] private bool slime;
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject healthBarCanvas;
    [SerializeField] HealthBar healthBar;
    [SerializeField]bool boss;
    // Start is called before the first frame update
    private void Awake()
    {
        health = GetComponent<Health>();
        if (health != null)
        {
            healthBar.LinkToHealthComponent(health);
            healthBar.SetMaxHealth((int)health.LP);
            health.dieEvent.AddListener(OnDieListener);
            health.lifeChangeEvent.AddListener(OnLifeChange);
        }
        else
        {
            Debug.LogError("ENEMYHEALTH: health not found for " + gameObject);
        }
        // if (health <=0) { health = 5; }
    }
    void Start()
    {
        //animator = GetComponent<Animator>();
        // dead = false;
        // healthBar.SetMaxHealth((int)health.LP);
    }

    // Update is called once per frame
    // void Update()
    void OnDieListener()
    {
        // if (health <= 0)
        // {
        // dead = true;
        if (dieAnimation != "")
        {
            animator.Play(dieAnimation);
        }
        else if (slime)
        {
            print("slime died");
            animator.SetBool("die", true);
        }
        else if (dieAnimation == "")
        {
            Destroy(); print("destroyed");
        }

        // }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        health.dieEvent.RemoveListener(OnDieListener);
        health.lifeChangeEvent.RemoveListener(OnLifeChange);
    }

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.tag == tag && !health.IsDead())
    //    {
    //        print(other.tag);
    //        healthBarCanvas.SetActive(true);
    //        //health.LP -= other.GetComponent<HitBox>().damage;
    //        // healthBar.SetHealth((int)health.LP); linked to health compo in awake
    //        DamageIndicater indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicater>();
    //        indicator.SetDamageText((int)other.GetComponent<WarriorCharacter>().firstActionDamage);
    //        if (hitAnimation != null && !health.IsDead()) { animator.Play(hitAnimation); }
    //       // return;
    //    }

    //}


    public void OnLifeChange(float previousLp)
    {
        float damage = previousLp - health.LP;
        if (damage > 0)
        {
            DisplayHit(damage);
        }
    }

    public void DisplayHit(float damage)
    {
        Debug.Log("ENEMYHEALTH: Display hit dmg: " + damage + " for " + gameObject);
        if (!boss) { healthBarCanvas.SetActive(false); healthBarCanvas.SetActive(true); }
        DamageIndicater indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicater>();
        indicator.SetDamageText((int)damage);
        if (hitAnimation != null && !health.IsDead()) { animator.Play(hitAnimation); }
    }

}
