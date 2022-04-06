using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompleteButton : MonoBehaviour
{
    [SerializeField] Quest quest;
    [SerializeField] Character character;
    [SerializeField] QuestBlockController questBlock; 

    void Start()
    {
        character = GameManager.instance.character; 
    }

    public void GetQuest(Quest importedQuest)
    {
        quest = importedQuest; //Make this button specific for this particular quest that is active in the player's list. 
    }

    public void GrantReward()
    {
        character.AddMoney(quest.goldReward); //Increase the gold of the player by the quest's goldReward variable. 
        character.activeQuests.Remove(quest); //We need to remove this. Otherwise the QuestBlock will keep
                                              //coming up as this quest is still in the list. 
        questBlock.Hide();
        Debug.Log("granted 1000 gold");
    }


}
