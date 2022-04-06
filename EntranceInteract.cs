using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceInteract : Interactable
{
	Animator anim;
    [SerializeField] Transition transition; 

  	public override void Interact(Character character)
  	{
  		GameObject player = GameManager.instance.player;
  		anim = GetComponentInParent<Animator>();
  		if(anim != null)
  		{
            GamePauseController.instance.SetPause(true);
  		  	anim.SetTrigger("isInteract");
  		}

  		transition.InitiateTransition(player.transform);
  	}


}
