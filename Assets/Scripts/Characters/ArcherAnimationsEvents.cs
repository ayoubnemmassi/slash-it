using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArcherAnimationsEvents : MonoBehaviour
{

    public UnityEvent bowShootEvent = new UnityEvent();

    // private void Awake()
    // {
    //     bowShootEvent = new UnityEvent();
    // }

    public void OnBowShoot()
    {
        bowShootEvent?.Invoke();
    }

}
