using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_QuestTest : Interactable
{
    [SerializeField] Actor actor; //The NPC's profile. 
    [SerializeField] ItemContainer tradeItemPanel; //The item container that contains the items being sold. 
    [SerializeField] NPCQuestController questController;
    DialogContainer dialog;

    public override void Interact(Character character)
    {
        //Pause game.
        GamePauseController.instance.SetPause(true);
        //Create a dialog and initialize the dialog. 
        List<string> temp = new List<string>();
        temp.Add("ÄãºÃ");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(temp, actor, tradeItemPanel, questController);
        GameManager.instance.dialogSystem.Initialize(dialog);
    }

    //When pressed Esc key, this will run. 
    public override void StopInteract(Character character)
    {
        //Hide the trade panel. 
        GameManager.instance.tradePanel.GetComponent<TradeSystem>().Show(false);
        //Resumes game.
        GamePauseController.instance.SetPause(false);
    }
}
