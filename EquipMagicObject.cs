using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Tool Action/Equip Magic Objects")]
public class EquipMagicObject : ToolAction
{
    [SerializeField] GameObject moDialogPanel;

    string firstPart = "要将 ";
    //string itemName;
    string secondPart = " 装备为贴身法宝吗?";

    public override void OnItemUsed(Item usedItem, ItemContainer inventoryworldPoint) //This overriding function will call out the message panel
                                                                                      //asking the player whether they want to equip the MO.
    {
        moDialogPanel = GameManager.instance.moDialogPanel;
        moDialogPanel.SetActive(true);
        moDialogPanel.GetComponent<MODialogPanel>().actionID = 1; //Determine that by clicking Yes, the item will be equipped. 
        string inputText = firstPart + usedItem.Name + secondPart; 
        moDialogPanel.GetComponent<MODialogPanel>().SetItemText(inputText, usedItem, inventoryworldPoint);
    }
}
