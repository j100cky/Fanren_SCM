using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Tool Action/Seed Tile")]
public class SeedTile : ToolAction
{
	public override bool OnApplyToTileMap(Vector3Int gridPosition, 
		TileMapReadController tileMapReadController, 
		Item item)
	{
		if(tileMapReadController.cropManager.Check(gridPosition) == false) //Checking if the tile has been plowed. 
		{
			return false;
		}
		tileMapReadController.cropManager.Seed(gridPosition, item.crop);


		return true;
	}

	public override void OnItemUsed(Item usedItem, ItemContainer inventory)
	{
		inventory.Remove(usedItem);
	}
}
