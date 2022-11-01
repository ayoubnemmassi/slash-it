using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    private GameObject target;
    private Transform slimeTransform;
    [SerializeField] private float throwForce;

    [SerializeField] float Ypos;
    [SerializeField] float angular;
    public static float slowingP = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //new Vector3(target.transform.position.x, target.transform.position.y+Ypos,target.transform.position.z)//MoveTowards(slimeTransform.position, new Vector3(target.transform.position.x, target.transform.position.y + Ypos, target.transform.position.z),5)
        //  print(slimeTransform.rotation.eulerAngles.y);
        GetComponent<Rigidbody>().AddForce(Quaternion.Euler(angular, slimeTransform.rotation.eulerAngles.y, 0) * Vector3.forward * throwForce, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            // other.GetComponent<WarriorAnimsFREE.WarriorMovementController>().runSpeed *= slowingP;
            Character charac = other.GetComponent<Character>();
            if (charac != null)
            {
                charac.SetSlowDown(5, slowingP); // slow down for 5 seconds with slowingP speed
            }
            else
            {
                Debug.LogError("MUD: Character compo of Player not found");
            }
        }
        if (other.tag != "Enemy")
            Destroy(gameObject);

    }

    public void SetupTarget(GameObject target, Transform slime) { this.target = target; slimeTransform = slime; }
}
