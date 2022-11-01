using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LifeChangeEvent : UnityEvent<float> { }

public class Health : MonoBehaviour
{

    public UnityEvent dieEvent = new UnityEvent();
    public LifeChangeEvent lifeChangeEvent = new LifeChangeEvent();

    private float lp;
    [SerializeField] private float maxLP = 10;
    [SerializeField] private bool destroyOnDie = false;

    public float GetMaxLP()
    {
        return maxLP;
    }


    public float GetLPRatio()
    {
        return lp / maxLP;
    }


    public float LP
    {
        get { return lp; }
        set
        {
            float previousLp = lp;
            if (value <= 0)
            {
                lp = 0;

                dieEvent?.Invoke();
            }
            else if (value >= maxLP)
            {
                lp = maxLP;
            }
            else
            {
                lp = value;
            }

            if (lp != previousLp)
            {
                lifeChangeEvent?.Invoke(previousLp);
            }
        }
    }

    private void Awake()
    {
        LP = maxLP;
        Debug.Log("HEALTH " + GetInstanceID() + " : LP init to maxLP");
        // dieEvent = new UnityEvent();
        // lifeChangeEvent = new UnityEvent();

        if (destroyOnDie)
        {
            dieEvent.AddListener(OnDieListener);
        }
    }

    public bool IsDead()
    {
        return LP <= 0;
    }

    private void OnDestroy()
    {
        dieEvent.RemoveAllListeners();
    }

    private void OnDieListener()
    {
        Debug.Log(gameObject.name + " : I'm dead and destroyed ! ");
        Destroy(gameObject);
    }

}
