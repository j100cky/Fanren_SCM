using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Tool Action/Place Object")]
public class PlaceObject : ToolAction
{
	public override bool OnApplyToTileMap(Vector3Int gridPosition, 
		TileMapReadController tileMapReadController, 
		Item item)
	{

		//Check if tileMapReadController exists.
		if(tileMapReadController == null)
        {
			Debug.Log("tilemapReadController not found.");
			return false;
        }

		//First, check if there is already an object placed on the designated grid.  
		if (tileMapReadController.objectsManager.Check(gridPosition) == true)	
		{
			Debug.Log("this position has already been occupied.");

			//Do not call the OnApplyToTileMap() method if the position is occupied. 
			return false;	
		}

		//refer to the tilemapcontroller and call the Place function in objects manager. 
		tileMapReadController.objectsManager.Place(item, gridPosition);
		return true;
	}

	//When the item is placed, we want its count to reduce by one. 
	public override void OnItemUsed(Item usedItem, ItemContainer inventory)	
	{
		//Decrease the count by 1 after placement.
		inventory.Remove(usedItem);
	}
}
