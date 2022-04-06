using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
	public Item item;
	public int count;
	public float maxLingqi;
	public float currentLingqi;
	public bool isGodMode = false;

	public ItemSlot(Item importedItem, int count = 1, float maxLingqi = 0f)
    {
		item = importedItem;
		this.count = count;
		this.maxLingqi = maxLingqi;
		currentLingqi = this.maxLingqi;
    }

	public void SetGodMode()
    {
		this.isGodMode = true;
    }

	public void Copy(ItemSlot slot)
	{
		item = slot.item;
		count = slot.count;
	}

	public void CopyOne(ItemSlot slot) //Used for transferring items one at a time, like purchasing or right clicking. 
	{
		item = slot.item;
		count = 1;
	}


	public void Set(Item item, int count)
	{
		this.item = item;
		this.count = count;
	}
	public void Clear()
	{
		if(isGodMode == true)
        {
			return; 
        }
        else
        {
			item = null;
			count = 0;
		}

	}

	public void ClearOne() //Used for getting rid of one item count at a time, like purchsing or right clicking. 
	{
		if(isGodMode == true)
        {
			return;
        }
		count -= 1;
	}

	public void StackOne() //Use to stack items. 
	{
		if(isGodMode == true)
		{ return; }
		count += 1;
	}

	public void StackAll(ItemSlot fromSlot, ItemSlot toSlot) //Used to stack all the items dragging to the item slot pointing. 
	{
		if(isGodMode == true)
        {
			return;
        }

		toSlot.count += fromSlot.count;
		fromSlot.item = null;
		fromSlot.count = 0;
	}
}

[CreateAssetMenu(menuName = "Data/Item Container")]
public class ItemContainer : ScriptableObject
{
	public List<ItemSlot> slots;
	public bool isDirty; //For the item tool bar to update. When isDirty is true, the item panel will update the count. 
	public bool isGodMode = false;  //If it is god mode, item count will not decrease. 

	//Populate the item container if it is a newly created one. 
	internal void Init()
    {
		slots = new List<ItemSlot>();
		for(int i = 0; i<40; i++)
        {
			slots.Add(new ItemSlot(null));
        }
    }


	public void Add(Item item, int count = 1) //this method will add the number of drops into the inventory.
	{
		isDirty = true; 
		if(item.isStackable == true) //if an item is stackable...
		{
			ItemSlot itemSlot = slots.Find(x => x.item == item); //see if the item is already exist in the inventory
			if (itemSlot != null) //if it is in the inventory...
			{
				itemSlot.count += count; //we will add the number of how many picked up to the existing number. 
			}
			else //if the stackable item is not in the inventory...
			{
				itemSlot = slots.Find(x => x.item == null); //we will find a empty slot
				if (itemSlot != null) //only when the invntory is not full
				{
					itemSlot.item = item; //we will update the empty slot with the item's parameters (image and number)
					itemSlot.count = count; // the number will be count, which is by default 1.
				}
			}
		}
		else
		{
			ItemSlot itemSlot = slots.Find(x => x.item == null); // if the item is non stackable, add the item into a new slot
			if (itemSlot != null) //only when the invntory is not full
			{
				itemSlot.item = item; 
			}
		}
	}

	public bool CheckCount(Item itemToCheck, int consumed)
    {
		ItemSlot itemSlot = slots.Find(x => x.item == itemToCheck);
		if(itemSlot.count > consumed)
        {
			//Return true to indicate that the item to be consumed is enough. 
			return true;
        }
        else
        {
			return false;
        }
	}

	public void Remove(Item itemToRemove, int count = 1)
	{
		isDirty = true;
		if(itemToRemove.isStackable) //reduce the item count by a default of 1 when the Remove() function is called is the item is stackable. 
		{
			if(isGodMode == false)
            {
				ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
				if(itemSlot == null) {return;}
				itemSlot.count -= count;
				if(itemSlot.count <= 0)
				{
					itemSlot.Clear();
				}
            }
            else
            {
				return;
            }

		}
		else{
			while(count > 0)
			{
				if(isGodMode == false)
                {
					count -= 1;
					ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
					if (itemSlot == null) { return; }
					itemSlot.Clear();
				}

                else
                {
					return;
                }


			}
		}
	}

	internal bool CheckFreeSpace() //Check to see if there is more free space in the inventory.//
	{
		for(int i = 0; i < slots.Count; i++)
		{
			if(slots[i].item == null)
			{
				return true;
			}
		}

		return false;
	}

	internal bool CheckItem(ItemSlot checkingItem) //Checking to see if the item is already present in the inventory.//
	{
		ItemSlot itemSlot = slots.Find(x => x.item == checkingItem.item);
		if(itemSlot == null) { return false; }
		if(checkingItem.item.isStackable){ return itemSlot.count > checkingItem.count;}

		return true;
	}

/*	public int ReturnItemCount(string itemName) //This function is used by the QuestGoal script to obtain the currentAmount
                                                //of the item in the quest was looking for. 
    {
		ItemSlot itemSlot = slots.Find(x => x.item == itemName); //Find the itemslot that bears the same item name as
																	  //the argument when this function is called. 
		if(itemSlot == null) {return 0; } //If the item is not found, the number is going to be zero. 
												   //return returnItemCountSlot.count; //If the item is found, the return value will be the itemslot's count
		return itemSlot.count;
    }*/
	
	//Called be other script (such as new game) to clear the container. 
	public void ClearAll()
    {
		for(int i = 0; i < slots.Count; i++)
        {
			slots[i].Clear();
        }
    }

}
