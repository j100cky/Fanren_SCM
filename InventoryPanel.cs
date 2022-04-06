using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : ItemPanel
{
	public override void OnLeftClick(int id)
	{
		Debug.Log("0");
		GameManager.instance.dragAndDropController.OnLeftClick(inventory.slots[id]);
		Show();

	}

	public override void OnRightClick(int id)
	{
		if(GameManager.instance.tradePanel.GetComponent<TradeSystem>().isTrading == true && 
			inventory.slots[id].item != null &&
			GamePauseController.instance.isShiftPressed == false) //When it is a right click on the item during trading, we want to sell all.
        {
			//Increase the cash as a product of the selling price and the count.
			GameManager.instance.character.AddMoney(inventory.slots[id].item.sellingPrice); 
			inventory.slots[id].count -= 1; //Clean the slot.
			if(inventory.slots[id].count <= 0)
            {
				inventory.slots[id].item = null;
            }
			//play some animation here, like the item goes up and reduce in opacity. 
		}
/*        else if (Input.GetKey(KeyCode.LeftShift)&& Input.GetMouseButton(1))
        {
				int count = inventory.slots[id].count;
				GameManager.instance.character.AddMoney(inventory.slots[id].item.sellingPrice * count);
				inventory.slots[id].count = 0;
				inventory.slots[id].item = null;
        }*/
        else if (GameManager.instance.tradePanel.GetComponent<TradeSystem>().isTrading == true &&
			inventory.slots[id].item != null &&
			GamePauseController.instance.isShiftPressed == true) //When the shift key is pressed down, bulk sell the item.
        {
			int count = inventory.slots[id].count;
			GameManager.instance.character.AddMoney(inventory.slots[id].item.sellingPrice * count);
			inventory.slots[id].count = 0;
			inventory.slots[id].item = null;
		}
		else
        {
			GameManager.instance.dragAndDropController.OnRightClick(inventory.slots[id]);
        }
		Show();

	}

}
