using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanelAcceptButton : MonoBehaviour
{
    [SerializeField] GameObject questPanel;
    [SerializeField] Character character;

    public void AcceptQuest()
    {
        Quest quest = questPanel.GetComponent<NPCQuestPanel>().quest;
        quest.isQuestActive = true; //We want to set the quest as active. 
        character.activeQuests.Add(quest); //We want to add this quest to the character's active quest list
                                           //for the quest panel's reference. 
        questPanel.SetActive(false); //We want to hide the NPC quest panel to indicate an end of conversation. 
        GamePauseController.instance.isPaused = false; //Un-pause the game.
        Debug.Log("quest accepted"); 
    }
}
