using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitsManager : MonoBehaviour
{
    GameObject[] pits;
    private bool pitsPassable = false;
    private float lastPassableChangeTime = 0;

    void Awake()
    {
        pits = GameObject.FindGameObjectsWithTag("Pit");
    }

    public void setPitsPassable(bool passable)
    {
        if (pitsPassable != passable && (Time.time - lastPassableChangeTime > 0.8))
        {
            Debug.Log("Changed pits passable to: " + passable);
            foreach (GameObject pit in pits)
            {
                pit.GetComponent<Collider>().isTrigger = passable;
            }
            pitsPassable = passable;
            lastPassableChangeTime = Time.time;
        }
    }
}
