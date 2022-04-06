using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LootContainerInteract : Interactable, IPersistant
{
	[SerializeField] GameObject closedChest;
	[SerializeField] GameObject openedChest;
	[SerializeField] bool opened;
	[SerializeField] ItemContainer globalBoxContainer;

	//Create a new ItemContainer scriptable object on-site if it is not attached (i.e. not global).
	private void Start()
    {
		if (globalBoxContainer == null)
		{
			Init();
		}
    }

	private void Init()
    {
		globalBoxContainer = (ItemContainer)ScriptableObject.CreateInstance(typeof(ItemContainer));
		globalBoxContainer.Init(); //A method that populate the new item container. 
	}


	//When right click on the chest/box, if it is closed, open it. 
	public override void Interact(Character character)
	{

		if(opened == false)
		{
			opened = true;
			//Change the sprite from closed to opened. 
			closedChest.SetActive(false);
			openedChest.SetActive(true);
			//Ask the BoxInteractController script to use the Open() function. 
			character.GetComponent<BoxInteractController>().Open(globalBoxContainer);
			//Pause game. 
			GamePauseController.instance.SetPause(true);

		}
		else
		{
			opened = false;
			closedChest.SetActive(true);
			openedChest.SetActive(false);

			character.GetComponent<BoxInteractController>().Close();
		}
	}

	//When Esc is pressed, this function is called, which hides the panels, set the sprite from opened to closed, and resumes game. 
	public override void StopInteract(Character character)
	{
		character.GetComponent<BoxInteractController>().Close();
		opened = false;
		closedChest.SetActive(true);
		openedChest.SetActive(false);

		GamePauseController.instance.SetPause(false);
	}

	//Create a class and a constructor for the information that we want to save for the chest object state. 
	[Serializable]
	public class SaveChestItemData
    {
		//We want to save the item's identity and the item's count. 
		public int itemID;
		public int count; 
		
		//A constructor to create such a class. 
		public SaveChestItemData(int id, int c)
		{
			itemID = id;
			count = c;
		}
    }

	//For reasons I don't understand, we need another class to create a list of SaveChestItemData so the JSON system can work properly. 
	[Serializable]
	public class ToSave
    {
		public List<SaveChestItemData> itemDatas; 
		public ToSave()
        {
			itemDatas = new List<SaveChestItemData>();
        }
    }

	//The Save() function is called in the PlaceableObjectManager when the scene is unloaded (leaves the scene or exit game). 
	public string Save()
	{
		//Create a new list of SaveChestItemData using the constructor ToSave(). 
		ToSave toSave = new ToSave();

		//Within this box's container, for each ItemSlot, if the ItemSlot is empty,
        //update the SaveChestItemData with the ID -1 and the count 0. 
		for (int i = 0; i < globalBoxContainer.slots.Count; i++)
		{
			if (globalBoxContainer.slots[i].item == null)
			{
				toSave.itemDatas.Add(new SaveChestItemData(-1, 0));
			}
			//If the ItemSlot is not empty, update the SaveChestItemData with the item ID and the count. 
			else
			{
				toSave.itemDatas.Add(new SaveChestItemData(
					globalBoxContainer.slots[i].item.ID,
					globalBoxContainer.slots[i].count));
			}
		}

		//Use JsonUtility.ToJson to convert the information in toSave into a JSON file.
        //The return value is stored in a string variable (called jsonString) in the PlaceableObjectManager script. 
		return JsonUtility.ToJson(toSave);
	}

	//The Load() function is called in the PlaceableObejctsmanager script when the player enters a scene. 
	public void Load(string jsonString)
    {
		//The jsonString string variable is loaded. If it is null, do not update the chest with the jsonString. 
		if(jsonString == "" || jsonString == "{}"||jsonString == null) { return; }
		//Sometimes the Load() function is conflict with the Start() function. So we call Init() again here.
		if(globalBoxContainer == null) 
		{
			Init(); 
		}

		//Convert the jsonString from string variable to a ToSave variable, and copy that to the toLoad ToSave variable. 
		ToSave toLoad = JsonUtility.FromJson<ToSave>(jsonString);
		//Then, for each SaveChestItemData data in the toLoad's list, create the corresponding ItemSlot into the box's item container. 
		for(int i = 0; i < toLoad.itemDatas.Count; i++)
        {
			if(toLoad.itemDatas[i].itemID == -1)
            {
				globalBoxContainer.slots[i].Clear();
            }
            else
            {
				globalBoxContainer.slots[i].item = GameManager.instance.itemDB.items[toLoad.itemDatas[i].itemID];
				globalBoxContainer.slots[i].count = toLoad.itemDatas[i].count;
            }
        }
    }
}
