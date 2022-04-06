using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Tool Action/Studying Skillbook")]
public class SkillbookReading : ToolAction
{
    [SerializeField] DialogContainer dialog;
    [SerializeField] Actor actor;
    SkillbookContainer skillbookContainer; //The skillbookContainer of the player, will be getting from GameManager.
    Skillbooks usedSkillbook; //Used to hold the skillbook that matches the item.

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        //Check if the item has been learned. 
        //First find the Skillbooks that corresponding to this item. 
        for (int i = 0; i < GameManager.instance.skillbookLibrary.skillbooks.Count; i++)
        {
            if (usedItem.ID == GameManager.instance.skillbookLibrary.skillbooks[i].item.ID)
            {
                usedSkillbook = GameManager.instance.skillbookLibrary.skillbooks[i];
            }
            else
            {
                continue;
            }
        }
        //Then Check whether this skillbook has been learned. 
        skillbookContainer = GameManager.instance.skillbookContainer;
        //If the Skillbooks is already learned, don't add it to the container. The player can sell it or whatever. 
        if (skillbookContainer.CheckExisting(usedSkillbook) == true)
        {
            //Show the dialog to tell the player what happened. 
            List<string> dialog1 = new List<string>();
            dialog1.Add("你把神识探了进去。");
            dialog1.Add("原来这是一本名为" + usedItem.Name + "的修炼功法。");
            dialog1.Add("......");
            dialog1.Add("你已经学会了这本功法，可以直接打坐修炼了。没必要再学一遍。");
            dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
            dialog.MakeDialogContainer(dialog1, actor);
            GameManager.instance.dialogSystem.Initialize(dialog);
        }
        //If this is a new Skillbooks to the player, register it to the SkillbookContainer. 
        else
        {
            //Show the dialog.
            List<string> dialog1 = new List<string>();
            dialog1.Add("你把神识探了进去。");
            dialog1.Add("原来这是一本名为" + usedItem.Name + "的修炼功法。");
            dialog1.Add("......");
            dialog1.Add("你把内容默默记在脑海里。以后可以随时随地修炼这套功法了！");
            dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
            dialog.MakeDialogContainer(dialog1, actor);
            GameManager.instance.dialogSystem.Initialize(dialog);
            //Clear the item from the inventory and add it to the skill panel. 
            inventory.Remove(usedItem);
            //Add the skillbook into the skillbook container, which will be used to refresh the skillbook list. 
            skillbookContainer.Add(usedSkillbook);
        }
    }
}
