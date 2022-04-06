using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]	//Mark this class as serializable so that we can configure this class in the inspector.
public class PlaceableObject	//If we were not making a new class, we would have to make a dictionary for each placeable object instead of a list. 
{
	public Item placedItem;
	public Transform targetObject;
	public Vector3Int positionOnGrid;
	/// <summary>
    /// serialized JSON string which contains the current state of the object. 
    /// </summary>
	public string objectState;

	public PlaceableObject(Item item, Vector3Int pos, string objState = null)	//Construct a placeable object.
	{	
		placedItem = item; 
		positionOnGrid = pos;
		objectState = objState;
	}
}



[CreateAssetMenu(menuName = "Data/Placeable Objects Container")]

public class PlaceableObjectContainer : ScriptableObject
{
	//Create a list to store all the placeable objects that are placed in the scene.
	public List<PlaceableObject> placeableObjects;
	//Record the name so each container is saved individually according to each name. 
	public string containerSceneName;

	//A function that returns the placeable object at the specified position.
	internal PlaceableObject Get(Vector3Int position)
	{
		return placeableObjects.Find(x => x.positionOnGrid == position); 
	}

	//Called by other scripts to remove all placeable objects (e.g. new game).
	public void RemoveAll()
    {
		placeableObjects = new List<PlaceableObject>();

    }
}
