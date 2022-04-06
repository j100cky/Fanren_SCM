using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    Animator anim;
    public Item outputItem;
    public int processingTime;
    [SerializeField] List<Recipe_Ingredient> ingredients;
    [SerializeField] GameObject itemIconPrefab;
    List<GameObject> instantiatedIcons; 
    List<Vector3> itemIconPositions;
    bool isActive; //Determines whether the mortar can accept recipe. 
    public bool isReady; //Determines whether the mortar is operatable. 
    [SerializeField] GameObject chymeContainer;
    public bool isContainerReady; //Determines whether the mortar can operate. 

    private void Start()
    {
        anim = GetComponent<Animator>();
        itemIconPositions = new List<Vector3>();
        instantiatedIcons = new List<GameObject>();
        SetIconPositions();
        isContainerReady = true;
    }

/*    private void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            SetIconPositions();
            ShowIngredientIcons();
        }

    }*/

    public void ResetIngredientList()
    {
        if(ingredients != null)
        {
            for(int i = 0; i < ingredients.Count; i++)
            {
                ingredients.Remove(ingredients[i]);
            }
        }
    }

    //Check the item that is being thrown in.
    public bool CheckItem(Item usedItem, ItemContainer inventory)
    {
        //Check whether the item is a recipe or a material. 
        if (usedItem.category == itemCategory.Recipe)
        {
            //If it is a recipe, check if the mortar is active. If active, do nothing. If inactive, set it to active and obtain the next item needed. 
            if (isActive == false)
            {
                isActive = true;
                //Set the output item
                //Clean the list of the previous ingredients. 
                ResetIngredientList();
                //ShowIngredientIcons();
                Debug.Log("The motar is now activated.");

                return true; //return a value to let the ToolAction script knows that the check has pass through. 
            }
            else
            {
                Debug.Log("The mortar is already trying to work on something else.");
                return false;
            }
        }

        //Else, check if the item is a material or medicine. 
        else if (usedItem.category == itemCategory.Material || usedItem.category == itemCategory.Medicine)
        {
            //Check if the ingredient matches any of the required ingredients. 
            for(int i = 0; i < ingredients.Count; i++)
            {
                if(ingredients[i].item == usedItem)
                {
                    //Check item's count in the inventory.
                    if(inventory.CheckCount(usedItem, ingredients[i].count) == false)
                    {
                        Debug.Log("this is the correct ingredient, but the quantity is not enough"); 
                    }
                    else
                    {
                        //Remove the item fro mteh Inventory
                        inventory.Remove(usedItem, ingredients[i].count);
                        //Remove the ingredient from the list
                        ingredients.Remove(ingredients[i]);
                        //Update teh icons
                        UpdateIcons();
                        //If the ingredients are all filled in, run the ready animation. 
                        if(ingredients.Count == 0)
                        {
                            isReady = true;
                            ReadyAnimation();
                        }
                        //let the ToolAction script knows that the check has pass through. 
                        return true;
                    }

                }
                else
                {
                    continue;
                }
            }
            //If none is matched, do nothing. 
            Debug.Log("it is an incorrect ingredient. Please throw in the correct ingredient.");
            return false;

        }
        //Else, if the item is not even a material, 
        else
        {
            Debug.Log("The motar does not take this item. Please leave");
            return false;
        }
    }

    //Set the output item and processingTime so that the chymecontainer can know this info. 
    public void SetOutputItem(Item output)
    {
        outputItem = output;
        int time = 0; 
        for(int i = 0; i < ingredients.Count; i++)
        {
            time += ingredients[i].processTime; 
        }
        processingTime = time;
    }

    //This function is called by the ThrowRecipe.. tool action. 
    public void ImportIngredient(Recipe_Ingredient ingredient)
    {
        ingredients.Add(ingredient);
    }

    //Show what ingredients are needed. 
    public void ShowIngredientIcons()
    {
        for(int i = 0; i< ingredients.Count; i++)
        {
            GameObject go = Instantiate(itemIconPrefab, itemIconPositions[i], Quaternion.identity);
            go.GetComponent<DoneCraftingNoticeScript>().ImportItem(ingredients[i].item);
            instantiatedIcons.Add(go);
        }
        /*Vector3 iconPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        GameObject go = Instantiate(itemIconPrefab, iconPosition, Quaternion.identity);
        itemIconClone = go;
        itemIconClone.GetComponent<SpriteRenderer>().sprite = ingredients[ingredientIndex].item.icon;*/
    }

    //This is called after each ingredient is put in. It update the icon position and numbers. 
    public void UpdateIcons()
    {
        ResetIngredientIcons();
        ShowIngredientIcons();
    }

    public void ResetIngredientIcons()
    {
        for(int i = 0; i < instantiatedIcons.Count; i++)
        {
            Destroy(instantiatedIcons[i]);
        }
    }

    //Show the container gameobject. This is called by the PlaceChymeContainer script. 
    public void SetContainerActive(bool b)
    {
        chymeContainer.SetActive(true);
        /*        if (b == false) //If the container is not filled
                {
                    chymeContainer.GetComponent<Animator>().SetTrigger("finished");
                }*/
        chymeContainer.GetComponent<Interact_ChymeContainer>().isFilled=b;
    }

/*    //Hide the required ingredient prefab. 
    public void RemoveIngredientPrefab(Item usedItem)
    {
        for (int i = 0; i < instantiatedIcons.Count; i++)
        {
            //Call the CheckAndDestroy() function in the DoneCr.. script so that the icon can destroy itself if items match. 
            bool result = instantiatedIcons[i].GetComponent<DoneCraftingNoticeScript>().CheckAndDestroy(usedItem);
            //If the result returned is true, stop running the function to avoid destroying additional icons. 
            if(result == true)
            {
                return;
            }
            else
            {
                continue;
            }
        }

    }*/
    
    //Set animator to ready
    public void ReadyAnimation()
    {
        anim.SetTrigger("ready");
    }

/*    //Start the animation. 
    public void StartAnimation()
    {
        anim.SetTrigger("start");
    }

    //End the animation. 
    public void EndAnimation()
    {
        anim.SetTrigger("finished");
    }*/

    //Set the list of icon positions. 
    private void SetIconPositions()
    {
        for(int i = 0; i <= ((int)(ingredients.Count-1)/2)+1; i++)
        {
            if(i == 0)
            {
                itemIconPositions.Add(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
            }
            else
            {
                itemIconPositions.Add(new Vector3(transform.position.x-(float)(i)*1.5f, transform.position.y + 1.5f, transform.position.z));
                itemIconPositions.Add(new Vector3(transform.position.x+(float)(i)*1.5f, transform.position.y + 1.5f, transform.position.z));
            }
        }
    }

    //This is called by the last frame of the finishing animation to reset isActive and isReady and also adjust the status of the container.  
    public void ResetMortar()
    {
        isActive = false;
        isReady = false;
        anim.SetBool("start", false);
        chymeContainer.GetComponent<Interact_ChymeContainer>().isFilled = true; 
        isContainerReady = false; //Since the container is filled, it is not ready. 
    }
}
