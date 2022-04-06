using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapReadController : MonoBehaviour
{
	[SerializeField] Tilemap tilemap;
	public CropManager cropManager; //for managing plowing and croping. 
	public PlaceableObjectReferenceManager objectsManager;	//Referencing the object manager in the GameManager,
                                                            //which contains reference of object manager passed from each scene.


	//[SerializeField] List<TileData> tileDatas;
	//Dictionary<TileBase, TileData> dataFromTiles;

	//private void Start()
	//{
	//	dataFromTiles = new Dictionary<TileBase, TileData>();
	//	foreach (TileData tileData in tileDatas)
	//	{
	//		foreach (TileBase tile in tileData.tiles)
	//		{
	//			dataFromTiles.Add(tile, tileData);
	//		}
	//	}
	//}


	public Vector3Int GetGridPosition(Vector2 position, bool mousePosition = false) 
	{
		if (tilemap == null)
		{
			tilemap = GameObject.Find("BaseTilemap").GetComponent<Tilemap>();
		}

		if(tilemap == null) { return Vector3Int.zero; }

		Vector3 worldPosition;
		if(mousePosition)
		{
			worldPosition = Camera.main.ScreenToWorldPoint(position);
		}
		else
		{
			worldPosition = position;
		}
		Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);

		return gridPosition;
	}

	public TileBase GetTileBase(Vector3Int gridPosition) //Obtain the information of the tile in the BaseTileMap that the mouse is pointing at.
	{
		if (tilemap == null)
		{
			tilemap = GameObject.Find("BaseTilemap").GetComponent<Tilemap>();	//Fing BaseTilemap
		}

		if(tilemap == null) { return null; }

		TileBase tile = tilemap.GetTile(gridPosition); 	//Assign the tile information of the tile the player is pointing at to the return of this function.
		Debug.Log(tile);

		return tile;
	}


	//public TileData GetTileData(TileBase tilebase)
	//{
	//	return dataFromTiles[tilebase];
	//}
}