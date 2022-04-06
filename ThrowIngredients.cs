using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Throw Ingredient")]
public class ThrowIngredients : ToolAction
{
	//Describe what happens when right clicked. 
	public override void OnItemUsed(Item usedItem, ItemContainer inventory)
	{
		//Obtaining where the mouse is pointing at.
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

		//Find colliders at the mouse position. If the collider carries a Furnace script, do the CheckItem() function. 
		Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.1f);
		if (collider == null) { return; }
		if (collider.GetComponent<Mortar>() == null) { return; }
		bool complete = collider.GetComponent<Mortar>().CheckItem(usedItem, inventory);
	}
}
