using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Use Hulu")]

public class HuluOnFurnace : ToolAction
{
    public bool isContainerFilled; //Determined whether the hulu is filled. 
    //public Item filledHulu; //Determines what the hulu will become after collecting the output from the furnace. 

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        //Find colliders at the mouse position. If the collider carries a Furnace script, do the CheckItem() function. 
        Collider2D collider = Physics2D.OverlapCircle(mousePos, 0.01f);
        if (collider == null) { return; }

        if (collider.GetComponent<Furnace>() != null)
        {
            Furnace furnace = collider.GetComponent<Furnace>();
            if (isContainerFilled == true)
            {
                Debug.Log("please use an empty hulu to collect the output.");
                //Some animation. 
            }
            else
            {
                if(furnace.isStillBaking == true)
                {
                    //Add the furnace's outputitem to the inventory.
                    inventory.Add(furnace.outputItem);
                    //Reset the timers of the furnace so it can start the next process. 
                    furnace.ResetTimers();
                    //Remove one hulu from the inventory.
                    inventory.Remove(usedItem);
                }
                else
                {
                    Debug.Log("the furnace is still working on it.");

                }
            }

        }

    }
}
