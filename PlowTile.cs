using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tool Action/Plow")]

public class PlowTile : ToolAction
{
	[SerializeField] List<TileBase> canPlow;
	[SerializeField] int lengthOfBlock = 1; //How big the area can this plow plow in a straight line.
	[SerializeField] int widthOfBlock = 1; 

	public override bool OnApplyToTileMap(Vector3Int gridPosition, TileMapReadController tileMapReadController, Item item)
	{
		if(GameManager.instance.GetComponent<GamePauseController>().isCoolingDown == true)	//Do not continue this function when the isCoolingDown is true.
		{
			Debug.Log("Skill cooling down");
			return false;
		}

		/*		//Obtain the mouse position.
				Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				mousePos = new Vector3(mousePos.x, mousePos.y, 0f);
				//Determine the position of the mouse against the player. 
				Vector3 playerPos = GameManager.instance.player.transform.position;
				float horizontal = mousePos.x - playerPos.x;
				float vertical = mousePos.y - playerPos.y;
				//Determining in which direction will the blocks extend
				if(Mathf.Abs(horizontal) > Mathf.Abs(vertical))
				{
					//If clicking left of the player, each additional block is 1 right of the mouse point. 
					if(horizontal > 0)
					{
						for (int i = 0; i < numOfBlocks; i++)
						{
							Vector3Int pos = new Vector3Int(gridPosition.x + i, gridPosition.y, gridPosition.z);
							gridPositions.Add(pos);
						}
					}
					//If clicking right of the player, each additional block is 1 left of the mouse point. 
					else
					{
						for (int i = 0; i < numOfBlocks; i++)
						{
							Vector3Int pos = new Vector3Int(gridPosition.x - i, gridPosition.y, gridPosition.z);
							gridPositions.Add(pos);
						}
					}
				}
				else
				{
					//If clicking above the player, each additional block is 1 above the mouse point. 
					if (vertical > 0)
					{
						for (int i = 0; i < numOfBlocks; i++)
						{
							Vector3Int pos = new Vector3Int(gridPosition.x, gridPosition.y + i, gridPosition.z);
							gridPositions.Add(pos);
						}
					}
					//If clicking below the player, each additional block is 1 below the mouse point. 
					else
					{
						for (int i = 0; i < numOfBlocks; i++)
						{
							Vector3Int pos = new Vector3Int(gridPosition.x, gridPosition.y - i, gridPosition.z);
							gridPositions.Add(pos);
						}
					}
				}*/

		List<Vector3Int> gridPositions = new List<Vector3Int>();
		//Obtain the character's facing information and register the additional grids according to the direction. 
		float lastHorizontal = GameManager.instance.player.GetComponent<CharacterController2D>().lastMotionVector.x;
		float lastVertical = GameManager.instance.player.GetComponent<CharacterController2D>().lastMotionVector.y;
		if(lastHorizontal == 1f)
        {
			//j is for determining how much width. i is for determining how much length. 
			for(int j = -(widthOfBlock-1)/2; j <= (widthOfBlock-1)/2; j++)
            {
				for (int i = 0; i < lengthOfBlock; i++)
				{
					Vector3Int pos = new Vector3Int(gridPosition.x + i, gridPosition.y + j, gridPosition.z);
					//If the grid position has placeableObjects, don't register it into the plowable positions. 
					if (GameManager.instance.placeableObjects.Check(pos) == true)
					{
						continue; 
					}
                    else
                    {
						gridPositions.Add(pos);
                    }

				}
			}

		}
		else if(lastHorizontal == -1f)
        {
			for (int j = -(widthOfBlock - 1) / 2; j <= (widthOfBlock - 1) / 2; j++)
            {
				for (int i = 0; i < lengthOfBlock; i++)
				{
					Vector3Int pos = new Vector3Int(gridPosition.x - i, gridPosition.y + j, gridPosition.z);
					if (GameManager.instance.placeableObjects.Check(pos) == true)
					{
						continue;
					}
					else
					{
						gridPositions.Add(pos);
					}
				}
            }

		}
		else if(lastVertical  == 1f)
		{
			for (int j = -(widthOfBlock - 1) / 2; j <= (widthOfBlock - 1) / 2; j++)
            {
				for (int i = 0; i < lengthOfBlock; i++)
				{
					Vector3Int pos = new Vector3Int(gridPosition.x + j, gridPosition.y + i, gridPosition.z);
					if (GameManager.instance.placeableObjects.Check(pos) == true)
					{
						continue;
					}
					else
					{
						gridPositions.Add(pos);
					}
				}
			}

		}
		else if(lastVertical == -1f)
        {
			for (int j = -(widthOfBlock - 1) / 2; j <= (widthOfBlock - 1) / 2; j++)
            {
				for (int i = 0; i < lengthOfBlock; i++)
				{
					Vector3Int pos = new Vector3Int(gridPosition.x + j, gridPosition.y - i, gridPosition.z);
					if (GameManager.instance.placeableObjects.Check(pos) == true)
					{
						continue;
					}
					else
					{
						gridPositions.Add(pos);
					}
				}
			}

		}

		//Reset the list of interacting grid positions.
		GameManager.instance.interactingObjectContainer.interactingTiles = new List<Vector3Int>();
		//Instantiating the skill prefab to play the skill's animation.
		GameObject go = Instantiate(skillPrefab);
		//Finding the mouse position and set it to the object's position.
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos = new Vector3(mousePos.x, mousePos.y, 0f);
		go.transform.position = mousePos;
		for (int i = 0; i < gridPositions.Count; i++)
        {
			//Assign the tile that the player is pointing at and the subsequent tiles to this variable tileToPlow.
			TileBase tileToPlow = tileMapReadController.GetTileBase(gridPositions[i]);

			//Add the grids that are interacting to the list of interacting grid. This list will be used by the Plow() function.
			GameManager.instance.interactingObjectContainer.interactingTiles.Add(gridPositions[i]);
		}
		return true;
	}

}
