using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WarriorAnimsFREE;

public class WarriorCharacter : Character
{

    [SerializeField] public float firstActionDamage = 5;
    [SerializeField] private float secondActionDamage = 2;
    [SerializeField] private float secondActionDamageRadius = 2;
    [SerializeField] private float thirdActionDamage = 1;
    [SerializeField] private float thirdActionTime = 3; //in seconds
    [SerializeField] private float thirdActionDamageInterval = 0.5f; //in seconds
    [SerializeField] private float thirdActionSpeedMultiplier = 1.5f;
    [SerializeField] private AudioClip swordAudioClip;

    private Leap leapComponent;
    private WarriorController wc;
    private WarriorMovementController wmc;
    private SuperCharacterController scc;
    private AreaDamage areaDamage;
    private ParticleHolder particleHolder;
    private AudioSource audioSrc;

    protected override void AdditionnalAwake()
    {
        leapComponent = GetComponent<Leap>();
        wc = GetComponent<WarriorController>();
        wmc = GetComponent<WarriorMovementController>();
        scc = GetComponent<SuperCharacterController>();
        // normalRunSpeed = wmc.runSpeed;
        areaDamage = GetComponent<AreaDamage>();
        particleHolder = GetComponent<ParticleHolder>();
        audioSrc = GetComponent<AudioSource>();
    }

    public override bool CheckIsGrounded()
    {
        return wc.MaintainingGround();
    }

    protected override void SetMovementComponentsEnabled(bool state)
    {
        wc.enabled = state;
        wmc.enabled = state;
        scc.enabled = state;
        playerRigidbody.detectCollisions = state;
    }
    public override float GetSpeed()
    {
        return wmc.runSpeed;
    }

    public override void SetSpeed(float speed)
    {
        wmc.runSpeed = speed;
    }

    protected override bool RunFirstAction()
    {
        Debug.Log("First action");
        if (inputController.requestAttack())
        {
            Vector3 forward = playerRigidbody.transform.forward;
            // Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + playerRigidbody.transform.forward, Color.red, 5, false);
            GameObject[] damagedEnemies = areaDamage.DamageEnemiesInArea(playerRigidbody.position + (2 * forward), 1.5f, firstActionDamage);
            Debug.Log("WARRIOR: Damaged enemies: " + damagedEnemies.Length);
            audioSrc.PlayOneShot(swordAudioClip);
            foreach (GameObject damEnem in damagedEnemies)
            {
                NavMeshAgent navMesh = damEnem.GetComponentInParent<NavMeshAgent>();
                if (navMesh != null)
                {
                   // navMesh.Warp(damEnem.transform.position + forward);
                }
                else
                {
                    Debug.LogError("WARRIOR: Hit enemy: " + damEnem.name + ", nav mesh = null ");
                }
            }
            return true;
        }
        return false;
    }

    protected override bool RunSecondAction()
    {
        Debug.Log("Second action");
        if (leapComponent.doLeap(transform.forward))
        {
            returnToGroundEvent.AddListener(OnReturnToGround);
            return true;
        }

        return false;
    }
    private void OnReturnToGround()
    {
        Debug.Log("WARRIOR: return to ground after leap");
        GameObject[] damagedEnemies = areaDamage.DamageEnemiesInArea(playerRigidbody.position, secondActionDamageRadius, secondActionDamage);
        foreach (GameObject damEnem in damagedEnemies)
        {
            NavMeshAgent navMesh = damEnem.GetComponentInParent<NavMeshAgent>();
            if (navMesh != null)
            {
                Vector3 enemPos = damEnem.transform.position;
                Vector3 playerPos = playerRigidbody.position;
                playerPos.y = enemPos.y; // Stay in the nav mesh y
                navMesh.Warp(enemPos + (enemPos - playerPos) * 2);
            }
            else
            {
                Debug.LogError("WARRIOR: Hit enemy: " + damEnem.name + ", nav mesh = null ");
            }
        }
        particleHolder.PlayParticles("ShockWave");
        audioSrc.Play();

        returnToGroundEvent.RemoveListener(OnReturnToGround);
    }

    private void OnDisable()
    {
        returnToGroundEvent.RemoveListener(OnReturnToGround);
    }


    protected override bool RunThirdAction()
    {
        Debug.Log("Third action");
        StartSpinning();
        return true;
    }

    private void StartSpinning()
    {
        wc.SpinningAttack();
        SetSpeedCoefficient(thirdActionSpeedMultiplier);
        StartCoroutine(Spinning(thirdActionTime));
    }

    private IEnumerator Spinning(float seconds)
    {
        if (seconds <= 0)
        {
            StopSpinning();
        }
        else
        {
            areaDamage.DamageEnemiesInArea(playerRigidbody.position, 3f, thirdActionDamage);
            audioSrc.PlayOneShot(swordAudioClip);
            yield return new WaitForSeconds(thirdActionDamageInterval);
            yield return Spinning(seconds - thirdActionDamageInterval);
        }
    }

    private void StopSpinning()
    {
        SetSpeedCoefficient(1);
        wc.ResetAnimatorTriggerNumber(); //Stop spinning animation
    }

}
