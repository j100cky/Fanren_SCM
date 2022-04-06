using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
[Serializable]
public class CraftItemSlot
{
	public Item item; 
	public int count;

	public void Set(Item item, int count)
	{
		this.item = item;
		this.count = count;
	}

	public void Clear()
	{
		item = null;
		count = 0;
	}
}

public class CraftingBoardItemcontainer : MonoBehaviour
{
	public List<CraftItemSlot> slots;

	public void Add(Item item, int count = 1)
	{
		if(item.isStackable == true)
		{
			CraftItemSlot craftItemSlot = slots.Find(x => x.item == item); //Check to see if the item is present in the inventory
			if(craftItemSlot != null) //If the item is present in the inventory..
			{
				craftItemSlot.count += count; //Add the count to the existing item count.
			}
			else //if the item is stackable but new to the inventory
			{
				craftItemSlot = slots.Find(x => x.item == null);//First find an empty slot.
				if(craftItemSlot != null) // If an empty slot is found.
				{
					craftItemSlot.item = item; //update the craft item slot with the item that was passed in
					craftItemSlot.count = count;
				}
			}
		}
		else //if the item is not stackable
		{
			CraftItemSlot craftItemSlot = slots.Find(x => x.item == null); //Find an empty slot
			if(craftItemSlot != null) //If an empty slot is found
			{
				craftItemSlot.item = item;
			}
		}
	}

	public void Remove(Item itemToRemove, int count = 1)
	{
		if(itemToRemove.isStackable == true) //If the item is stackable, we want to remove 1 count at a time.
		{
			CraftItemSlot craftItemSlot = slots.Find(x=>x.item == itemToRemove); //Find this item in the item container.
			if(craftItemSlot == null) { return; }//If this item is not found in the slots, do nothing.
			craftItemSlot.count -= count;
			if(craftItemSlot.count <= 0) //If the count of the item reaches 0, clear the item from the slot.
			{
				craftItemSlot.Clear();
			}
		}
		else //If the item is not stackable, clear the item from the slot directly. 
		{
			while(count>0)
			{
				count -= 1;
				CraftItemSlot craftItemSlot = slots.Find(x => x.item == itemToRemove);//Find this item in the item container.
				if(craftItemSlot == null) { return; }//If this item is not found in the slots, do nothing.
				craftItemSlot.Clear(); //If this item is found, clear it from the slot directly. 
			}
		}
	}

	internal bool CheckFreeSpace()
	{
		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[1].item == null)
			{
				return true;
			}
		}
			return false;
	}

	internal bool CheckItem(ItemSlot checkingItem)
	{
		CraftItemSlot craftItemSlot = slots.Find(x => x.item == checkingItem.item);
		if(craftItemSlot == null) {return false;}
		if(checkingItem.item.isStackable == true)
		{
			return craftItemSlot.count > checkingItem.count;
		}
		return true;
	}
}
