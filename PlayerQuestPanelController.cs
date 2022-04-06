using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestPanelController : MonoBehaviour
{
    [SerializeField] List<GameObject> questBlocks;
    [SerializeField] Character character;
    [SerializeField] GameObject questBlockPrefab;
    [SerializeField] Transform contentObject;


    void Update()
    {
        CreateQuestBlocks();
        UpdateQuestBlocks();
 /*       if(Input.GetKeyDown(KeyCode.T))
        {
            GameManager.instance.inventoryContainer.ReturnItemCount(Fruit);
        }*/
    }

    private void CreateQuestBlocks()
    {
        if(character.activeQuests.Count == 0) 
        { 
            return; 
        }
        if(questBlocks.Count == 0) //If the block count is not updated to the quest list, initialize the block count. 
        {
            for (int i = 0; i < character.activeQuests.Count; i++)
            {
                GameObject go = Instantiate(questBlockPrefab, contentObject); //Instantiate the questBlock in the Content gameObject Transform.
                questBlocks.Add(go); //Add this questBlockPrefab's questBlockController to the list.
            }
        }
        else //When block count is not 0, there are three cases: 1) same quest counts, 2) more blocks than quest counts,
             //3) less blocks than quest counts.
        {
            if(questBlocks.Count == character.activeQuests.Count) //When their list counts are the same, we want it to update
                                                                  //information so the orders are correct. 
            {
                UpdateQuestBlocks();
            }

            else if(questBlocks.Count < character.activeQuests.Count) //When we need more quest blocks...
            {
                for(int i = questBlocks.Count; i< character.activeQuests.Count; i++) //Add the number of blocks until it matches the quest list count. 
                {
                    GameObject go = Instantiate(questBlockPrefab, contentObject); //Instantiate the questBlock in the Content gameObject Transform.
                    questBlocks.Add(go); //Add this questBlockPrefab's questBlockController to the list.
                }
            }
            else //When we have more quest blocks then the quest list, we need to clear every quest blocks, refresh the list,
                 //and rebuild the blocks according to the list.
            {
                questBlocks = new List<GameObject>(); //We will refresh the list and update every quest. 
                foreach(Transform questBlockPrefab in contentObject) //Destroy all children objects (the quest blocks) in the content object. 
                {
                    GameObject.Destroy(questBlockPrefab.gameObject);
                }
                for (int i = 0; i < character.activeQuests.Count; i++) //Then, rebuild the list and quest blocks
                {
                    GameObject go = Instantiate(questBlockPrefab, contentObject); //Instantiate the questBlock in the Content gameObject Transform.
                    questBlocks.Add(go); //Add this questBlockPrefab's questBlockController to the list.
                }
            }

        }

/*        for (int i = 0; i < character.activeQuests.Count; i++)
        {
                GameObject go = Instantiate(questBlockPrefab, contentObject); //Instantiate the questBlock in the Content gameObject Transform.
                questBlocks.Add(go); //Add this questBlockPrefab's questBlockController to the list.
        }*/
    }


    private void UpdateQuestBlocks()
    {
        for(int i = 0; i < character.activeQuests.Count; i++)
        {
            Quest quest = character.activeQuests[i]; //Reference to the ith quest in the Character's list. 
            QuestBlockController questBlockController = questBlocks[i].GetComponent<QuestBlockController>(); //Reference to the ith Quest Block.
            questBlocks[i].SetActive(true);
            if(quest != null)
            {
                questBlockController.questTitle.text = quest.questName;
                questBlockController.questDescription.text = quest.description;
                questBlockController.questNPC.text = quest.questNPC;
                questBlockController.questProgress.text = quest.questGoal.currentAmount.ToString() + "/" + quest.questGoal.targetAmount.ToString();
                questBlockController.questReward.text = quest.goldReward.ToString() + "gold";
                //questBlockController.questCompleteButton.GetComponent<QuestCompleteButton>().GetQuest(quest); //Transfer the quest information
                                                                                                              //to the QuestCompleteButton script
                                                                                                              //so that the button prefab will
                                                                                                              //be attached to this particular 
                                                                                                              //quest block(?)
                PlayerInventorySearch(quest);
            }

            
/*            if (quest.questGoal.IsQuestComplete()) 
            { 
                questBlockController.questCompleteButton.SetActive(true); //Showing the "quest complete" button when the quest is complted. 
            }*/
        }
    }

    private void PlayerInventorySearch(Quest quest) //This function searches the QuestGoal targetItem in the player's inventory.
    {
        int itemCount = 0; 
        ItemSlot itemSlot = GameManager.instance.inventoryContainer.slots.Find(x => x.item == quest.questGoal.targetItem);
        if(itemSlot == null)
        {
            itemCount = 0;
        }
        else
        {
            itemCount = itemSlot.count;
        }

        //int itemCount = GameManager.instance.inventoryContainer.ReturnItemCount(quest.questGoal.targetName);
        quest.questGoal.UpdateTargetItemNumber(itemCount);
    }

}
