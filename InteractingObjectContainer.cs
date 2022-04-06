using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[Serializable]
public class InteractingObject
{
	public ToolHit objectBeingHit;
	public Vector3Int positionOnGrid;

	public InteractingObject(ToolHit hit, Vector3Int pos)
	{
		objectBeingHit = hit;
		positionOnGrid = pos;
	}
}
*/

public class InteractingObjectContainer : MonoBehaviour
{
	public List<Transform> interactingObjects;	//Store the list of interacting objects. Reset every time OnApply() is used. 
	public List<Transform> brokenObjects;	//Store the objects that have run out of health, will be broken when OnApply() is called. 
	public List<Vector3Int> interactingTiles; //To contain the tiles that the plow is trying to plow.
	//public Transform interactingObjectWithPanels; //To contain the object that needs a panel. i.e. furnace, NPCs...

	/*internal Transform Get(string name)	//A function that finds the name of the interacting object in the list.
	{
		return interactingObjects.Find(x => x.name == name);
	}*/

	//We actually don't need this because we are refreshing the list every time OnApply() is called. 
	/*public bool Check(string name)	//Returns true or false after trying to find an object in the list of interacting objects. 
	{
		return Get(name) != null;
	}*/
}


