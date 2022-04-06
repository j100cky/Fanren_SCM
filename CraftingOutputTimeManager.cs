using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CraftingOutputTimeManager : TimeAgent
{

	[SerializeField] GameObject craftedItemPreview;
    [SerializeField] CraftOutputButton outputButton; 
    [SerializeField] Crafting crafting;
    [SerializeField] FireSourceButton fireSource; //If this component is not null, the crafting time will be divided by the multiplier. 
    float craftingTime; //Store it here temporarily so the craftingTimeMultiplier can work on this. 
    float counter;
    float opacity;

    void Start()
    {
/*        crafting = 
            GameManager.instance.interactingObjectContainer.interactingObjectWithPanels.GetComponent<Crafting>(); //Obtain the crafting script from the object that we are interacting. */
        onTimeTick += Tick;
        Init();
        counter = 0;
        opacity = 0;
    }

    public void Tick()
    {
   
        if(crafting.isCrafting == false) {return;} //Only run the Tick() function when there is nothing crafting. 
        counter += 1f; //For every one second, counter adds 1 value. 
        craftingTime = crafting.currentRecipe.output.item.craftingTime;
        if(fireSource.item != null)
        {
            craftingTime = craftingTime / fireSource.item.craftingTimeMultiplier; //Divide the original crafting time by the multiplier specified in the heat source's data. 
        }
        Debug.Log("time=" + counter ); 
        float percentOpa = counter/craftingTime; //Determine the percent opacity by dividing how long the crafting time is divide by total time needed. 
        craftedItemPreview.GetComponent<Image>().color = new Color(1f,1f,1f,percentOpa); //Use percentOpacity to determine how opaque the image is. When complete, the black mask
        // is fully transparent.
        if(counter >= craftingTime) //When the counter reaches crafting time... 
        {
            outputButton.Set(crafting.currentRecipe.output); //The output button gets the item slot.
            Debug.Log("Done crafting");
            crafting.ShowDoneCrafting(outputButton.outputSlot.item); //use the ShowDoneCrafting() function in the Crafting script which shows the item icon above the furnace 
            //that is crafting that item. 
            counter = 0; //Reset the counter so the next crafting will start again.
            crafting.isCrafting = false; //Make crafting false so the counter won't continue.
            return;
        }
    }
}
