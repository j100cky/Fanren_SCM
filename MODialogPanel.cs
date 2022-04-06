using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MODialogPanel : MonoBehaviour
{
    [SerializeField] Text askingPrompt;
/*    string firstPart = "要将 ";
    string itemName;
    string secondPart = " 装备为贴身法宝吗?";
*/
    Item selectedMO;
    ItemContainer inventory;

    public int actionID; //The variable actionID determines what action will clicking the Yes button does.
                     //1 = equip the item.
                     //2 = take off the item. 


/*    public void SetText(string text)
    {
        askingPrompt.text = text;
    }*/

    public void SetItemText(string inputText, Item item, ItemContainer inventoryworldPoint)
    {
        inventory = inventoryworldPoint; //Knowing the inventory is important to Remove the item from the Inventory. 
        selectedMO = item;
        askingPrompt.text = inputText;
    }

    public void OnYesButtonClicked() //When the YesButon is clicked, there will be different functions played according to different ID. 
    {
        if(actionID == 1)
        {
           
            //Transfer this item into the itemslot in the status panel's MO slot. 
            GameManager.instance.statusPanel.GetComponent<StatusPanelController>().Equip(selectedMO);
             //Call the OnEquiped() function of the item's CarryingEffects.
            selectedMO.carryingEffects.OnEquiped(GameManager.instance.player.transform); 
            //Delete this item from the inventory...
            inventory.Remove(selectedMO);
            gameObject.SetActive(false); //Close the dialog panel.
            
        }
        else if (actionID == 2)
        {
            //Take off the item and copy the item to the inventory.
            GameManager.instance.statusPanel.GetComponent<StatusPanelController>().UnEquip();
            GameManager.instance.inventoryContainer.Add(selectedMO, 1); //Return the item to the inventory. 
            gameObject.SetActive(false); //Close the dialog panel.
            //selectedMO.carryingEffects.OnUnEquipped();

        }

    }

    public void OnNoButtonClicked() //Simply do nothing. 
    {
        gameObject.SetActive(false);
    }
}
