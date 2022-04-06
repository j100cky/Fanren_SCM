using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_BlackSmith1 : Interactable
{
    [SerializeField] Actor actor; //The NPC's profile. 
    [SerializeField] ItemContainer tradeItemPanel; //The item container that contains the items being sold. 
    DialogContainer dialog;

    public override void Interact(Character character)
    {
        //Pause game.
        GamePauseController.instance.SetPause(true);
        //Create a dialog and initialize the dialog. 
        List<string> temp = new List<string>();
        temp.Add("你好，我是铁匠的学徒铁头。");
        temp.Add("我可以帮你打造一些武器和工具。");
        dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(temp, actor, tradeItemPanel);
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
