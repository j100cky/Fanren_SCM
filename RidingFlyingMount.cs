using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Data/Tool Action/Riding Flying Mount")]


public class RidingFlyingMount : ToolAction
{
	BoxCollider2D boxcollider;
	CharacterController2D characterController; 
	[SerializeField] float flyingSpeed = 10f;
	FlyingSwordSkillScript flyingMountTarget; //For referencing the flying mount skill controller script. 
	GameObject player;

	public override bool OnApply(Vector2 worldPoint)
	{
		player = GameManager.instance.player;
		boxcollider = player.GetComponent<BoxCollider2D>();
		
		characterController = player.GetComponent<CharacterController2D>();
		
/*		//Check whether another flying mount is aready summoned. 
		if(player.GetComponent<FlyingMountController>().isRiding == true)
        {
			Debug.Log("Another flying mount is existing. Please dismount first");
			return false;
        }*/

		if(boxcollider.enabled == true)	//When boxcollider is enabled, that means the player is not mounted. Will mount.
		{
			//Pause the game while the animation is playing
			GamePauseController.instance.SetPause(true);
			//Instantiate the skill prefab and set the position slightly below the player. 
			GameObject go = Instantiate(skillPrefab);
			go.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 2f,
				player.transform.position.z);
			//Store the SkillController script to the flyingMountTarget variable because we need to use this script several times. 
			flyingMountTarget = go.GetComponent<FlyingSwordSkillScript>();
			//Set the flying speed of the flying mount. Pass the value to the damage variable. 
			flyingMountTarget.SetDamage(flyingSpeed);
/*			//Change the player's position and set player animation to standstill. 
			flyingMountTarget.ManMount();*/
			return true;
		}
		else	//When the boxcollider is disabled, that means the player is mounted. Clicking the item will dismount.
		{
			//Pause the game while the animation is playing. 
			GamePauseController.instance.SetPause(true);
			//Change the player position back and set player animation to walking. 
			flyingMountTarget.ManDismount();
			return false;
		}
	}

}
