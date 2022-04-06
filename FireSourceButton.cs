using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireSourceButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] ItemSlot fireSourceSlot;
	[SerializeField] Image icon;
	[SerializeField] Text text;
	[SerializeField] itemCategory itemCategoryRestriction;
	public Item item; //Make it public so that the CraftingOutputTimeManager can read this. 

	private void Start()
    {
		icon.enabled = false;
		text.enabled = false; //Hide these two components so that the slot won't be having the white blank. 
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

	public void OnPointerClick(PointerEventData eventData)
	{
		/*ItemSlot itemBeingDragged = new ItemSlot();
		itemBeingDragged = GameManager.instance.dragAndDropController.itemSlot.Copy();*/

		GameManager.instance.dragAndDropController.OnLeftClick(fireSourceSlot);
		
		if(fireSourceSlot.item == null) //When clicking on the button to take away the item, we want the button to clear the icon and text.
		{
			icon.sprite = null;
			icon.enabled = false;
			//text.enabled = false;
		}			
		else
		{
			if(fireSourceSlot.item.category == itemCategoryRestriction) //If the item category of the item that is dragged in contains 
			//the heat source item category, then allow the item to be placed into this button's item slot.
			{
				item = fireSourceSlot.item;
				icon.sprite = fireSourceSlot.item.icon;
				icon.enabled = true;
				//text.enabled = true;
			}
			else
			{
				Debug.Log("This is not a heat source!");
				GameManager.instance.dragAndDropController.OnLeftClick(fireSourceSlot); //Transfer the item back to the drag item slot.
			}
		}

	}

}
