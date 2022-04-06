using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportBoxInteract : Interactable
{
    [SerializeField] ItemContainer exportBoxContainer; 

    public override void Interact(Character character)
    {
        //The Item on the current inventory button will be added to the export box. 
        exportBoxContainer.Add(GameManager.instance.player.GetComponent<ToolbarController>().GetItemSlot.item,
            GameManager.instance.player.GetComponent<ToolbarController>().GetItemSlot.count);
        //Remove the item from the inventory.
        GameManager.instance.inventoryContainer.slots[GameManager.instance.player.GetComponent<ToolbarController>().selectedTool].Clear();
    }
}
