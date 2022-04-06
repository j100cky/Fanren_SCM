using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{

	[SerializeField] public CraftingBoardItemcontainer craftingBoard; 
	public List<InventoryButton> craftingButtons;
	[SerializeField] Crafting crafting;
	[SerializeField] RecipeList recipeList;
	[SerializeField] Image craftItemSprite;
	[SerializeField] Button craftButton;
	[SerializeField] Button heatSourceButton;
	[SerializeField] CraftOutputButton outputButton; //This is for nullifying the crafteditem slot when materials are removed.  
	

	
	private void Start()
	{
		Init();
	}

	public void Init()
	{
		SetCraftingPanelIndex();
		Show();
		craftButton.interactable = false; //Disable the craft button until the corrected materials are presented. 
		craftItemSprite.enabled = false; //Make the Image disable otherwise there will be a white blank in the slot. 
	}

	private void Update()
	{

		Show();
	}

	private void OnEnable()
	{
		Show();
	}

	private void SetCraftingPanelIndex() //cycles through the inventory to update the inventory panel
	{
		for(int i = 0; i < craftingButtons.Count && i < craftingBoard.slots.Count; i++) //Must contain craftingBoard.slots.count because there are more buttons than slots in this case. 
		{
			craftingButtons[i].SetCraftPanelIndex(i);
		}
	}

	public virtual void Show() //if there is an item in that index, update the button, if not, hide the button's icon. This mimic the item getting attached to the mouse.
	{
		for(int i = 0; i < craftingButtons.Count && i < craftingBoard.slots.Count; i++)//Must contain craftingBoard.slots.count because there are more buttons than slots in this case. 
		{
			if(craftingBoard.slots[i].item == null)
			{
				craftingButtons[i].Clean();
			}
			else
			{
				craftingButtons[i].CraftSet(craftingBoard.slots[i]);
			}
		}
	}

	public virtual void OnCraftingLeftClick(int id)
	{
		GameManager.instance.dragAndDropController.OnCraftingLeftClick(craftingBoard.slots[id]);
		CheckForCraftable();
		Show();
	}

	public void CheckForCraftable() //Make this a separate function so the CraftButtonController can use this too, after one item is crafted. 
	{
		crafting.PreCraftChecks(recipeList); //Run the precraft check, which checks for 1)if there is any empty space in the bag to receive new items. 
		
		if(crafting.craftable == true)
		{
			if(crafting.isCrafting == false) //When the materials are enough and it is not crafting, we will set the crafting button active and show the preview. 
			{
				craftItemSprite.enabled = true; //Make the item preview image  enabled.
				craftItemSprite.sprite = crafting.recipe.output.item.icon; //Set the image on the crafting panel as the icon of the item being crafted. 
				craftButton.interactable = true; //Allow the player to choose whether they craft or not. 
			}
			else //When the materials are ready but it is crafting, we set the craft button inactive but do not change the preview panel (if we do the sprite will become null).
			{
				craftButton.interactable = false; //Do not allow the player to craft. 
			}

		}

		else //When the materials are not enouth (e.g. the player takes the items out from the crafting panel)...
		{
			if(crafting.isCrafting == true) //If it is already crafting something, we don't want to change the preview, we just want to set the craft button inactive.
			{
				craftButton.interactable = false;
			}
			else	//When it is not crafting, feel free to clear the preview and set the craft button inactive. 
			{
				craftItemSprite.enabled = false; //Disable the item preview image again otherwise there will be a white blank.
				craftItemSprite.sprite = null;//remove the image if itemslot is removed from the craftaBoard.
				craftButton.interactable = false;
			}
			

		}
	}


	public virtual void OnCraftingRightClick(int id)
	{

	}


}
