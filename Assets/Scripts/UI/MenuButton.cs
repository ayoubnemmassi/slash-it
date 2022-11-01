using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] MenuSelectionButtonController menuSelectionButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	public UnityEvent MenuButtonEvent;

	// Update is called once per frame
	void Update()
    {
        if (menuButtonController) { 
		if(menuButtonController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if(Input.GetAxis ("Submit") == 1){

				animator.SetBool ("pressed", true);
				MenuButtonEvent.Invoke();
			}else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
		}
		}
        else 
		{
			if (menuSelectionButtonController.index == thisIndex)
			{
				animator.SetBool("selected", true);
				if (Input.GetAxis("Submit") == 1)
				{

					animator.SetBool("pressed", true);
					MenuButtonEvent.Invoke();
				}
				else if (animator.GetBool("pressed"))
				{
					animator.SetBool("pressed", false);
					animatorFunctions.disableOnce = true;
				}
			}
			else
			{
				animator.SetBool("selected", false);
			}
		}
	}
}
