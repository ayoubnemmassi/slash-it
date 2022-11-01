using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryArea : MonoBehaviour
{
    [SerializeField] private float maxStaySeconds = 2;
    private float timeToTryADirection = 5;
    private float lastStayStartTime = 0;

    private void OnTriggerStay(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            if (lastStayStartTime == 0)
            {
                lastStayStartTime = Time.time;
                return;
            }
            float stayTime = Time.time - lastStayStartTime;
            if (stayTime > maxStaySeconds)
            {
                Character charac = other.gameObject.GetComponent<Character>();
                if (charac != null)
                {
                    // Transform trans = other.gameObject.transform;
                    // Vector3 dir = stayTime > timeToTryADirection ? trans.right : trans.forward;
                    // inputCtrl.requestMove(dir);
                    charac.MoveAway();
                    lastStayStartTime = 0;
                }
                else
                {
                    Debug.LogError("TEMPAREA: character not found");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ("Player".Equals(other.gameObject.tag))
        {
            lastStayStartTime = 0;
            // InputController inputCtrl = other.gameObject.GetComponent<InputController>();
            // if (inputCtrl != null)
            // {
            //     inputCtrl.requestMove(Vector3.zero);
            // }
            // else
            // {
            //     Debug.LogError("TEMPAREA: Input controller not found");
            // }
        }
    }

}
