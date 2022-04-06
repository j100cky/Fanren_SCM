using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    [SerializeField] GlobalBoxPanel globalBoxPanel;
    [SerializeField] InventoryPanel inventoryPanel; 

    //Show the box panel and the inventory panel. 
    //These functions will be called by the Interact() script on the boxes. 
    public void Open(ItemContainer itemContainer)
    {
        //Copy the global box container to targetItemContainer.
        targetItemContainer = itemContainer;
        //Transfer data from targetItemContainer to the GlobalBoxPanel. This is the key to success. 
        globalBoxPanel.inventory = targetItemContainer; 
        //Show both the box panel and the inventory panel. 
        globalBoxPanel.gameObject.SetActive(true);
        inventoryPanel.gameObject.SetActive(true);
    }

    //Hide the box panel and the inventory panel.
    public void Close()
    {
        //Hide both the box panel and the inventory panel. 
        globalBoxPanel.gameObject.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }
}
