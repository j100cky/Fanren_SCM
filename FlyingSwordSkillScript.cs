using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSwordSkillScript : SkillController
{
	CharacterController2D characterController; 

	//This is called by the specific animation frame at the end of the summoning animation. 
	public void ManMount()  
	{
		//Change the player's position slightly above to mimic a jumping effect. Will work on smoothing it more later. 
		player.transform.position = new Vector3(player.transform.position.x,
			player.transform.position.y + 1, player.transform.position.z);
		//Set the main character's animator isMounting parameter to true so he uses the Idle animation while mounting
		player.GetComponent<Animator>().SetBool("isMounting", true);
		//Run the other functions related to mounting. 
		Mount();
	}

	public void ManDismount() //Emunate the dismounting effect.
	{
		Dismount();
		player.transform.position = new Vector3(player.transform.position.x,
			player.transform.position.y - 1, player.transform.position.z);  //lower the player a little bit. Later will make a smooth jumping effect. 
		player.GetComponent<FlyingMountController>().isRiding = false;  //Change the isRiding variable value back to false so the script won't keep looking for the GO.
		player.GetComponent<Animator>().SetBool("isMounting", false);   //When dismount, the main character will go back to walking animations.
	}

	private void Mount()
	{
		//Resume the game.
		GamePauseController.instance.SetPause(false);
		//Cancel collision box of the player so it won't hit any buildings.
		BoxCollider2D boxcollider = player.GetComponent<BoxCollider2D>(); 
		boxcollider.enabled = false;
		//Increase movement speed. 
		characterController = GameManager.instance.player.GetComponent<CharacterController2D>();
		characterController.speed = characterController.speed+damage;
		//Increase the sorting order of the player so that it looks like flying.
		player.GetComponent<SpriteRenderer>().sortingOrder = 99;
		//Change the sorting layer of the sword from VFX to Foreground and set order to high. 
		GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
		GetComponent<SpriteRenderer>().sortingOrder = 99;
		//Change the isRiding bool variable in the FlyingMountController script to make mount follow. 
		player.GetComponent<FlyingMountController>().SetFlyingMount(this.gameObject);
		player.GetComponent<FlyingMountController>().isRiding = true;
	}

	private void Dismount()
	{
		//enable box collider.
		BoxCollider2D boxcollider = player.GetComponent<BoxCollider2D>();
		boxcollider.enabled = true;
		//resume walking speed.
		characterController.speed = characterController.originalSpeed;
		//Resume the sorting order of the player so that it looks like flying.
		player.GetComponent<SpriteRenderer>().sortingOrder = 0;    
		//Play the dismount animation.
		anim.SetTrigger("isDismount");  
	}

	//This is called by the last frame of animation. 
	public override void FinishAnimation()
	{
		if (gameObject != null)
		{
			Destroy(gameObject);
			ResetCoolDown();
			ResetPause();
		}

	}
}
