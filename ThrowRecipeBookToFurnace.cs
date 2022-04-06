using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Throw Recipe Book")]
public class ThrowRecipeBookToFurnace : ToolAction
{
	[SerializeField] List<Recipe_Ingredient> ingredient;
	[SerializeField] Item output;

	//Describe what happens when right clicked. 
	public override void OnItemUsed(Item usedItem, ItemContainer inventory)
	{
		//Obtaining where the mouse is pointing at.
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

		//Find colliders at the mouse position. If the collider carries a Furnace script, do the CheckItem() function. 
		Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.1f); 
		if(collider == null) { return; }
		if(collider.GetComponent<Mortar>() == null) { return; }
		bool complete = collider.GetComponent<Mortar>().CheckItem(usedItem, inventory);
		if(complete == true)
        {
			//If the recipe book is accepted, remove the item. 
			inventory.Remove(usedItem);
            for (int i = 0; i < ingredient.Count; i++)
            {
                collider.GetComponent<Mortar>().ImportIngredient(ingredient[i]);
            }
			collider.GetComponent<Mortar>().ShowIngredientIcons();
			collider.GetComponent<Mortar>().SetOutputItem(output);
;
		}

	}
}
