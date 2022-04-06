using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : MonoBehaviour
{
    public ItemContainer inventory;
	public List<InventoryButton> buttons;

	
	private void Start()
	{
		Init();
	}

	public void Init()
	{
		SetIndex();
		Show();
	}

	private void LateUpdate() //Check every late update for the value of isDirty. If it is true, that means the inventory is changed.
                              //It should update the item count or icon. 
	{
		if(inventory.isDirty)
		{
			Show();
			inventory.isDirty = false;
		}
	}

	private void OnEnable()
	{
		Show();
	}

	private void SetIndex() //cycles through the inventory to update the inventory panel
	{
		if(buttons != null)
        {
			for(int i = 0; i < buttons.Count; i++)
			{
				buttons[i].SetIndex(i);
			}
        }

	}

	public virtual void Show() //if there is an item in that index, update the button, if not, hide the button's icon.
                               //This mimic the item getting attached to the mouse.
	{
		if(buttons != null)
        {
			for(int i = 0; i < inventory.slots.Count && i < buttons.Count; i++)
			{
				if(inventory.slots[i].item == null)
				{
					buttons[i].Clean();
				}
				else
				{
					buttons[i].Set(inventory.slots[i]);
				}
			}
        }


	}

	public virtual void OnLeftClick(int id)
	{

	}

	public virtual void OnRightClick(int id)
	{

	}

	public virtual void OnShiftRightClick(int id)
    {

    }

	[Serializable]
	public class SavedContainerItemData
    {
		public int itemID;
		public int count;
		public SavedContainerItemData(int ID, int c)
        {
			itemID = ID;
			count = c;
        }
    }

	[Serializable]
	public class ListOfItemData
    {
		public List<SavedContainerItemData> itemDatas;
		public ListOfItemData()
        {
			itemDatas = new List<SavedContainerItemData>();
        }
    }

	[SerializeField] string jsonString;

	public virtual void SaveContainerData()
    {
		ListOfItemData listOfItemData = new ListOfItemData();

		for(int i = 0; i < inventory.slots.Count; i++)
        {
			if (inventory.slots[i].item == null)
			{
				listOfItemData.itemDatas.Add(new SavedContainerItemData(-1, 0));
			}
            else
            {
				listOfItemData.itemDatas.Add(new SavedContainerItemData(inventory.slots[i].item.ID, 
					inventory.slots[i].count));
			}
        }

		//Save the data into the local disk.
		System.IO.File.WriteAllText(Application.persistentDataPath + "/InventoryData.json", JsonUtility.ToJson(listOfItemData));
    }

	public virtual void LoadContainerData()
    {
		string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/InventoryData.json");
		
		if(jsonString == "" || jsonString == null) { return; }

		ListOfItemData listOfItemData = JsonUtility.FromJson<ListOfItemData>(jsonString);

		for(int i = 0; i < listOfItemData.itemDatas.Count; i++)
        {
			if(listOfItemData.itemDatas[i].itemID == -1)
            {
				inventory.slots[i].Clear();
            }
            else
            {
				inventory.slots[i].item = GameManager.instance.itemDB.items[listOfItemData.itemDatas[i].itemID];
				inventory.slots[i].count = listOfItemData.itemDatas[i].count;
            }
        }
    }
}

