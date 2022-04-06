using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPageController : MonoBehaviour
{
	//[SerializeField] Item item;
	[SerializeField] Text itemName;
	[SerializeField] Text itemCategory;
	[SerializeField] Text itemDescription;
	[SerializeField] Image itemImage;
	[SerializeField] Text maxLingqi;
	[SerializeField] Text currentLingqi;
	[SerializeField] Text itemPurchasePrice;
	[SerializeField] Text itemSellingPrice;
	[SerializeField] GameObject purchasePricePanel;
	[SerializeField] GameObject sellingPricePanel;
	[SerializeField] GameObject restoreHPPanel;
	[SerializeField] GameObject restoreMPPanel;
	[SerializeField] GameObject lingqiPanel;


	//[SerializeField] InventoryButton inventoryButton;
	Vector3 offset = new Vector3(2,-2,0); //Position offset so that the information page will not get in the way of the mouse. 

	public void Set(ItemSlot itemSlot)
	{
		Item item = itemSlot.item;
		gameObject.SetActive(true);
		transform.position = Input.mousePosition + offset;
		itemImage.sprite = item.icon;
		itemName.text = item.Name;
		itemCategory.text = item.category.ToString();
		itemDescription.text = item.itemDescription;
		
		if(itemSlot.maxLingqi != 0)
        {
			lingqiPanel.SetActive(true);
			maxLingqi.text = itemSlot.maxLingqi.ToString();
			currentLingqi.text = itemSlot.currentLingqi.ToString();
        }
		else
        {
			lingqiPanel.SetActive(false);
        }


		if(item.restoreHP != 0)
        {
			restoreHPPanel.SetActive(true);
			restoreHPPanel.GetComponentInChildren<Text>().text = item.restoreHP.ToString();
        }
        else
        {
			restoreHPPanel.SetActive(false);
		}

		if(item.restoreMP != 0)
        {
			restoreMPPanel.SetActive(true);
			restoreMPPanel.GetComponentInChildren<Text>().text = item.restoreMP.ToString();
		}
        else
        {
			restoreMPPanel.SetActive(false);
		}

		itemPurchasePrice.text = "¹ºÂò: " + item.purchasePrice.ToString();
		itemSellingPrice.text = "Âô³ö: " + item.sellingPrice.ToString();
		ShowPrice();

	}

	private void ShowPrice() //Determine whether to show the price or not depending on whether the player is trading. 
    {
		if(GameManager.instance.tradePanel.GetComponent<TradeSystem>().isTrading == true)
        {
			purchasePricePanel.SetActive(true);
			sellingPricePanel.SetActive(true);
		}
        else
        {
			purchasePricePanel.SetActive(false);
			sellingPricePanel.SetActive(false);
		}
    }

	public void Clear()
	{
		gameObject.SetActive(false);
	}

}
