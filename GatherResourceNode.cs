using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
	Undefined,
	Tree,
	Ore,
	Enemy,
	WildPlants,
	Crops
}


[CreateAssetMenu(menuName = "Data/Tool Action/Gather Resource Node")]

public class GatherResourceNode : ToolAction
{
	//[SerializeField] float sizeOfInteractableArea = 1f;
	[SerializeField] List<ResourceNodeType> canHitNodesOfType;

	float sizeOfAttackArea; //The area where the player can actually hit.//
	//float skillAttackSize; //Will be referencing attackSize from the Item scriptable object//
	//public int skillID ; //For referencing to the skill ID in the skill animator.//
	public float damage; //The damage this skill makes.//
	//[SerializeField] float manaCost; //The mana cost of this skill.//
									 //bool isCoolingDown; //Does not call OnApply() if this is true.  
	//[SerializeField] GameObject skillPrefab; //For the animation and stuff

	public override bool OnApply(Vector2 worldPoint)
	{
		if(GamePauseController.instance.isPaused == true)
		{
			return false;
		}

		if(GamePauseController.instance.isCoolingDown == true)	//Do not continue this function when the isCoolingDown is true.
		{
			Debug.Log("Skill cooling down");
			return false;
		}

		//Check the character's mana. 
		Character character = GameManager.instance.character;
		if (character.currentMana <= 0)
		{
			Debug.Log("no enough mana");
			//Play some animation//
			return false;
		}

		sizeOfAttackArea = GameManager.instance.character.useToolRange; //Pass the character's useToolRange value to here for distance calculation.//
		//skillAttackSize = GameManager.instance.player.GetComponent<ToolbarController>().GetItem.attackSize;	//Obtain the specific tool's attack size.
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));	//Obtaining where the mouse is pointing at.
		float distance = (mousePos - worldPoint).sqrMagnitude; //Calculating casting range.
		distance = Mathf.Sqrt(distance);
		if(distance > sizeOfAttackArea) // The the range the player desires to cast is further than the character's cast range, no magic is cast. 
		{
			return false;
		}

		//Call cooldown. 
		GamePauseController.instance.SetCoolDown(true);

		//Use mana.
		character.UseMana(manaCost);

		//Reset the list of interacting object so they don't keep adding.
		GameManager.instance.interactingObjectContainer.interactingObjects = new List<Transform>();
		GameManager.instance.interactingObjectContainer.brokenObjects = new List<Transform>();
		

		//Instantiating the skill animator prefab.
		GameObject go = Instantiate(skillPrefab);
		//Set the attack size of the skill
		go.GetComponent<SkillController>().SetAttackSize(skillAttackSize);
		//Setting the attack damage of the skill. 
		go.GetComponent<SkillController>().SetDamage(damage);
		//Set the position of the skill prefab.
		Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos3d = new Vector3(mousePos3d.x, mousePos3d.y, 0);
		go.transform.position = mousePos3d;
/*		//Find all colliders within the circle.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos3d, skillAttackSize);

		foreach (Collider2D c in colliders)
		{
			ToolHit hit = c.GetComponent<ToolHit>(); //to detect the components in the script ToolHit. Store the info in the var "hit"
			if (hit != null)    //If the collider has the hit script attached...
			{
				if (hit.CanBeHit(canHitNodesOfType) == true) //If the type of the object can be hit by the tool selected...
				{
					//Set the ToolHit in the HammerHit animator prefab so that the Hit() function can be called at the exact frame of the animation.
					go.GetComponent<SkillController>().SetToolHit(hit);
					
				}

			}

		}*/

		return false;
/*		//If the desired range is within the character's casting range, continue.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(mousePos, attackSize, 0f); //create a array to store any object that overlaps with the area of tool. 		
		UsingSkills.instance.summonSkill(mousePos, skillID);	//Call the summonSkill function which instantiate the Skill prefab. 
		
		character.UseMana(manaCost);	//Use mana
		GameManager.instance.interactingObjectContainer.interactingObjects = new List<Transform>();	//Reset the list of interacting object so they don't keep adding.
		GameManager.instance.interactingObjectContainer.brokenObjects = new List<Transform>();
		foreach(Collider2D c in colliders)
		{
			ToolHit hit = c.GetComponent<ToolHit>(); //to detect the components in the script ToolHit. Store the info in the var "hit"
			if(hit != null)	//If the collider has the hit script attached...
			{
				if(hit.CanBeHit(canHitNodesOfType) == true) //If the type of the object can be hit by the tool selected...
				{
					
					hit.Hit(damage); //Call the Hit() function, which includes reducing health, and updating interacting object list. 
				}

			}

		}

		return false;*/
	}
}
