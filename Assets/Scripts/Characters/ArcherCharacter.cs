using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarriorAnimsFREE;

public class ArcherCharacter : Character
{

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject trapPrefab;

    [SerializeField] private float firstActionDamage = 10;
    [SerializeField] private float thirdActionTime = 3;
    [SerializeField] private float thirdActionSpeedMultiplier = 2f;
    [SerializeField] private AudioClip thirdActionAudioClip;

    private float firstActionInitialCooldown = 0;
    private WarriorController wc;
    private WarriorMovementController wmc;
    private SuperCharacterController scc;

    private AudioSource audioSrc;
    private ArcherAnimationsEvents animEvents;

    // private float normalRunSpeed = 0;

    protected override void AdditionnalAwake()
    {
        wc = GetComponent<WarriorController>();
        wmc = GetComponent<WarriorMovementController>();
        scc = GetComponent<SuperCharacterController>();
        // normalRunSpeed = wmc.runSpeed;
        firstActionInitialCooldown = firstActionCooldown;
        audioSrc = GetComponent<AudioSource>();
        animEvents = GetComponentInChildren<ArcherAnimationsEvents>();
        RegisterEventsListeners(animEvents);
    }

    protected override void SetMovementComponentsEnabled(bool state)
    {
        wc.enabled = state;
        wmc.enabled = state;
        scc.enabled = state;
        playerRigidbody.detectCollisions = state;
    }

    private void RegisterEventsListeners(ArcherAnimationsEvents animEvents)
    {
        animEvents.bowShootEvent.AddListener(OnBowShoot);
    }

    private void UnregisterEventsListeners(ArcherAnimationsEvents animEvents)
    {
        animEvents.bowShootEvent.RemoveListener(OnBowShoot);
    }

    public override bool CheckIsGrounded()
    {
        return wc.MaintainingGround();
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
        return inputController.requestAttack();
        //OnBowShoot will be fired by animation event
    }

    public void OnBowShoot()
    {
        GameObject arrowGO = Instantiate(arrowPrefab, transform.position + transform.forward + Vector3.up, Quaternion.identity);
        print("arrow lanched");
        Projectile proj = arrowGO.GetComponent<Projectile>();
        proj.damage = firstActionDamage;
        proj.Shoot(transform.forward, gameObject);
        audioSrc.Play();
    }

    protected override bool RunSecondAction()
    {
        Debug.Log("Second action");
        Instantiate(trapPrefab, transform.position, Quaternion.identity);
        return true;
    }

    protected override bool RunThirdAction()
    {
        Debug.Log("Third action");
        SetSpeedCoefficient(thirdActionSpeedMultiplier);
        firstActionCooldown = 0.5f;
        audioSrc.PlayOneShot(thirdActionAudioClip);
        StartCoroutine(ThirdActionEnd());
        return true;
    }

    private IEnumerator ThirdActionEnd()
    {
        yield return new WaitForSeconds(thirdActionTime);
        SetSpeedCoefficient(1);
        firstActionCooldown = firstActionInitialCooldown;
    }

    private void OnDestroy()
    {
        UnregisterEventsListeners(animEvents);
    }

}