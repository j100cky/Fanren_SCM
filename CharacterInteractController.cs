using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
	CharacterController2D characterController;

	Rigidbody2D rbody;
	[SerializeField] float offsetDistance = 1f; 
	[SerializeField] float sizeOfInterableArea = 0.2f; //Define the size of mouse pointer for clicking an object. 
	[SerializeField] float rangeOfInteractArea = 2;

	Character character;

	[SerializeReference] HighlightController highlightController; //we cannot directly access HighlightController script, so we need to make a reference here.

	Interactable hit; //Refencing the Interactable class of the collider 
	Operatable ope; //Referencing the Operatable class of the collider. 

	private void Awake()
	{
		characterController = GetComponent<CharacterController2D>();
		rbody = GetComponent<Rigidbody2D>();
		character = GetComponent<Character>();
	}


		private void Update()
	{
		Check(); //used to check whether the char is walking past the target to show the highlighter. 

		if(Input.GetMouseButtonDown(1))//interact using the right mouse click
		{
			Interact(); //perform the function Interact when the right mouse is clicked. 
		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(hit != null)
			{
				hit.StopInteract(character); //Perform the function StopInteract in different scripts inheriting the Interactable script.These are usually setting the UI inactive.//
			}
			
		}
		//Another situation is to operate objects. When the right mouse button is hold, an object can be operated. 
		if(Input.GetMouseButton(1))
        {
				Operate();
        }
        else
        {
			if (ope != null)
			{
				ope.StopOperate();
			}

        }
	}

	private void Check() //defining the Method check.
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));	//Obtaining where the mouse is pointing at.
		Vector2 position = rbody.position + characterController.lastMotionVector * offsetDistance;
		float distance = (mousePos - rbody.position).sqrMagnitude; //Calculating casting range.
		if(distance > GameManager.instance.character.mentality) 
		{
			highlightController.Hide(); //when the player leaves, hide the highlighter icon. 
			return;
		}

		Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, sizeOfInterableArea);


		foreach(Collider2D c in colliders)
		{
			Interactable hit = c.GetComponent<Interactable>();
			if(hit != null) //if the player is walking by and facing the interactable
			{
				highlightController.Highlight(hit.gameObject);
				//Debug.Log("Interactable detected.");
				return;
			}

			highlightController.Hide(); //when the player leaves, hide the highlighter icon. 
		}
	}

	//This function describes the mechanism of interaction. 
	private void Interact() 
	{
		//Obtaining where the mouse is pointing at.
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));	
		Vector2 position = rbody.position + characterController.lastMotionVector * offsetDistance; 
		float distance = (mousePos - rbody.position).sqrMagnitude; //Calculating casting range.
		if(distance > GameManager.instance.character.mentality) //Not interact if the player is too far from the NPC/object.
		{
			Debug.Log("distance too far");
			return;
		}

		//Store all colliders 
		Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, sizeOfInterableArea);

		//Store the collider's Interactable component to the variable "hit"
		foreach (Collider2D c in colliders)
		{
			hit = c.GetComponent<Interactable>();
			//Sometimes the interactable is overlapped with buildings that have boxcolliders. When this happens, move on to next collider. 
			if(hit == null)
            {
				continue;
            }
            else
            {
				//do the Interact function in the interactable script. 
				hit.Interact(character);

				//Only interact with the first one.
				break;
			}
		}
	}

	private void Operate()
    {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		Vector2 position = rbody.position + characterController.lastMotionVector * offsetDistance;
		float distance = (mousePos - rbody.position).sqrMagnitude; //Calculating casting range.
		if (distance > GameManager.instance.character.mentality) //Not interact if the player is too far from the NPC/object.
		{
			Debug.Log("distance too far");
			return;
		}

		Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.01f);
		if(collider == null) { return; }
		ope = collider.GetComponent<Operatable>();
		if (ope != null)
		{
			ope.Operate();
		}
		else 
		{
			Debug.Log("ope is not found");
		}

	}
}
