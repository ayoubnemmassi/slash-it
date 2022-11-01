using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
 GameObject panel;
    Animator animator;
    bool open;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
       // print(GetComponent<RectTransform>().position.y);
        if (Input.GetKeyDown(KeyCode.I)) { if (!open) { OpenInventory(); } else { CloseInventory();  }  }   
    }
   public void OpenInventory() 
    {
        animator.Play("inventory");
        open = true;
    }
    public void CloseInventory()
    {

        animator.Play("inventoryclosed");
        open = false;
    }
    public void Print() { print("clicked"); }
}
