using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBoxPanel : ItemPanel
{
    public override void OnLeftClick(int id)
    {
        GameManager.instance.dragAndDropController.OnLeftClick(inventory.slots[id]);
        Show(); //This is essential for the button to update the icon. 
    }
}
