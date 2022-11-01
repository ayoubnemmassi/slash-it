using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] MenuSelectionButtonController menuSelectionButtonController;
	[SerializeField] string tag;
	[SerializeField] Animator animator;
	public bool disableOnce;

	void PlaySound(AudioClip whichSound){
		if(!disableOnce){
			if (tag == "selection") { menuSelectionButtonController.audioSource.PlayOneShot(whichSound); }
			else { menuButtonController.audioSource.PlayOneShot(whichSound); }
		}else{
			disableOnce = false;
		}
	}
	void PlayAnimation(string animationName) 
	{

		animator.Play(animationName);
	}
}	
