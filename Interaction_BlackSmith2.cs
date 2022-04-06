using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_BlackSmith2 : Interactable
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
        temp.Add("老夫鲁铁。");
        temp.Add("我这有上好的矿石。");
        temp.Add("偶尔还有几件我亲自炼制的武器。只不过他们都是抢手货，见不见得到就看你的机缘了。");
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
