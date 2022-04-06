using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//This script is attached to different scene's SceneManager object. 
public class PlaceableObjectManager : MonoBehaviour
{
	public PlaceableObjectContainer placeableObjects;	//Make reference to the placeable objects in that scene.
	[SerializeField] Tilemap targetTilemap; //Make reference to the tilemap of the current scene. 
											//We will read the tilemap and place object on the grid.
	[SerializeField] List<Item> randomNaturalResourcesItem; //A list of prefabs that will be randomly spawned into this map. 
	[SerializeField] Tilemap resourceSpawnTilemap; //The tilemap that specifies where can wild plants spawn. 
	[SerializeField] float resourceSpawnPercentage = 0.2f; //The spawning chance of netural resources. 

	public string JSON_string;

	void Start()
	{
		//Pass this manager's values to the GameManager's reference manager.  
		GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().placeableObjectManager = this;
		SpawnRandomResourceNode();//Spawn the natural resources randomly. 
		VisualizeMap(); 	//Load the placeable objects onto the scene.
	}

	private void OnDestroy()	//Clean the reference to the targetObject when the scene is changed or game mode is ended. 
	{
		for(int i = 0; i<placeableObjects.placeableObjects.Count; i++)
		{
			if (placeableObjects.placeableObjects[i].targetObject == null) { continue; }

			IPersistant persistant = placeableObjects.placeableObjects[i].targetObject.GetComponent<IPersistant>();
			if(persistant != null)
            {
				//Call the Save() function which stores and returns information into a string variable.
				string jsonString = persistant.Save();
				//Tell the placed object's objectState what the jsonString is. 
				placeableObjects.placeableObjects[i].objectState = jsonString;
            }


			placeableObjects.placeableObjects[i].targetObject = null;
		}
	}

	//For spawning natural resources randomly in the tilemap.
	private void SpawnRandomResourceNode()
    {
		//Create a list of positions of the tilemap. 
		List<Vector3Int> positions = new List<Vector3Int>();
		foreach (Vector3Int position in resourceSpawnTilemap.cellBounds.allPositionsWithin)
		{
			//Do not di anything if the position does not contain a tile. 
			if (resourceSpawnTilemap.HasTile(position) == false)
			{
				continue;
			}
			positions.Add(position);
		}
		//Spawn natural resource at a random chance. 
		for (int i = 0; i < positions.Count; i++)
		{
			//Check for whether the position has a crop grown on it, if yes, don't spawn natural resources. 
			if(GameManager.instance.GetComponent<CropManager>().cropsManager.Check(positions[i]) == true)
            {
				continue;
            }

			//Generate a random number to determine whether a position can generate an enemy or not. 
			float randomNum = Random.Range(0f, 1f);
			//Generate a random int to determine which enemy to spawn from the list. 
			int randomResource = Random.Range(0, randomNaturalResourcesItem.Count);
			if (randomNum >= (1f - resourceSpawnPercentage))
			{
				//The function Place() contains a checking step to check for whether the grid is registered. 
				Place(randomNaturalResourcesItem[randomResource], positions[i]);
			}
			else
			{
				continue;
			}
		}
	}

	private void VisualizeMap()
	{
		//loop through the placeableObjects list in the PlaceableObjectContainer, and instantiate the object if any placed object is found. 
		for (int i = 0; i<placeableObjects.placeableObjects.Count; i++)	
		{
			//Call the VisualizeItem() function, which instantiate objects onto the designated grid. 
			VisualizeItem(placeableObjects.placeableObjects[i]);	
		}
	}

	private void VisualizeItem(PlaceableObject placeableObject)
	{
		//Instantiate the item that was registered in the PlaceableObjectContainer.
		GameObject go = Instantiate(placeableObject.placedItem.itemPrefab);
		go.transform.parent = transform; 
		//Converting the registered position to a world position, adding offsets.
		Vector3 position = targetTilemap.CellToWorld(placeableObject.positionOnGrid);
		float offset = targetTilemap.cellSize.x / 2f;
		position = new Vector3(position.x + offset, position.y, position.z);
		//Changing the position of the placed item to the registered position.
		go.transform.position = position;
		//Loading object state. 
		IPersistant persistant = go.GetComponent<IPersistant>();
		if(persistant != null) //E.g. the LootContainerInteract is inheriting IPersistant. 
        {
			persistant.Load(placeableObject.objectState);
			Debug.Log("Object state loaded");
        }
		//Assign the targetObject variable in the PlaceableObject class with the item that is placed. 
		placeableObject.targetObject = go.transform; 

	}

	//The Place() function will only be used for placing new objects.
	public void Place(Item item, Vector3Int positionOnGrid)	 
	{

		Debug.Log("1");
		//First check for availability. Do not place the item on the map if the Check() function returns a placeable object.
		if (Check(positionOnGrid) == true) {return;}
		//Create a new placeable object when Place() is called. 
		PlaceableObject placeableObject = new PlaceableObject(item, positionOnGrid);
		//Make this object appear on the map.
		VisualizeItem(placeableObject); 
		//Add this placeable object to the placeable object container list.
		placeableObjects.placeableObjects.Add(placeableObject);


	}

	//For checking if the tile has an object on it.
	public bool Check(Vector3Int position)	 
	{
		//Returns the object if there is one registered in the placeableObjects list. 
		return placeableObjects.Get(position) != null; 
	}

	//A function that removes the object from the list. 
	public void RemoveObject(Transform objectTransform)
	{

		PlaceableObject go = placeableObjects.placeableObjects.Find(x => x.targetObject == objectTransform);
		placeableObjects.placeableObjects.Remove(go);
	}

	//Saving and Loading placeable object manager. 

	[System.Serializable]
	public class PlaceableObjectSaveSata
    {
		public int itemID;
		public int[] pos;
		public string objectState;

		public PlaceableObjectSaveSata(PlaceableObject pObject)
		{
			itemID = pObject.placedItem.ID;
			pos = new int[3];
			pos[0] = pObject.positionOnGrid.x;
			pos[1] = pObject.positionOnGrid.y;
			pos[2] = pObject.positionOnGrid.z;
			objectState = pObject.objectState;
		}

	}

	//For the list of all placeable objects in one particular scene. 
	[System.Serializable]
	public class PlaceableObjectListSaveData
    {
		public List<PlaceableObjectSaveSata> objectList;

		public PlaceableObjectListSaveData(PlaceableObjectContainer container)
        {
			objectList = new List<PlaceableObjectSaveSata>();
			for (int i = 0; i < container.placeableObjects.Count; i++)
			{
				objectList.Add(new PlaceableObjectSaveSata(container.placeableObjects[i]));
			}
        }
	}

	//For the list of all placeableobjects in the game. 
	[System.Serializable]
	public class PlaceableObjectContainerListSaveData
    {
		public List<PlaceableObjectListSaveData> containerList;
		public PlaceableObjectContainerListSaveData(PlaceableObjectContainerList containerList)
        {
			this.containerList = new List<PlaceableObjectListSaveData>();
			for(int i = 0; i < containerList.placeableObjectContainers.Count; i++)
            {
				this.containerList.Add(new PlaceableObjectListSaveData(containerList.placeableObjectContainers[i]));
            }
        }
    }

	public void Save()
    {
		//First go over each placeable object and generate their object state for the current scene. 
		//Did not do this for other scenes as their object states were recorded OnDestroy().
		for (int i = 0; i < placeableObjects.placeableObjects.Count; i++)
		{
			if (placeableObjects.placeableObjects[i].targetObject == null) { continue; }

			IPersistant persistant = placeableObjects.placeableObjects[i].targetObject.GetComponent<IPersistant>();
			if (persistant != null)
			{
				//Call the Save() function which stores and returns information into a string variable.
				string jsonString = persistant.Save();
				//Tell the placed object's objectState what the jsonString is. 
				placeableObjects.placeableObjects[i].objectState = jsonString;
			}
		}

		//Then create the SaveData class. 
		PlaceableObjectContainerListSaveData saveData = new PlaceableObjectContainerListSaveData(GameManager.instance.placeableObjectContainers);
		string fileName = "PlaceableObjectContainers";
		JSON_string = JsonUtility.ToJson(saveData);
		System.IO.File.WriteAllText(Application.persistentDataPath + fileName + ".json", JSON_string);
	}

	public void Load()
    {
		string fileName = "PlaceableObjectContainers";
		string loadTemp = System.IO.File.ReadAllText(Application.persistentDataPath + fileName+ ".json");
		if (loadTemp == "" || loadTemp == "{}" || loadTemp == null) 
		{
			Debug.Log("json not found");
			return; 
		}
		PlaceableObjectContainerListSaveData loadedData = JsonUtility.FromJson<PlaceableObjectContainerListSaveData>(loadTemp);
		
		//Reset all placeable object containers in the game manager. 
		for(int i = 0; i < GameManager.instance.placeableObjectContainers.placeableObjectContainers.Count; i++)
        {
			GameManager.instance.placeableObjectContainers.placeableObjectContainers[i].RemoveAll();
		}
		
		for(int j = 0; j < loadedData.containerList.Count; j++)
        {
			for (int i = 0; i < loadedData.containerList[j].objectList.Count; i++)
			{
				Item importedItem = GameManager.instance.itemDB.items[loadedData.containerList[j].objectList[i].itemID];
				Vector3Int importedPosition = new Vector3Int();
				importedPosition.x = loadedData.containerList[j].objectList[i].pos[0];
				importedPosition.y = loadedData.containerList[j].objectList[i].pos[1];
				importedPosition.z = loadedData.containerList[j].objectList[i].pos[2];

				//Loading the object state of the placeable object, if it has one. 

				string objectState;

				if (loadedData.containerList[j].objectList[i].objectState != null)
				{
					objectState = loadedData.containerList[j].objectList[i].objectState;
				}
				else
				{
					objectState = null;

				}
				placeableObjects.placeableObjects.Add(new PlaceableObject(importedItem, importedPosition, objectState));
			}
		}



	}

}
