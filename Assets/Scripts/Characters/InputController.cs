using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    [HideInInspector] public bool inputAttack = false;
    [HideInInspector] public bool inputJump = false;
    [HideInInspector] public Vector3 inputJumpDirection = Vector3.zero;
    [HideInInspector] public Vector3 moveInput = Vector3.zero;

    [SerializeField] private Vector3 cameraPosition = new Vector3(0, 15, -6);
    [SerializeField] private float jumpCooldown = 0.3f;
    [SerializeField] private float attackCooldown = 0.3f;

    private float lastJumpTime = 0;
    private float lastAttackTime = 0;

    private Character charac;

    private void Awake()
    {
        charac = GetComponent<Character>();
    }

    private void Update()
    {
        Camera.main.transform.position = transform.position + cameraPosition;
    }

    public void CollectInputs()
    {
        moveInput = Vector3.zero;

        if (charac.IsGrounded())
        {
            if (Input.GetKey(KeyCode.Z))
            {
                moveInput = transform.forward;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                moveInput = -transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveInput = transform.right;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveInput = -transform.forward;
            }
        }

        if (inputJump && Time.time - lastJumpTime > jumpCooldown)
        {
            inputJump = false;
            Debug.Log("INPUT: jump input ended");
        }
        if (inputAttack && Time.time - lastAttackTime > attackCooldown)
        {
            inputAttack = false;
            Debug.Log("INPUT: attack input ended");
        }
    }

    public bool requestJump(Vector3 dir)
    {
        if (!inputJump)
        {
            Debug.Log("INPUT: new jump");
            lastJumpTime = Time.time;
            inputJump = true;
            inputJumpDirection = dir;
        }
        return inputJump;
    }

    public bool requestJump()
    {
        return requestJump(transform.forward);
    }

    public bool requestAttack()
    {
        if (!inputAttack)
        {
            Debug.Log("INPUT: new attack");
            lastAttackTime = Time.time;
            inputAttack = true;
        }
        return inputAttack;
    }

}
