using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PotionsButtons : MonoBehaviour
{
    
    // Start is called before the first frame update
    private bool potion;
    [SerializeField] string name;
    private float timerEffect;
    private float timerCoolDown;
    private float potionTimeEffect;
    [SerializeField] private float _potionTimeEffect = 5;
    
    Button button;
    Image image;
    private float coolDown = 20;
    private bool firstTime;

    void Start()
    {
        potionTimeEffect = _potionTimeEffect;
        firstTime = true;
        button = GetComponent<Button>();
        image = GetComponent<Image>();

    }
    void Update()
    {
        if (potion)
        {
            firstTime = false;
            print("potion effect" + potionTimeEffect);
            button.interactable = false;
            image.fillAmount = 0;
            timerEffect = Time.deltaTime;
            potionTimeEffect -= timerEffect;
            coolDown = 20;

        }
        else if (!potion && coolDown > 0 && !firstTime)
        {

           
            timerCoolDown = Time.deltaTime;
            coolDown -= timerCoolDown;
            image.fillAmount += timerCoolDown/20;
        }
        if (coolDown <= 0)
        {

            timerCoolDown = 0;
            button.interactable = true;
        }
        if (potionTimeEffect <= 0)
        {
            timerEffect = 0;
            potionTimeEffect = _potionTimeEffect;
            if (name == "purple") { Wizard.boltDamage = 5; } 
            else
            {
                Mud.slowingP = 0.5f;
            }
           
            potion = false;

        }
    }
    public void itemSelected(string name)
    {
        switch (name)
        {
            case "purple": Wizard.boltDamage = 0; print("selected " + Wizard.boltDamage); potion = true; break;
            case "green": Mud.slowingP = 1; potion = true; break;
        }
    }
}
