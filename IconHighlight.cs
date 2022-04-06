using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IconHighlight : MonoBehaviour
{
	public Vector3Int cellPosition; //For referencing the tilemap since we want the item to be placed within the tiles. 
	Vector3 targetPosition; //A variable to contain the converted world position.
	[SerializeField] Tilemap targetTilemap; //A tilemap variable to let the system know which tilemap to use. We will attach the marker tilemap to here.
	SpriteRenderer spriteRenderer; //Cache the sprite renderer of the icon highlight and later set it to the item's sprite.

	bool canSelect; //When it is determined as selectable in the ToolsCharacterController, this is set true.
	bool show; //When it is determined as having preview in the ToolbarController script, this is set true.

	public bool CanSelect //The value of this variable is given by the ToolsCharacterController script. 
	{
		set{
			canSelect = value;
			gameObject.SetActive(canSelect && show); //Only when both bools are true will the IconHighlight be set active. 
		}
	}

	public bool Show //The value of this variable is given by the ToolbarController script. 
	{
		set{
			show = value;
			gameObject.SetActive(canSelect && show); //Only when both bools are true will the IconHighlight be set active. 
		}
	}

	private void Update()
	{
		targetPosition = targetTilemap.CellToWorld(cellPosition);
		transform.position = targetPosition + targetTilemap.cellSize/2; //Set the icon highlight's position to the mouse position, adding offsets since the tiles are anchored at the bottom left corner. 
	}


	internal void Set(Sprite icon)
	{
		if(spriteRenderer == null) //If the icon highlight does not contain a sprite renderer, which is true at the beginning. 
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
			spriteRenderer.sprite = icon; //Set the sprite renderer to the icon of the item.
	}
}
