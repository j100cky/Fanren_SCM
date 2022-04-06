using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Tool Action/Item Placement")] 
public class ItemPlacementBehavior : ToolAction
{
	[SerializeField] GameObject itemPrefab; //Referencing the item prefab object to be placed into the map.//


	public override bool OnApplyToTileMap(Vector3Int gridPosition, //For tools that are used on tiles, like a shovel.//
		TileMapReadController tileMapReadController, 
		Item item)
	{
		Vector3 adjustedGridPosition = new Vector3(gridPosition.x+0.5f, gridPosition.y+0.5f, gridPosition.z);
		GameObject o = Instantiate(itemPrefab, adjustedGridPosition, Quaternion.identity);
		GameManager.instance.player.GetComponent<ToolsCharacterController>().occupiedTiles.Add(gridPosition);


		return true;
	}

	public override void OnItemUsed(Item usedItem, ItemContainer inventory)
	{
		inventory.Remove(usedItem);
	}

}
