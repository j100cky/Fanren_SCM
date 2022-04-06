using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_ObjectWithListOfQuests : Interactable
{
    [SerializeField] Actor actor;
    DialogContainer dialog;

    public override void Interact(Character character)
    {
        if(character.role == CharacterRole.Baker)
        {
            GamePauseController.instance.SetPause(true);
            GameManager.instance.npcMultipleQuestPanel.SetActive(true);
        }
        else
        {
            List<string> temp = new List<string>();
            temp.Add("桌子上写满了门派里派发下来的任务。看来掌管炼制的弟子要努力工作了。。。");
            dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
            dialog.MakeDialogContainer(temp, actor);
            GameManager.instance.dialogSystem.Initialize(dialog);
        }
    }

    public override void StopInteract(Character character)
    {
        GameManager.instance.npcMultipleQuestPanel.SetActive(false);
    }
}
