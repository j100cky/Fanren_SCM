using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_ChymeContainer : Interactable
{
    public bool isFilled;
    [SerializeField] Mortar mortarScript;
    [SerializeField] Item filledChymeContainer;
    [SerializeField] Item emptyChymeContainer;
    Furnace furnace; 

    public override void Interact(Character character)
    {
        gameObject.SetActive(false);
        mortarScript.isContainerReady = false;
        if (isFilled == false)
        {
            GameManager.instance.inventoryContainer.Add(emptyChymeContainer);
        }
        else
        {
            GameManager.instance.inventoryContainer.Add(filledChymeContainer);
            //Find all the furnaces around the container and give all of them the info about output and process time. 
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);
            foreach(Collider2D c in colliders)
            {
                furnace = c.GetComponent<Furnace>();
                if(furnace != null)
                {
                    furnace.outputItem = mortarScript.outputItem;
                    furnace.processingTime = mortarScript.processingTime;
                }
            }
        }

    }
}
