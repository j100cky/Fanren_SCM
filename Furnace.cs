using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script describes the action of the furnace, mimicking the Overcooked game play. 
public class Furnace : TimeAgent
{
    public bool isActive = false; //Describe the working status of the furnace. When active, it will ask for the next item on the recipe's list.
                                  //When inactive, the next item needs to be a recipe. 
    public Item outputItem;
    [SerializeField] GameObject itemIconPrefab; //For the icon that will show up above the furnace. 
    DoneCraftingNoticeScript doneCraftingScript; //For storing the donecrafting script of the itemIconPrefab;
    public int processingTimer;
    public int processingTime; //Used to copy the process time of the imported chyme
    int bakingTimer; //Used to store how long has the output been baked. 
    [SerializeField] int bakingTime = 5; //Used to determine how long can the output be baked without being destroyed. Defalut is 5 game-seconds.  
    public SpriteRenderer furnaceLight; //The sprite that changes color. 
    private Coroutine furnaceLightChange; //Need this to reference the right coroutine so it can be stopped. 
    private bool coroutineActive;
    public bool isStillBaking; //Determine whether the output is left in the furnace or taken out already. 

    private void Start()
    {
        processingTimer = -1;
        //ingredientIndex = 0;
        onTimeTick += Tick;
        Init();
    }

    //This function is called when a filled chymecontainer is placed onto the furnace. 
    public void BeginProcessing()
    {
        //Set isActive to true so that the next chymecontainer will not load. 
        isActive = true;
        //Start the coroutine and store the coroutine into the furnaceLightChange variable. 
        furnaceLightChange = StartCoroutine(ChangeLightColor());
        //Set process timer to 0 so that the Tick() can run. 
        processingTimer = 0;
    }

/*    //This function is called when an item is thrown in (by the item's ToolAction script). 
    public bool CheckItem(Item usedItem)
    {
        //Check whether the item is a recipe or a material. 
        if(usedItem.category == itemCategory.Recipe)
        {
            //If it is a recipe, check if the furnace is active. If active, do nothing. If inactive, set it to active and obtain the next item needed. 
            if(isActive == false)
            {
                isActive = true;
                Debug.Log("The furnace is now activated.");

                return true; //return a value to let the ToolAction script knows that the check has pass through. 
            }
            else 
            {
                Debug.Log("The furnace is already trying to work on something else.");
                return false; 
            }
        }
        else if(usedItem.category == itemCategory.Material)
        {
            //Check if the item matches the nextItem, if yes, update the next item. 
            if(usedItem == nextIngredient.item)
            {
                //Debug.Log("ingredient is accepted. Please throw in the next ingredient");

                RemoveIngredientPrefab();
                //ingredientIndex += 1; //Set the index to the next one so that the next item prefab will be the next ingredient. 
                processingTimer = 0; //Set the timer to 0 so it can start to Tick(). 
                //Start the furnace light color change. 
                furnaceLightChange = StartCoroutine(ChangeLightColor());
                coroutineActive = true;
                return true; //let the ToolAction script knows that the check has pass through. 
                
            }
            else 
            {
                Debug.Log("it is an incorrect ingredient. Please throw in the correct ingredient.");
                return false; 
            }
        }
        else 
        {
            Debug.Log("The furnace does not take this item. Please leave");
            return false; 
        }
    }

    //This function is called by the ThrowRecipe.. tool action. 
    public void ImportIngredient(Recipe_Ingredient ingredient)
    {
        ingredients.Add(ingredient);
    }*/

/*    //This function sets the next ingredient to the nextIngredient slot. 
    public void SetNextItem()
    {
        nextIngredient = ingredients[ingredientIndex];
        ShowIngredient();
        if(coroutineActive == true)
        {
            StopCoroutine(furnaceLightChange);
            coroutineActive = false;
        }

        if(ingredientIndex < ingredients.Count)
        {
            ingredientIndex += 1;
        }
        else 
        {
            Debug.Log("All ingredients are loaded");
        }

        processingTimer = -1; //Set the timer to -1 so that the Tick() will not start just yet because the player need to throw in the ingredient. 
    }*/

/*    //This function shows the sprite of the ingredient to the prefab. 
    public void ShowIngredient()
    {
        //Show the first ingredient needed. 
        Vector3 iconPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        GameObject go = Instantiate(itemIconPrefab, iconPosition, Quaternion.identity);
        itemIconClone = go; 
        itemIconClone.GetComponent<SpriteRenderer>().sprite = ingredients[ingredientIndex].item.icon;
    }*/

/*    //This function removes the ingredient prefab. 
    public void RemoveIngredientPrefab()
    {
        Destroy(itemIconClone);
    }*/
    
    //Show the output icon.The player needs to collect the output before it gets overcooked. 
    public void ShowOutputIcon()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        GameObject go = Instantiate(itemIconPrefab, pos, Quaternion.identity);
        doneCraftingScript = go.GetComponent<DoneCraftingNoticeScript>();
        doneCraftingScript.ImportItem(outputItem);
    }

    //Show the prefab of the output. 
    public void DropOutput()
    {
        float spread = 2f;
        Vector3 position = transform.position;
        position.x += spread * UnityEngine.Random.value - spread / 2;
        position.y += spread * UnityEngine.Random.value - spread / 2;
        ItemSpawnManager.instance.SpawnItem(position, outputItem, 1);
        doneCraftingScript.DestroyIcon();//Remove the icon prefab.
        ResetTimers();
    }

    //Change the furnace light color.
    public IEnumerator ChangeLightColor()
    {
        Color newColor; 
        float red = 1;
        float green = 1;
        float blue = 1;
        float changedValue;
        bool forward = true; //If values changing from 1 to 0, it is a forward. Otherwise it is not. 

        while (true)
        {
            if(forward == true)
            {
                if(green >= 0)
                {
                    changedValue = 3 / 255f;
                }
                else //If the color changed to below 0, make forward false so it can go backward. 
                {
                    changedValue = -3 / 255f;
                    forward = false;
                }
            }
            else
            {
                if(green >=1)
                {
                    changedValue = 3 / 255f;
                    forward = true;
                }
                else
                {
                    changedValue = -3 / 255f;
                }

            }
            newColor = new Color(red, green, blue);
            furnaceLight.color = newColor;
            green = green - changedValue;
            blue = blue - changedValue;
            yield return new WaitForSeconds(0.05f);
        }
    }

    //The Tick function describes what the furnace does at each game second. 
    public void Tick()
    {
        //Run the timer if timer value is not -1 (when the next item is shown and waiting to be thrown in.
        if (processingTimer >= 0 && processingTimer < processingTime)
        {
            //Debug.Log("tick");
            processingTimer += 1;
        }
        //When timer reaches the process time determined by the ingredient class, stop the timer and proceed to the next ingredient on the list. 
        else if (processingTimer >= processingTime)
        {
            Debug.Log("processing finished.");
            if(isStillBaking == false) //If the output is just finished and the icon has not been instantiate..
            {
                //Show the output item icon.
                ShowOutputIcon();
                //Set isStillBakign to true so next time it will go to the else path. 
                isStillBaking = true;
                //Increase the baskingTimer count. 
                bakingTimer += 1;
            }
            else //If isStillBaking is true, don't need to instantiate the icon. Determine whether the baking timeris greater then baking time.
            {
                if(bakingTimer < bakingTime)
                {
                    //Increase the baskingTimer count. 
                    bakingTimer += 1;
                }
                else
                {
                    //Change the item in the icon into waste. 
                    Debug.Log("the output is overcooked");
                    doneCraftingScript.DestroyIcon();
                    ResetTimers(); 
                }

            }


            
            //ShowOutput();

            //Stop the coroutine.
            if (coroutineActive == true)
            {
                StopCoroutine(furnaceLightChange);
                furnaceLight.color = new Color(1, 1, 1, 1);//Return color to yellow. 
                coroutineActive = false;
            }
/*            //Return processing timer back to -1 so it won't continue to spit output. 
            processingTimer = -1;*/
        }
        //Otherwise, timer value should be -1, which indicates that the ingredient is waiting to be put in. 
        else
        {
            return;
        }
    }

    //Called when the output(or waste) is taken away. 
    public void ResetTimers()
    {
        processingTimer = -1;
        StopCoroutine(furnaceLightChange);
        furnaceLight.color = new Color(1, 1, 1, 1);//Return color to yellow. 
        coroutineActive = false;
        isStillBaking = false;
        isActive = false;
        bakingTimer = 0;
        doneCraftingScript.DestroyIcon();//Remove the icon prefab.
    }

}
