using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropController : MonoBehaviour
{
	[SerializeField] public ItemSlot itemSlot;
	[SerializeField] GameObject itemIcon;
	[SerializeField] Text itemIconCount;
	RectTransform iconTransform;
	Image itemIconImage;

	private void Start()
	{
		itemSlot = new ItemSlot(null);
		iconTransform = itemIcon.GetComponent<RectTransform>();
		itemIconImage = itemIcon.GetComponent<Image>();
	}

	private void Update()
	{
		if(itemIcon.activeInHierarchy == true)
		{
			iconTransform.position = Input.mousePosition;

			if (Input.GetMouseButtonDown(0))
			{
				if(EventSystem.current.IsPointerOverGameObject() == false) //Throw things away.
				{
					Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					worldPosition.z = 0;

					ItemSpawnManager.instance.SpawnItem(worldPosition, itemSlot.item, itemSlot.count);

					itemSlot.Clear();
					itemIcon.SetActive(false);

				}
			}
			
		}
	}


	internal void OnLeftClick(ItemSlot itemSlot)
	{
		if(this.itemSlot.item == null) //Note that the this.itemSlot is the temporary item holder.//
		{
			this.itemSlot.Copy(itemSlot);
			itemSlot.Clear();
		}
		else
		{
			//Debug.Log("1");
			if(this.itemSlot.item == itemSlot.item && this.itemSlot.item.isStackable == true)
			{
				itemSlot.StackAll(this.itemSlot, itemSlot);
			}
			else
			{
				Item item = itemSlot.item;
				int count = itemSlot.count;
				itemSlot.Copy(this.itemSlot);
				this.itemSlot.Set(item, count);
			}

		}
		UpdateIcon(this.itemSlot);
	}

	internal void OnCraftingLeftClick(CraftItemSlot itemSlot)
	{
		if(this.itemSlot.item == null) //Note that the this.itemSlot is the temporary item holder.//
		{
			this.itemSlot.item = itemSlot.item;
			this.itemSlot.count = itemSlot.count;
			itemSlot.Clear();
		}
		else
		{
			if(this.itemSlot.item == itemSlot.item && this.itemSlot.item.isStackable == true)
			{
				itemSlot.count += this.itemSlot.count;
			}
			else
			{
				Item item = itemSlot.item;
				int count = itemSlot.count;
				itemSlot.item = this.itemSlot.item;
				itemSlot.count = this.itemSlot.count;
				this.itemSlot.Set(item, count);
			}

		}
		UpdateIcon(this.itemSlot);
	}

/*	internal void OnChestLeftClick(ChestItemSlot chestItemSlot) 
	{
		if(this.itemSlot.item == null)
		{
			this.itemSlot.item = chestItemSlot.item;
			this.itemSlot.count = chestItemSlot.count;
			chestItemSlot.Clear();
		}
		else
		{
			if(this.itemSlot.item == chestItemSlot.item && this.itemSlot.item.isStackable == true)
			{
				chestItemSlot. count += this.itemSlot.count;
				this.itemSlot.Clear();

			}
			else
			{
				Item item = chestItemSlot.item;
				int count = chestItemSlot.count;
				chestItemSlot.item = this.itemSlot.item;
				chestItemSlot.count = this.itemSlot.count;
				this.itemSlot.Set(item, count);
			}
			
		}
		UpdateIcon(this.itemSlot);
	}*/

	internal void OnTradePanelLeftClick(ItemSlot itemSlot) 
	{
		if(this.itemSlot.item == null)
		{
			//Because we are trading, we should buy things one at a time
			this.itemSlot.CopyOne(itemSlot);
			itemSlot.ClearOne();
			GameManager.instance.character.LoseMoney(itemSlot.item.purchasePrice);
		}
		else //When the mouse is holding some item. 
		{
			//Stack the item when it is the same item.
			if (this.itemSlot.item == itemSlot.item) 
			{
				itemSlot.ClearOne();
				this.itemSlot.StackOne();
				GameManager.instance.character.LoseMoney(itemSlot.item.purchasePrice);
			}
		}
		UpdateIcon(this.itemSlot);
	}

	internal void OnRightClick(ItemSlot itemSlot)
	{

		if(this.itemSlot.item == null) //Take one at a time if the drag container is empty. 
		{
			if(itemSlot.item.isStackable == true)
			{
				this.itemSlot.item = itemSlot.item;
				this.itemSlot.StackOne();
				itemSlot.ClearOne();
				if(itemSlot.count <=0) //Clear the slot if the item count goes to zero.
				{
					itemSlot.Clear();
				}

			}
			else // When the item is non stackable, we want just the usual item exchange function. 
			{
				this.itemSlot.Copy(itemSlot);
				itemSlot.Clear();

			}

		}
		else
		{
			if(this.itemSlot.item == itemSlot.item && itemSlot.item.isStackable == true) //If the target is the same item as the drag item, add count one at a time.
			{
					this.itemSlot.StackOne();
					itemSlot.ClearOne();
					if(itemSlot.count <=0) //Clear the slot if the item count goes to zero.
					{
						itemSlot.Clear();
					}
			}
			else //When the item is non stackable, or when they are not the same item, simply exchange the items. 
			{
				Item item = itemSlot.item;
				int count = itemSlot.count;
				itemSlot.item = this.itemSlot.item;
				itemSlot.count = this.itemSlot.count;
				this.itemSlot.Set(item, count);
			}
				
		}		

		UpdateIcon(this.itemSlot);
		
	}

	/*internal void OnChestRightClick(ChestItemSlot chestItemSlot)
	{
		if(this.itemSlot.item == null)
		{
			if(chestItemSlot.item.isStackable == true)
			{
				this.itemSlot.item = chestItemSlot.item;
				this.itemSlot.count += 1;
				chestItemSlot.count -= 1;
				UpdateIcon(this.itemSlot);
				return;
			}
			else
			{
				this.itemSlot.item = chestItemSlot.item;
				chestItemSlot.Clear();
				UpdateIcon(this.itemSlot);
				return;
			}

		}
		else
		{
			if(this.itemSlot.item == chestItemSlot.item && chestItemSlot.item.isStackable == true) //If they are the same and is stackable, stack them altogether. 
			{
					this.itemSlot.StackOne();
					chestItemSlot.count -= 1;

			}
			else //If they are either not the same or non stackable, exchange them
			{
				Item item = chestItemSlot.item;
				int count = chestItemSlot.count;
				chestItemSlot.item = this.itemSlot.item;
				chestItemSlot.count = this.itemSlot.count;
				this.itemSlot.Set(item, count);
			}

		}

	UpdateIcon(this.itemSlot);
	}
*/
	internal void OnTradePanelRightClick(ItemSlot itemSlot)
	{

	}

	private void UpdateIcon(ItemSlot dragItemSlot)
	{
		if (itemSlot.item == null)
		{
			itemIcon.SetActive(false);
			
		}
		else
		{
			itemIcon.SetActive(true);
			if(dragItemSlot.item.isStackable == true) //When the item is stackable, show both the count and the sprite. 
			{
				itemIconCount.gameObject.SetActive(true);
				itemIconCount.text = dragItemSlot.count.ToString();
				itemIconImage.sprite = itemSlot.item.icon;
			}
			else //When the item is non stackable, don't show the text, just show the sprite. 
			{
				itemIconCount.gameObject.SetActive(false);
				itemIconImage.sprite = itemSlot.item.icon;
			}
		
			
		}
	}


}
