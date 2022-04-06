using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftOutputButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] public ItemSlot outputSlot;
	[SerializeField] Image icon;
	[SerializeField] public Text text;
	Item item;
	[SerializeField] RecipePanel recipePanel; 
	[SerializeField] Crafting crafting;

	private void Start()
	{
		icon.enabled = false; //Set the Image to inactive otherwise there will be a white blank when nothing is there.
		text.enabled = false; //Set the Text to inactive otherwise there will be a "9999" when nothing is there.
		/*crafting = 
			GameManager.instance.interactingObjectContainer.interactingObjectWithPanels.GetComponent<Crafting>(); //Obtain the crafting script from the object that we are interacting. */
	}


	public void Set(ItemSlot slot) //Setting up the icon sprite and text for the slot, when they are assigned.  
	{
		outputSlot.Copy(slot);
		item = slot.item; //passing the information of the item in that slot into the item variable so that the item page can use this item's information. 
		icon.enabled = true; //Allow the icon to be set active. Otherwise the slot will be a white blank when nothing is there. 
		text.enabled = true; //For the same reason above.
		icon.sprite = slot.item.icon;
		icon.gameObject.SetActive(true);

		if (slot.item.isStackable == true)
		{
			text.gameObject.SetActive(true);
			text.text = slot.count.ToString();
		}

		else
		{
			text.gameObject.SetActive(false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData) //For showing the item information page.
	{
		//Debug.Log("touched the button");
		if(icon.sprite == null)
		{return;}
		GameManager.instance.itemPageController.Set(new ItemSlot(item));
	}

	public void OnPointerExit(PointerEventData eventData) // For making the item information page disappear. 
	{
		GameManager.instance.itemPageController.Clear();
	}

	public void OnPointerClick(PointerEventData eventData) //Move the item from the slot to the drag and drop item container when clicked. 
	{
		GameManager.instance.character.GainCraftEXP(outputSlot.item.giveCraftEXP); //This gives the player the respective
                                                                                   //amount of crafting EXP specified by the item datasheet.
																				   //It has to be coded here because the KillDoneCrafting() function
																				   //will erase the outputSlot.
		GameManager.instance.dragAndDropController.OnLeftClick(outputSlot);
		icon.enabled = false; //Set the Image to inactive otherwise there will be a white blank when nothing is there.
		text.enabled = false; //Set the Text to inactive otherwise there will be a "9999" when nothing is there.
		icon.sprite = null;
		item = null;

		recipePanel.CheckForCraftable(); //Reset the active/inactive of the craft button as well as the preview after the item is taken out.
		//icon.gameObject.SetActive(false);
		//text.gameObject.SetActive(false);

		crafting. KillDoneCrafting();	
	}


/*	public void Clear()
	{
		item = null;
		icon.sprite = null;
		icon.gameObject.SetActive(false);
		text.text = null;
	}
*/

}
