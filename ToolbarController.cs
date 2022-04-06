using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarController : MonoBehaviour
{
    [SerializeField] int toolbarSize = 10;
    public int selectedTool;
    public bool isToolbarActive = true;

    [SerializeField] IconHighlight iconHighlight; //For referencing the variables and functions in the IconHighlight script. 

    public Action<int> onChange;

    public Item GetItem
    {
        get{
            return GameManager.instance.inventoryContainer.slots[selectedTool].item;
        }
    }

    public ItemSlot GetItemSlot
    {
        get
        {
            return GameManager.instance.inventoryContainer.slots[selectedTool];
        }
    }

    private void Start()
    {
        onChange += UpdateHighlightIcon;
        UpdateHighlightIcon(selectedTool);
    }

    private void Update()
    {
        if(isToolbarActive == false) 
        {
            iconHighlight.Show = false;
            return; 
        }
    	float delta = Input.mouseScrollDelta.y;
    	if(delta != 0)
    	{
    		if(delta > 0)
    		{
    			selectedTool -= 1;
                selectedTool = (selectedTool < 0 ? toolbarSize - 1 : selectedTool);
    		}
    		else
    		{
    			selectedTool +=1;
                selectedTool = (selectedTool >= toolbarSize ? 0 : selectedTool);
    		}
    		onChange?.Invoke(selectedTool);
    	}
    }

    internal void Set(int id)
    {
        selectedTool = id;
    }

    public void UpdateHighlightIcon(int id = 0)//This function will determine whether the item has a icon highlight.
                                               //The inventory button will pass the value of id into this parameter.
    {
        Item item = GetItem;
        if(item == null) //In case an empty slot is selected. Hide the iconHighlight, and skip the method. 
        {
            iconHighlight.Show = false;
            return;
        }
        iconHighlight.Show = item.iconHighlight;
        if(item.iconHighlight == true) //If the item is previewable, we will change the sprite of the icon highlight to the sprite of the item for preview. 
        {
            iconHighlight.Set(item.icon); //The function Set (written in the IconHighlight script) will
                                          //replace the sprite of the icon highlight with the sprite of the item.
        }
    }

}
