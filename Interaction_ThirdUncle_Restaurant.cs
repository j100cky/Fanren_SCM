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
        temp.Add("�ޣ���С�Ӱ���");
        temp.Add("��ô������������������Ļ�����ˣ� ");
        temp.Add("��������������������Ӳ�ˡ��Ա����˲�����������");
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
