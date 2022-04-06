using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to NPCs.
public class TalkInteract : Interactable
{

	[SerializeField] DialogContainer dialog;
    NPCQuestController npcQuestController;

    void Start()
    {
        npcQuestController = GetComponent<NPCQuestController>(); //Read the QuestController of the NPC.
    }

  	public override void Interact(Character character)
  	{
  		//GameManager.instance.dialogSystem.Initialize(dialog); //Start the dialog.
        if(npcQuestController != null) //If the NPC contains a quest, transfer this NPCQuestController
                                       //to the DialogSystem so it can show the NPCQuestPanel for the player to accept. 
        {
            //Debug.Log("interacted");
            GamePauseController.instance.SetPause(true);
            npcQuestController.Initialize(dialog); //Provide the actor information to the npcQuestController so it can get the portrait. 
            //GameManager.instance.dialogSystem.GetNPCQuestController(npcQuestController);
        }
        else
        {
            GameManager.instance.dialogSystem.Initialize(dialog); //Start the dialog
        }
  	}

}
