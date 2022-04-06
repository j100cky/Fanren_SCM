using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPanelController : Interactable
{
	[SerializeField] GameObject craftingPanels;

	public override void Interact(Character character)
	{
		craftingPanels.SetActive(true); //When the furnace is interacted, show the crafting panel. 
	}

	public override void StopInteract(Character character)
	{
		craftingPanels.SetActive(false); //When the interaction is exited, don't show the crafting panel.
	}
}
