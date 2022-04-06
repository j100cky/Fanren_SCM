using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is to manage the connection between the Essential scene and the sene managers in different scenes. 

public class PlaceableObjectReferenceManager : MonoBehaviour	
{
	//Reference to placeable object manager to obtain information about placeable objects in this scene. 
	public PlaceableObjectManager placeableObjectManager;
	//Reference to all placeableobject containers so they can be cleaned by events such as new game. 
	public List<PlaceableObjectContainer> containers;


	public void Place(Item item, Vector3Int pos)
	{


		//If there is no placeable object manager, do not place. 
		if (placeableObjectManager == null)	
		{
			Debug.LogWarning("No placeableObjectsManager reference detected");
			return;
		}
		//Call the real Place() function in the placeableObjectManager script.
		placeableObjectManager.Place(item, pos);

	}

	//A function that calls the Check() function in the PlaceableObjectManager script, when there is one.
	public bool Check(Vector3Int pos)	 
	{
		if(placeableObjectManager == null)	//If there is no placeable object manager, do not run. 
		{
			Debug.LogWarning("No placeableObjectsManager reference detected");
			return false;
		}

		//Call the Check() function in the placeableobjectmanager script. 
		return placeableObjectManager.Check(pos);	
	}

	//Remove all containers that are registered here. Called by events such as a new game. 
	public void RemoveAll()
    {
		for(int i = 0; i < containers.Count; i++)
        {
			containers[i].RemoveAll();
        }
    }

	//Saving and loading
	public void Save()
    {
		if (placeableObjectManager == null)
        {
			Debug.LogWarning("No placeableObjectsManager reference detected");
			return;
		}
		placeableObjectManager.Save();
		//Debug.Log("0");
	}
	public void Load()
	{
		if (placeableObjectManager == null)
		{
			Debug.LogWarning("No placeableObjectsManager reference detected");
			return;
		}
		placeableObjectManager.Load();
		
	}

}
