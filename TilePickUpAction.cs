using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is used to describe tool-assisted or empty-handed tile pick up action.
[CreateAssetMenu(menuName = "Data/Tool Action/Harvest")]

public class TilePickUpAction : ToolAction
{
	[SerializeField] float damage;

    public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
    {
		//When it is a scythe using this tool action, call the scythe animation prefab. 
        if(skillPrefab != null)
        {
			//Get the character's 神识范围
			float sizeOfAttackArea = GameManager.instance.character.useToolRange;

			//Obtain the attack size of the specific tool.
			float attackSize = GameManager.instance.player.GetComponent<ToolbarController>().GetItem.attackSize;
			//Obtaining where the mouse is pointing at.
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));   
			//Obtaining the player's position
			Vector2 playerPos = new Vector2(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y);
			//Calculate the distance between the player and the mouse position
			float distance = Mathf.Sqrt((mousePos - playerPos).sqrMagnitude); 
			// Only instantiate the gameobject if the distance is within the character's shenshi range. 
			if (distance < sizeOfAttackArea) 
			{
				//Set cooldown to true here. 
				GamePauseController.instance.SetCoolDown(true);
				//Use mana.
				GameManager.instance.player.GetComponent<Character>().UseMana(manaCost);
				//Instantiating the skill animator prefab and reposition it.
				GameObject go = Instantiate(skillPrefab);
				go.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
				//Set the damage of this action. This is for calculating how many times the grass needs to be cut.
				go.GetComponent<SkillController>().SetDamage(damage);
				//Set the attack size of the tool
				go.GetComponent<SkillController>().SetAttackSize(skillAttackSize);
				//Load the grid position and the tilemap read controller to the skillcontroller script
				//so that it can call the PickUp() function at the exact frame. 
				go.GetComponent<SkillController>().SetTileMapReadController(gridPosition, tileMapReadController);
			}
			return true;
		}

        //If prefab is not detected (most likely empty handed), simply use the PickUp() function without instantiating any prefab. 
		else
        {
			tileMapReadController.cropManager.PickUp(gridPosition);

			return true;
		}

    }
}
