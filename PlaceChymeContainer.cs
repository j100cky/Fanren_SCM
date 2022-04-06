using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Place Chyme Container")]
public class PlaceChymeContainer : ToolAction
{
    public bool isContainerFilled;
    public Item emptyContainer;

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        //Find colliders at the mouse position. If the collider carries a Furnace script, do the CheckItem() function. 
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.01f);
        if (collider == null) { return; }
        //If the container is used on a mortar, remove the item from the inventory and make the mortar script set the container active. 
        if (collider.GetComponent<Mortar>() != null) 
        {
            collider.GetComponent<Mortar>().SetContainerActive(isContainerFilled);
            inventory.Remove(usedItem);
            //If a filled container is placed back to the mortar, set the containerReady to false so that the mortar can't run yet. 
            if(isContainerFilled == true)
            {
                collider.GetComponent<Mortar>().isContainerReady = false;
            }
            else //else if an empty container is placed, the mortar's isContainerReady is false. 
            {
                collider.GetComponent<Mortar>().isContainerReady = true;
            }
        }
        //Next code what happen when the filled container is dumped into the furnace. 
        if(collider.GetComponent<Furnace>() != null)
        {
            if(isContainerFilled == false) { return; }
            if(collider.GetComponent<Furnace>().isActive == true)
            {
                Debug.Log("the furnace is burning other stuff");
                return;
            }
            else
            {
                GameObject go = Instantiate(skillPrefab, mousePos, Quaternion.identity);
                inventory.Remove(usedItem);
                inventory.Add(emptyContainer);
                Debug.Log("furnace is activeted");
                collider.GetComponent<Furnace>().BeginProcessing();
            }
        }
        
    }
}
