using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Slider slider;

    private Health health;

    public void LinkToHealthComponent(Health health)
    {
        this.health = health;
        health.lifeChangeEvent.AddListener(OnLifeChangeEvent);
        SetMaxHealth((int)health.GetMaxLP());
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        print(gameObject.name+" set health to  "+ health);
    }

    void OnLifeChangeEvent(float previousLp)
    {
        Debug.Log("HEALTHBAR: Update life: from " + previousLp + " to" + health.LP + " for " + health.GetInstanceID());
        SetHealth((int)health.LP);
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.lifeChangeEvent.RemoveListener(OnLifeChangeEvent);
        }
    }

}
