using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolsCharacterController : MonoBehaviour
{
	CharacterController2D character;
	Rigidbody2D rbody;
	[SerializeField] float offsetDistance = 1f; 
	//[SerializeField] float sizeOfInterableArea = 1.2f; define the two distances used in item collection. 
	[SerializeField] MarkerManager markerManager; //for marking up tiles//
	[SerializeField] TileMapReadController tileMapReadController; //for highlighting tiles//
	[SerializeField] float maxDistance; //Use to specify how far can a tile be selected//
	[SerializeField] ToolAction onTilePickUp; //for picking up crops from the tiles.
 
	ToolbarController toolbarController; // for tool using 
	Animator animator; //for animation of actions//

	Vector3Int selectedTilePosition;
	bool isSelectable;
	public List<Vector3> occupiedTiles; //Create a list to store the tile positions that have an object on it.//



	[SerializeField] IconHighlight iconHighlight; //Referencing the IconHighlight script so we can modify the variables in there, such as icon highlight position.


	private void Awake()
	{
		character = GetComponent<CharacterController2D>();
		rbody = GetComponent<Rigidbody2D>();
		toolbarController = GetComponent<ToolbarController>();
		animator = GetComponent<Animator>();
		maxDistance = GetComponent<Character>().useToolRange;
	}

	private void Start()
	{
		occupiedTiles = new List<Vector3>();
	}

	private void Update()
	{
		if(GameManager.instance.GetComponent<GamePauseController>().isPaused == true) {return;} //Not implementing anything related to tools if isPaused == true.
		if(toolbarController.isToolbarActive == false) { return; } //Do not use tools in the toolbar if the skillbar is interacting. 
		SelectTile();
		CanSelectCheck();
		Marker(); //for highlighting tiles//
		if(Input.GetMouseButtonDown(0))
		{
			if(UseToolWorld() == true) //It there is a tree/rock, we don't want to cut the tree and then simultaneously plow the land. 
			{
				return;
			} //perform the function UseTool when the left mouse is clicked. 
			UseToolGrid(); //perform the plow() function after the useToolWorld function. 
		}
		if(Input.GetMouseButtonDown(1)) //When right click, call the useItem() funciton. 
        {
			UseItem();
		}
	}


	private bool UseToolWorld() //define the function UseTool to world objects, those with colliders. 
	{
		Vector2 position = rbody.position + character.lastMotionVector * offsetDistance; 
		
		Item item = toolbarController.GetItem;
		if(item == null) {return false;} //checking if there is a tool selected in the toolbar.
		if(item.onAction == null) {return false;} //checking if the tool has an action script attached to it. 

		animator.SetTrigger("act");
		bool complete = item.onAction.OnApply(position);

		if (complete == true)
		{
			if(item.onItemUsed != null) //If this item is also a consumable.
			{
				item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);	//Do whatever the OnItemUsed function does.
			}

			/*if(item.onItemPlacement != null)
			{
				item.onItemPlacement.OnItemPlacement(item, GameManager.instance.inventoryContainer, position);
			}*/
		}

		return complete;
	}

	private void SelectTile()
	{
		if(tileMapReadController != null)
        {
			selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
        }
        else { return; }

	}

	void CanSelectCheck()
	{
		Vector2 characterPosition = transform.position;
		Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		isSelectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
		markerManager.Show(isSelectable);
		iconHighlight.CanSelect = isSelectable;
	}

	private void Marker()
	{
		markerManager.markedCellPosition = selectedTilePosition;
		iconHighlight.cellPosition = selectedTilePosition; //Saving the selectedTilePosition to the iconHighlight's position so the highlight will appear at the mouse position.
	}

	private void UseToolGrid()
	{
		if(isSelectable == true)
		{
			Item item = toolbarController.GetItem;
			//If the player is empty handed, pick up the tile's crop.
			if (item == null) 
			{
				PickUpTile();
				return;
			}
			//If the player is holding something, but the item does not have a onTileMapAction action attached to it, return. 
			if (item.onTileMapAction == null){return;}
			//Skip this if the selectedTile is contained in the list of tiles that are occupied.
			if (occupiedTiles.Contains(selectedTilePosition)) {return;}

			animator.SetTrigger("act");
			bool complete = item.onTileMapAction.OnApplyToTileMap(selectedTilePosition, 
				tileMapReadController, 
				item);

			if (complete == true)
			{
				if(item.onItemUsed != null)
				{
				item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer);
				}
			}
		}
	}
	//Create a separate item use function so that the OnItemUsed() function of an item can be directly called by mouse right click.
	//This is used for consumption, skills, etc.
	private void UseItem() 
    {
		//Check for whether the mouse is clicking on an Interactable object. If yes, don't consume the item. 
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		//Vector2 position = new Vector2(transform.position.x, transform.position.y);
		float distance = (mousePos - rbody.position).sqrMagnitude; //Calculating casting range.

		if (distance < GameManager.instance.character.mentality)
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, 10f);
			if(colliders != null)
            {
				foreach (Collider2D collider in colliders)
				{
						if (collider.GetComponent<Interactable>() != null)
						{
							return;
						}
				}
			}

		}
		

		Item item = toolbarController.GetItem; //Get the currently-holding item

		//If there is no item or the item does not have an action for the onItenUsed variable, dont do anything. 
		if (item == null || item.onItemUsed == null) 
		{
			Debug.Log("there is no item selected or the item does not have a onItemUsed designated.");
			return; 
		}
		//Call the OnItemUsed() function in that item's item action. 
		item.onItemUsed.OnItemUsed(item, GameManager.instance.inventoryContainer); 
	}


	private void PickUpTile()
	{
		if(onTilePickUp == null) {return;}

		onTilePickUp.OnApplyToTileMap(selectedTilePosition, tileMapReadController, null);
	}
}
