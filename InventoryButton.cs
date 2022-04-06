using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] Image icon;
	[SerializeField] Text text;
	[SerializeField] Image highlight;
	public ItemSlot itemSlot; //This is for the reference of ItemPage.

	int myIndex;
	int myChestIndex;
	int myTradePanelIndex;
	int myCraftPanelIndex;

	public void SetIndex(int index)
	{
		myIndex = index;
	}

	public void SetTradePanelIndex(int index)
	{
		myTradePanelIndex = index;
	}

/*	public void SetChestIndex(int index)
	{
		myChestIndex = index;
	}*/

	public void SetCraftPanelIndex(int index)
	{
		myCraftPanelIndex = index;
	}

	int num = 1;

	//Setting up the icon sprite and text for each slot, when they are assigned.  This is called by the item panel.
	public void Set(ItemSlot slot) 
	{
		itemSlot = slot; //passing the information of the item in that slot into the item variable so that the item page can use this item's information. 
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

/*	public void EnableIcon() //Sometimes I have to disable the icon on Start(). This function will enable the icon when it is needed. 
    {
		icon.enabled = true;
    }*/

/*	public void ChestSet(ChestItemSlot chestSlot) //Setting up the icon sprite and text for each chest slot, when they are assigned.  
	{
		item = chestSlot.item; //Ensures that when entering a button that carries a chest item slot will show the correct item information. 
		icon.sprite = chestSlot.item.icon;
		icon.gameObject.SetActive(true);

		if (chestSlot.item.isStackable == true)
		{
			text.gameObject.SetActive(true);
			text.text = chestSlot.count.ToString();
		}

		else
		{
			text.gameObject.SetActive(false);
		}
	}*/

	public void CraftSet(CraftItemSlot craftSlot) //Setting up the icon sprite and text for each craft slot, when they are assigned. 
	{
		Item item = craftSlot.item; //Ensures that when entering a button that carries a craft item slot will show the correct item information. 
		icon.sprite = craftSlot.item.icon;
		icon.gameObject.SetActive(true);

		if(craftSlot.item.isStackable == true)
		{
			text.gameObject.SetActive(true);
			text.text = craftSlot.count.ToString();
		}
		else
		{
			text.gameObject.SetActive(false);
		}

	}

	public void Clean() //This method is to remove the icon and text when the item is used up. 
	{
		icon.sprite = null;
		icon.gameObject.SetActive(false);
		text.gameObject.SetActive(false);
		itemSlot = null;
	}

	public void OnPointerEnter(PointerEventData eventData) //Showing the item information page.
	{
		if(icon.sprite == null)
		{
			return;
		}
		GameManager.instance.itemPageController.Set(itemSlot);
	}

	public void OnPointerExit(PointerEventData eventData) //Hiding the item information page.
	{
		GameManager.instance.itemPageController.Clear();
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		ItemPanel itemPanel = transform.parent.GetComponent<ItemPanel>();
		TradeItemPanel tradeItemPanel = transform.parent.GetComponent<TradeItemPanel>();
		/*ChestItemPanel chestItemPanel = transform.parent.GetComponent<ChestItemPanel>();*/
		RecipePanel craftingPanel = transform.parent.GetComponent<RecipePanel>();
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (itemPanel != null)
			{
				itemPanel.OnLeftClick(myIndex);
				//Debug.Log(itemPanel);
			}
            else
            {
				//Debug.Log("9");
            }

			if (tradeItemPanel != null)
			{
				tradeItemPanel.OnTradePanelLeftClick(myTradePanelIndex);

			}
			/*if (chestItemPanel != null)
			{
				chestItemPanel.OnChestLeftClick(myChestIndex);
				//Debug.Log("onchestclick called");
			}*/
			if (craftingPanel != null)
			{
				craftingPanel.OnCraftingLeftClick(myCraftPanelIndex);
				//Debug.Log("onCraftLeftClick called");
			}
		}
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (itemPanel != null)
			{
				itemPanel.OnRightClick(myIndex);
				//Debug.Log("itemPanel onClick used");
			}

			if (tradeItemPanel != null)
			{
				tradeItemPanel.OnTradePanelRightClick(myTradePanelIndex);
				//Debug.Log("tradeItemPanel onClick used");
			}
			/*if (chestItemPanel != null)
			{
				chestItemPanel.OnChestRightClick(myChestIndex);
				//Debug.Log("onchestclick called");
			}*/
		}
	}



    public void Highlight(bool b) 
	{
		highlight.gameObject.SetActive(b);
	}
}
