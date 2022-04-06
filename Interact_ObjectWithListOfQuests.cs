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
            temp.Add("������д�����������ɷ����������񡣿����ƹ����Ƶĵ���ҪŬ�������ˡ�����");
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
