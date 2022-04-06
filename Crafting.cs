using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour //This script is meant to be attached to the CraftingPanel object or be referenced by the RecipePanel script.  
{
	[SerializeField] ItemContainer inventory;
	[SerializeField] CraftingBoardItemcontainer craftingBoardItemContainer;
	public CraftingRecipe recipe;
	public bool craftable;//A bool variable to tell the RecipePanel whether crafting is successful. If yes, show the item sprite.
	int craftingBoardItemCount; //count the number of itemson the crafting board to match the recipe's requirement. 
	public bool isCrafting = false; //Used by the Tick() function in the CraftingOUtputTimeManager. 
	public CraftingRecipe currentRecipe; //Store the recipe used in this current crafting session. This is needed because as soon as the player take the materials out 
	//from the panel, the recipe will disappear. 
	[SerializeField] GameObject doneCraftingNoticePrefab; //For the item icon above the furnace.
	GameObject doneCraftingNotice; //For saving the information of the item and for the destroy of the notice after item removal. 


	public void PreCraftChecks(RecipeList recipeList)
	{
		if(inventory.CheckFreeSpace() == false)
		{
			Debug.Log("not enough inventory space");
			return;
		}

		craftingBoardItemCount = 0; //reset the counter every time before it is counted. 
		for (int i = 0; i < craftingBoardItemContainer.slots.Count; i++)
		{
			if(craftingBoardItemContainer.slots[i].item != null) //If there are items in the crafting board...
			{
				craftingBoardItemCount += 1; //Assign the number of items in the crafting board to this craftingBoardItemCount variable.
			}
		}

		for(int i = 0; i < recipeList.recipes.Count; i++) //Cycling through each recipe in the grand recipe list. 
		{

			recipe = recipeList.recipes[i]; //Temporarily storing the i th element to the recipe variable. 
			//Debug.Log(recipe.output.item.name);
			for(int j = 0; j < recipe.elements.Count; j++) //Cycling through the required material for the i th recipe.
			{
				//Two conditions: 1) the number of elements is matched, 2) the type of elements is matched
				if(craftingBoardItemCount != recipe.elements.Count || craftingBoardItemContainer.CheckItem(recipe.elements[j]) == false) 

				{
					craftable = false;
					Debug.Log("Not enough raw material to craft.");
					recipe = null; //To prevent that the output of the last recipe to be stored in this variable. 
					break; //Break from this recipe and move to the next recipe. 
				}
				else
				{
					craftable = true;//craftable is only temporarily true unless all elements are checked and matched.
					continue; //If one material matches, continue checking further materials in the same recipe. 
				}

			}

			if(craftable == true) //When all required items in the recipe is met, craftable will be true here. 
			{
				break; //If the item is craftable, don't move on to the next recipe. So now the recipe variable is the recipe that matches.
			}
		}

	}

	public void Craft()
	{
		for(int i = 0; i < recipe.elements.Count; i++) //Remove the count amount of items from the inventory after crafting.
		{
			craftingBoardItemContainer.Remove(recipe.elements[i].item, recipe.elements[i].count);
		}
		//isCrafted = true;
		//Debug.Log("successfully crafted");
		
	}

	public void ShowDoneCrafting(Item item)
	{
		Vector3 noticePosition = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z); //Define the position that the prefab should be instantiated in.
		GameObject o = Instantiate(doneCraftingNoticePrefab, noticePosition, Quaternion.identity);//Instantiate the game object prefab above the furnace. 
		o.GetComponent<SpriteRenderer>().sprite = item.icon; //Change the icon of the done crafting notice. 
		doneCraftingNotice = o; //Make the doneCraftingNotice variable as the gameobject o so that the next function don't need to use the Find() function. 
	}

	public void KillDoneCrafting()
	{
		Destroy(doneCraftingNotice);
	}
}
