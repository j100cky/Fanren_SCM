using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
	Undefined,
	Enemy
}


[CreateAssetMenu(menuName = "Data/Tool Action/Hitting Enemy")]

public class HittingEnemyBaseScript : ToolAction
{
	float sizeOfAttackArea; //The area where the player can actually hit.//
	//float skillAttackSize; //Will be referencing attackSize from the Item scriptable object//
	[SerializeField] List<EnemyType> canHitNodesOfType;
	public float damage; //The damage this skill makes.//
	//public int skillID=1; //For referencing to the skill ID in the skill animator.//
	//public int hitBySkillEffectID=1; //For referencing to the hitBySkillID in the hitBySkill animator. Enemy hit by different weapons show different hit effects.//
	//public int miao = 10; //The number of enemy can be attacked at one instance.//
	//[SerializeField] float manaCost;

	public override bool OnApply(Vector2 worldPoint) //hit each enemy individually with individual skill effect//
	{

		//===========Checking for mana level.===============//
		Character character = GameManager.instance.character; //Check for mana level.//
		if (character.currentMana - manaCost < 0)
		{
			Debug.Log("no enough mana");
			//Play some animation//
			return false;
		}

		//=========Checking for whether the mouse is trying to attack an enemy outside of range==========//
		sizeOfAttackArea = GameManager.instance.character.useToolRange; //Pass the character's useToolRange value to here for distance calculation.//
		//skillAttackSize = GameManager.instance.player.GetComponent<ToolbarController>().GetItem.attackSize;
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		float distance = (mousePos - worldPoint).sqrMagnitude;
		distance = Mathf.Sqrt(distance);
		if (distance > sizeOfAttackArea)
		{
			Debug.Log("The level is too low to reach that far.");
			return false;
		}
		else
		{
			character.UseMana(manaCost);
			GameObject go = Instantiate(skillPrefab, mousePos, Quaternion.identity);
			go.GetComponent<SkillController>().SetDamage(damage);
			return true;
		}
	}

}
