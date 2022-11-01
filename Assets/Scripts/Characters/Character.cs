using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{

    //Max depenetration: 200 --> 500
    // Bounce threshold: 2
    // Def contact offset: 0.0001
    // Warrior capsule collider radius: 0.75 --> 0.6

    [SerializeField] private LayerMask mouseOrientationLayers;

    // private NavMeshAgent navMeshAgent;
    protected Rigidbody playerRigidbody;
    private Camera playerCamera;

    // private float distanceCenterFeet = 0;

    protected PitsManager pitsManager;
    protected InputController inputController;
    protected Image[] uiActions;

    private float previousY = 0;
    private bool isGrounded = true;
    private float normalRunSpeed = 0;

    private float slowDownEndTime = 0;


    public UnityEvent returnToGroundEvent = new UnityEvent();

    private float firstActionLastTime = 0;
    private float secondActionLastTime = 0;
    private float thirdActionLastTime = 0;
    public float firstActionCooldown = 0;
    public float secondActionCooldown = 0;
    public float thirdActionCooldown = 0;

    protected abstract bool RunFirstAction();
    protected abstract bool RunSecondAction();
    protected abstract bool RunThirdAction();

    void Awake()
    {
        // navMeshAgent = GetComponentInParent<NavMeshAgent>();
        playerRigidbody = GetComponentInChildren<Rigidbody>();
        playerCamera = Camera.main;
        pitsManager = FindObjectOfType<PitsManager>();
        inputController = GetComponentInChildren<InputController>();
        // returnToGroundEvent = new UnityEvent();

        GameObject healthBarGO = GameObject.FindGameObjectWithTag("healthbar");
        if (healthBarGO != null)
        {
            HealthBar healthBar = healthBarGO.GetComponent<HealthBar>();
            Health health = GetComponent<Health>();
            if (healthBar != null)
            {
                if (health != null)
                {
                    healthBar.LinkToHealthComponent(health);
                    Debug.Log("CHARAC healtbar: " + health.GetInstanceID() + ", lp: " + health.LP);
                }
                else
                {
                    Debug.LogError("CHARAC: Health not found");
                }
            }
            else
            {
                Debug.LogError("CHARAC: Healthbar not found");
            }
        }
        else
        {
            Debug.LogError("CHARAC: Healthbar not found");
        }

        InitUIActions();

        AdditionnalAwake();
        // Important: must be after AdditionnalAwake:
        normalRunSpeed = GetSpeed();
    }

    public string GetName()
    {
        return gameObject.name.Substring(0, gameObject.name.IndexOf("C"));
    }

    void InitUIActions()
    {
        string characName = GetName();
        uiActions = new Image[3];
        GameObject uiActionsHolder = GameObject.FindGameObjectWithTag("UIActionsHolder");
        if (uiActionsHolder != null)
        {
            for (int i = 1; i <= 3; i++)
            {
                Transform actionItem = uiActionsHolder.transform.Find("A" + i);
                if (actionItem != null)
                {
                    uiActions[i - 1] = actionItem.GetComponent<Image>();
                    string spriteName = characName + "A" + i;
                    Sprite actionSprite = Resources.Load<Sprite>("CharactersActions/" + spriteName);
                    if (actionSprite != null)
                    {
                        uiActions[i - 1].sprite = actionSprite;
                    }
                    else
                    {
                        Debug.LogError("Sprite of Action A" + i + " not found (" + spriteName + ")");
                    }
                }
                else
                {
                    Debug.LogError("Action A" + i + " not found in Actions UI holder");
                }
            }
        }
        else
        {
            Debug.LogError("UIActionsHolder not found");
        }
    }

    protected virtual void AdditionnalAwake()
    {
    }

    protected virtual void AdditionnalUpdate()
    {
    }

    public abstract float GetSpeed();

    public abstract void SetSpeed(float speed);

    public void SetSpeedCoefficient(float coef)
    {
        SetSpeed(normalRunSpeed * coef);
    }

    public void SetSlowDown(float time, float speedCoef)
    {
        slowDownEndTime = Time.time + time;
        SetSpeedCoefficient(speedCoef);
    }

    bool IsCooldownFinished(float lastTime, float cooldown)
    {
        return Time.time - lastTime >= cooldown;
    }

    void SetUIActionDisplay(Image img, float alpha)
    {
        if (alpha > 1)
        {
            alpha = 1;
        }
        else if (alpha < 0)
        {
            alpha = 0;
        }
        var tempColor = img.color;
        tempColor.a = alpha;
        img.color = tempColor;
    }

    void UpdateUIActionCooldownDisplay(int index, float lastTime, float cooldown)
    {
        float alpha = (Time.time - lastTime) / cooldown;
        SetUIActionDisplay(uiActions[index], alpha);
    }

    void Update()
    {
        UpdateUIActionCooldownDisplay(0, firstActionLastTime, firstActionCooldown);
        UpdateUIActionCooldownDisplay(1, secondActionLastTime, secondActionCooldown);
        UpdateUIActionCooldownDisplay(2, thirdActionLastTime, thirdActionCooldown);
        if (Input.GetMouseButtonUp(0) && IsCooldownFinished(firstActionLastTime, firstActionCooldown))
        {
            if (RunFirstAction())
            {
                firstActionLastTime = Time.time;
            }
        }
        if (Input.GetMouseButtonUp(1) && IsCooldownFinished(secondActionLastTime, secondActionCooldown))
        {
            if (RunSecondAction())
            {
                secondActionLastTime = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsCooldownFinished(thirdActionLastTime, thirdActionCooldown))
        {
            if (RunThirdAction())
            {
                thirdActionLastTime = Time.time;
            }
        }

        if (slowDownEndTime > 0 && Time.time >= slowDownEndTime)
        {
            SetSpeedCoefficient(1);
            slowDownEndTime = 0;
        }

        // bool movementOrder = false;
        // if (Input.GetKey(KeyCode.Z))
        // {
        //     navMeshAgent.velocity += transform.forward * navMeshAgent.speed;
        //     movementOrder = true;
        // }
        // if (Input.GetKey(KeyCode.Q))
        // {
        //     navMeshAgent.velocity += (Quaternion.Euler(0, -90, 0) * transform.forward) * navMeshAgent.speed;
        //     movementOrder = true;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     navMeshAgent.velocity += (Quaternion.Euler(0, 90, 0) * transform.forward) * navMeshAgent.speed;
        //     movementOrder = true;
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     navMeshAgent.velocity += (-transform.forward) * navMeshAgent.speed;
        //     movementOrder = true;
        // }
        // if (!movementOrder)
        // {
        //     navMeshAgent.velocity = Vector3.zero;
        // }

        Debug.DrawLine(transform.position, transform.position + 4 * transform.forward, Color.blue);
        Debug.DrawLine(transform.position, transform.position + 4 * (Quaternion.Euler(0, 80, 0) * transform.forward), Color.magenta);

        // Warning: Contact offset in Project Settings > Physics has been changed from 0.01 to 0.0001 in order to fix "invisible walls" issue induced by the several amount of colliders of the floor (one per cell)
        // pitsManager.setPitsPassable(!isGrounded);
        // if (isGrounded)
        // {
        //     if (Input.GetKey(KeyCode.Z))
        //     {
        //         playerRigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
        //     }
        //     if (Input.GetKey(KeyCode.Q))
        //     {
        //         playerRigidbody.AddForce(Quaternion.Euler(0, -90, 0) * transform.forward * speed, ForceMode.Impulse);
        //     }
        //     if (Input.GetKey(KeyCode.D))
        //     {
        //         playerRigidbody.AddForce((Quaternion.Euler(0, 90, 0) * transform.forward * speed), ForceMode.Impulse);
        //     }
        //     if (Input.GetKey(KeyCode.S))
        //     {
        //         playerRigidbody.AddForce(-transform.forward * speed, ForceMode.Impulse);
        //     }
        // }
        AdditionnalUpdate();
    }

    void FixedUpdate()
    {
        // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        // if (Physics.Raycast(ray, out hit, 100))
        // {
        //     Vector3 mousePosInWorld = hit.point;
        //     mousePosInWorld.y = transform.position.y;
        //     transform.LookAt(mousePosInWorld, Vector3.up);
        // }

        bool wasInAir = !isGrounded;
        float curY = transform.position.y;
        // isGrounded = Mathf.Abs(curY - previousY) < 0.00001f;
        isGrounded = CheckIsGrounded();
        if (wasInAir && isGrounded)
        {
            returnToGroundEvent?.Invoke();
        }
        previousY = curY;
        pitsManager.setPitsPassable(!isGrounded);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public virtual bool CheckIsGrounded()
    {
        // Debug.Log("velo y: " + playerRigidbody.velocity.y + ", x: " + playerRigidbody.velocity.x + ", z: " + playerRigidbody.velocity.z);
        // return Mathf.Abs(playerRigidbody.velocity.y) < 0.1f;
        //RaycastHit hit;
        //Vector3.down * distanceCenterFeet
        //return Physics.SphereCast(transform.position + Vector3.down * distanceCenterFeet, 0.5f, Vector3.down, out hit, 1f);
        // RaycastHit hit;
        // return Physics.Raycast(transform.position + (Vector3.down * (distanceCenterFeet - 0.2f)), Vector3.down, out hit, 0.3f))
        // bool isGrounded = Mathf.Abs(transform.position.y - previousY) < 0.00001f;
        // previousY = transform.position.y;
        float curY = transform.position.y;
        return Mathf.Abs(curY - previousY) < 0.00001f;
    }

    public void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // var layerMask = (1 << 10); //Ignore walls and pits (layer 10)
        // layerMask = ~layerMask;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseOrientationLayers))
        {
            Vector3 mousePosInWorld = hit.point;
            mousePosInWorld.y = transform.position.y;
            transform.LookAt(mousePosInWorld, Vector3.up);
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - mousePosInWorld, Vector3.up), Time.deltaTime * rotationSpeed);
        }
    }

    protected virtual void SetMovementComponentsEnabled(bool enabled)
    {
    }

    public void MoveAway()
    {
        Vector3 freeDir = FindNearFreeTile();
        if (freeDir != Vector3.zero)
        {
            SetMovementComponentsEnabled(false);
            transform.position += 3 * freeDir;
            SetMovementComponentsEnabled(true);
            Debug.Log("CHARAC: Moved away (of temporary area)");
        }
        else
        {
            Debug.LogError("CHARAC: Free tile not found");
        }
    }

    private Vector3 FindNearFreeTile()
    {
        float distCenterBorder = 0.7f;

        Vector3[] moveDirections = new Vector3[] { transform.forward, transform.right, -transform.forward, -transform.right };
        foreach (Vector3 dir in moveDirections)
        {
            if (!Physics.Raycast(transform.position, dir, distCenterBorder + 2f))
            {
                return dir;
            }
        }
        return Vector3.zero;
    }

    public static string MAX_WAVES = "MAX_WAVES";
    public static string PLAYED_GAMES = "PLAYED_GAMES";
    public static string WON_A_GAME = "WON_A_GAME";

    private static string GetStatName(string character, string statName)
    {
        return character + "-" + statName;
    }

    public static void SaveStat(string character, string statName, float val)
    {
        PlayerPrefs.SetFloat(GetStatName(character, statName), val);
    }

    public static void SaveStat(string character, string statName, bool val)
    {
        PlayerPrefs.SetInt(GetStatName(character, statName), val ? 1 : 0);
    }

    public static float GetMaxWaves(string character)
    {
        return PlayerPrefs.GetFloat(GetStatName(character, MAX_WAVES));
    }

    public static float GetPlayedGames(string character)
    {
        return PlayerPrefs.GetFloat(GetStatName(character, PLAYED_GAMES));
    }

    public static bool HasWonAGame(string character)
    {
        return PlayerPrefs.GetInt(GetStatName(character, WON_A_GAME)) == 1;
    }

}
