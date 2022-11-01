using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float damage = 10;
    public float speed = 70;

    private int shooterId = 0;
    private bool hitSomething = false;

    protected Rigidbody rb;
    protected AudioSource audioSrc;
    protected ParticleHolder particleHolder;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSrc = GetComponent<AudioSource>();
        particleHolder = GetComponent<ParticleHolder>();
    }

    public void Shoot(Vector3 direction, GameObject shooter)
    {
        shooterId = shooter.GetInstanceID();

        transform.LookAt(transform.position + (Quaternion.Euler(0, 90, 0) * direction));
        rb.velocity = direction * speed;
    }

    protected virtual void OnCollision(GameObject hitObject)
    {
        rb.velocity = Vector3.zero; //Stops at hit, preventing hit particles from moving

        Health health = hitObject.GetComponentInParent<Health>();
        if (health != null)
        {
            Debug.Log("PROJ: hit entity with health: " + hitObject);
            health.LP -= damage;
        }
        else
        {
            Debug.Log("PROJ: hit entity without health: " + hitObject + ", destroy proj");
        }
        StartCoroutine(DestroyDelayedly(1));
        particleHolder.PlayParticles("HitEffect");
        audioSrc.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hitSomething && !"Pit".Equals(other.gameObject.tag) && !"Trap".Equals(other.gameObject.tag) && other.gameObject.GetInstanceID() != shooterId)
        {
            hitSomething = true;
            OnCollision(other.gameObject);
        }
    }

    protected IEnumerator DestroyDelayedly(float seconds)
    {
        GetComponent<Renderer>().enabled = false; // Make the projectile invisible
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

}
