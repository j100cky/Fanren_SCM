using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumptionController : MonoBehaviour
{
	//This is not implemented because this script will interfere with ToolActions. 
	//E.g. when a medicine is right clicked, I want it to first test whether it is being clicked into a mortar, and consume only 
	//if it is not. 
/*
	ToolbarController toolbarController; //To get referen of what is the player currently holding.
	Character character; //To get refereence of the character's status.//

	private void Start()
	{
		toolbarController = GetComponent<ToolbarController>();
		character = GetComponent<Character>();
	}


	private void Update()
	{
		Item item = toolbarController.GetItem;
		if(item == null){return;}
		if(item.onItemUsed != null) { return; }
		if(item.isConsumable == false) {return;} 
		if(Input.GetMouseButtonDown(1))
		{
			character.RestoreHealth(item.restoreHP); 
			character.RestoreMana(item.restoreMP);
			GameManager.instance.inventoryContainer.Remove(item, 1);
		}
	}*/
}
