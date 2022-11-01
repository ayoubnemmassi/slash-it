using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinCharacter : Character
{

    [SerializeField] private GameObject pebblePrefab;
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private float firstActionDamage = 5;
    [SerializeField] private float secondActionDamage = 10;

    private Leap leapComponent;
    private UnityEngine.CharacterController charc;
    private Goblin_ro_ctrl goblin_Ro_Ctrl;
    private AudioSource audioSrc;

    private float distCenterFeet = 1;

    protected override void AdditionnalAwake()
    {
        charc = GetComponent<UnityEngine.CharacterController>();
        goblin_Ro_Ctrl = GetComponent<Goblin_ro_ctrl>();
        distCenterFeet = charc.center.y;
        leapComponent = GetComponent<Leap>();
        audioSrc = GetComponent<AudioSource>();
    }

    protected override void SetMovementComponentsEnabled(bool state)
    {
        charc.enabled = state;
        goblin_Ro_Ctrl.enabled = state;
    }

    public override bool CheckIsGrounded()
    {
        // RaycastHit hit;
        // bool raycast = Physics.Raycast(transform.position, Vector3.down, out hit, distCenterFeet + 0.1f);
        // Debug.LogError("check is grounded: " + raycast + ", " + hit.transform);
        float skinWidthRadius = charc.skinWidth / 2;
        return Physics.Raycast(transform.position + (skinWidthRadius * transform.forward), Vector3.down, distCenterFeet + 0.1f)
        || Physics.Raycast(transform.position + (skinWidthRadius * -transform.forward), Vector3.down, distCenterFeet + 0.1f)
        || Physics.Raycast(transform.position + (skinWidthRadius * transform.right), Vector3.down, distCenterFeet + 0.1f) || Physics.Raycast(transform.position + (skinWidthRadius * -transform.right), Vector3.down, distCenterFeet + 0.1f);
        // RaycastHit hit;
        // return Physics.SphereCast(transform.position, charc.skinWidth / 1.5f, Vector3.down, out hit, distCenterFeet + 0.3f);
    }
    public override float GetSpeed()
    {
        return goblin_Ro_Ctrl.speed;
    }

    public override void SetSpeed(float speed)
    {
        goblin_Ro_Ctrl.speed = speed;
    }

    protected override bool RunFirstAction()
    {
        Debug.Log("First action");
        GameObject pebbleGO = Instantiate(pebblePrefab, transform.position + transform.forward + Vector3.up, Quaternion.identity);
        Projectile proj = pebbleGO.GetComponent<Projectile>();
        proj.damage = firstActionDamage;
        proj.Shoot(transform.forward, gameObject);
        return true;
    }
    protected override bool RunSecondAction()
    {
        Debug.Log("Second action");
        GameObject dynamiteGO = Instantiate(dynamitePrefab, transform.position + transform.forward + Vector3.up, Quaternion.identity);
        Projectile proj = dynamiteGO.GetComponent<Projectile>();
        proj.damage = secondActionDamage;
        proj.Shoot(transform.forward, gameObject);
        return true;
    }

    protected override bool RunThirdAction()
    {
        Debug.Log("Third action");
        if (leapComponent.doLeap())
        {
            audioSrc.Play();
            return true;
        }
        else
        {
            return false;
        }
    }

}
