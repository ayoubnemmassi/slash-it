using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Leap : MonoBehaviour
{

    private PitsManager pitsManager;
    private InputController inputController;

    private void Awake()
    {
        pitsManager = FindObjectOfType<PitsManager>();
        inputController = GetComponentInChildren<InputController>();
    }

    public bool doLeap()
    {
        return doLeap(-transform.forward);
    }

    public bool doLeap(Vector3 dir)
    {
        pitsManager.setPitsPassable(true);

        // Default Max Depenetration Velocity changed from 10 to 100
        return inputController.requestJump(dir);
        // playerRigidbody.velocity = Vector3.zero;
        // playerRigidbody.angularVelocity = Vector3.zero;
        // playerRigidbody.AddForce(new Vector3(0, 1000, 0) + 100 * transform.forward, ForceMode.Impulse);
    }

}
