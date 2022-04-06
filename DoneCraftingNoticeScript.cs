using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneCraftingNoticeScript : MonoBehaviour
{
    Item item;
    [SerializeField] SpriteRenderer icon; 

    private void Start()
    {
        icon = GetComponent<SpriteRenderer>();
    }
    
    //Called by functions that tell the notice what Item is carried. 
    public void ImportItem(Item importedItem)
    {
        item = importedItem;
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if(item != null)
        {
            icon.sprite = item.icon;
        }
    }

    //Called when the icon needs to be destroyed (e.g. when an ingredient is thrown into the morta). 
    public bool CheckAndDestroy(Item checkedItem)
    {
        if(item != null)
        {
            if(item == checkedItem)
            {
                //Return a value to let the motar know so it can break the for loop after one success.
                return true;
                Destroy(gameObject);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //Called by other script if it simply need to destroy the icon.
    public void DestroyIcon()
    {
        Destroy(gameObject);
    }

}
