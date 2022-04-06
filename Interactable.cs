using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public virtual void Interact(Character character)
	{

	}

	public virtual void StopInteract(Character character)
	{
		GamePauseController.instance.isPaused = false; //Resume the game;
	}
}
