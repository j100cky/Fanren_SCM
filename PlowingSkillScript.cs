using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlowingSkillScript : SkillController
{
	List<Vector3Int> interactingTiles;

	//Called by the last frame of the animation. 
	public void ChangeTile()    //Change the tiles that the player is interacting with. 
	{
		interactingTiles = GameManager.instance.interactingObjectContainer.interactingTiles;
		for (int i = 0; i < interactingTiles.Count; i++)
		{
			GameManager.instance.GetComponent<CropManager>().Plow(interactingTiles[i]); //Call the Plow() function in the CropManager script. 
		}
		//GameManager.instance.GetComponent<GamePauseController>().isCoolingDown = false;		
	}
}
