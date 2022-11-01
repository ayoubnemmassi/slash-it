using UnityEngine;
using System.Collections;

public class Goblin_ro_ctrl : MonoBehaviour
{

    private Animator anim;
    private UnityEngine.CharacterController controller;
    private InputController inputCtrl;


    private Character charac;

    private bool battle_state;
    public float speed = 6.0f;
    public float runSpeed = 1.7f;
    public float turnSpeed = 60.0f;
    public float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] private float jumpPower = 8;



    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<UnityEngine.CharacterController>();
        inputCtrl = GetComponent<InputController>();
        charac = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) //battle_idle
        {
            anim.SetInteger("battle", 1);
            battle_state = true;

        }
        if (Input.GetKey(KeyCode.E))          //idle
        {
            anim.SetInteger("battle", 0);
            battle_state = false;
        }

        inputCtrl.CollectInputs(); // For movement, but also jump and attack
        if (controller.isGrounded)
        {
            moveDirection = inputCtrl.moveInput;

            // if (controller.isGrounded)
            // {
            // moveDirection = Vector3.zero;
            // if (Input.GetKey(KeyCode.Z))
            // {
            //     moveDirection = transform.forward;
            // }
            // if (Input.GetKey(KeyCode.Q))
            // {
            //     moveDirection = -transform.right;
            // }
            // if (Input.GetKey(KeyCode.D))
            // {
            //     moveDirection = transform.right;
            // }
            // if (Input.GetKey(KeyCode.S))
            // {
            //     moveDirection = -transform.forward;
            // }

            if (moveDirection != Vector3.zero)
            {
                moveDirection *= speed * runSpeed;
                if (battle_state == false)
                {
                    anim.SetInteger("moving", 1);//walk
                                                 // runSpeed = 1.0f;
                }
                else
                {
                    anim.SetInteger("moving", 2);//run
                                                 // runSpeed = 2.6f;
                }
            }
            else
            {
                anim.SetInteger("moving", 0);
            }
        }


        if (Input.GetKeyDown("m")) //defence_start
        {
            anim.SetInteger("moving", 6);
        }



        if (Input.GetKeyDown("p")) // defence_start
        {
            anim.SetInteger("moving", 8);
        }
        if (Input.GetKeyUp("p")) // defence_end
        {
            anim.SetInteger("moving", 9);
        }


        if (Input.GetMouseButtonUp(0)) //attack
        {
            anim.SetInteger("moving", 3);
        }
        if (Input.GetMouseButtonUp(1)) //alt attack1
        {
            anim.SetInteger("moving", 4);
        }
        if (Input.GetMouseButtonUp(2)) //alt attack2
        {
            anim.SetInteger("moving", 5);
        }

        // if (Input.GetKeyDown(KeyCode.Space)) //jump
        // {
        //     anim.SetInteger("moving", 7);
        // }
        if (inputCtrl.inputJump) //jump
        {
            moveDirection = inputCtrl.inputJumpDirection;
            if (moveDirection == Vector3.zero)
            {
                moveDirection = -transform.forward;
            }
            moveDirection.y = 1;
            moveDirection *= jumpPower;
            anim.SetInteger("moving", 7);
        }


        if (Input.GetKeyDown("o")) //die_1
        {
            anim.SetInteger("moving", 12);
        }
        if (Input.GetKeyDown("i")) //die_2
        {
            anim.SetInteger("moving", 13);
        }

        if (Input.GetKeyDown("u")) //defence
        {
            int n = Random.Range(0, 2);
            if (n == 0)
            {
                anim.SetInteger("moving", 10);
            }
            else { anim.SetInteger("moving", 11); }
        }


        // float turn = Input.GetAxis("Horizontal");
        // transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        charac.RotateTowardsMouse();
        controller.Move(moveDirection * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;
        // Camera.main.transform.position = transform.position + new Vector3(0, 15, -6);
    }

}

