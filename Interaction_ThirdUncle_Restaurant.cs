using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_ThirdUncle_Restaurant : Interactable
{
    [SerializeField] Actor actor; //The NPC's profile. 
    [SerializeField] GameObject eventAfterInteraction;

    public override void Interact(Character character)
    {
        //Pause game.
        GamePauseController.instance.SetPause(true);
        //Create a dialog and initialize the dialog. 
        List<string> temp = new List<string>();
        temp.Add("噢！是小子啊。");
        temp.Add("怎么想起来叔这啦？今天的活干完了？ ");
        temp.Add("来来来，叔给你搞上两个硬菜。吃饱了人才能有力气！");
        DialogContainer dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(temp, actor, null, null, eventAfterInteraction);
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
