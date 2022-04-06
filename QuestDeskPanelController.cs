using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDeskPanelController : MonoBehaviour
{
    public List<Quest> questList; //Contain the list of quest that the desk provides. 
    public List<GameObject> questBlocks; //Contains information about the shown quest blocks. 
    [SerializeField] GameObject questBlockPrefab; //The prefab of one quest block. 
    [SerializeField] Transform contentObject; //Used to set the parent of the instantiated questblocks. 
    private int currentQuestCount; 

    private void Start()
    {
        CreateQuestBlocks();
    }


    private void CreateQuestBlocks()
    {
        //If no quest, let the player know and do not update. 
        if (questList.Count == 0)
        {
            Debug.Log("there is no request.");
            return;
        }
        //If the block count is 0 and the quest list is not, create the blocks.
        if (questBlocks.Count == 0)
        {
            for (int i = 0; i < questList.Count; i++)
            {
                GameObject go = Instantiate(questBlockPrefab, contentObject);
                go.GetComponent<QuestBlockController>().SetAsNPCBlock(questList[i], i);
                questBlocks.Add(go);
            }
        }
        else
        {
            UpdateQuestBlocks();
        }
    }

    private void UpdateQuestBlocks()
    {
        int blockNum = questBlocks.Count;
        //Remove all blocks and re-instantiate all blocks based on the new quest list. 
        for (int i = 0; i < blockNum; i++)
        {
            questBlocks[i].GetComponent<QuestBlockController>().Destroy();
        }
        questBlocks = new List<GameObject>();
        for (int j = 0; j < questList.Count; j++)
        {
            GameObject go = Instantiate(questBlockPrefab, contentObject);
            go.GetComponent<QuestBlockController>().SetAsNPCBlock(questList[j], j);
            questBlocks.Add(go);
        }
    }

    //Called by events to add quests into the desk's quest list. 
    public void AddQuest(Quest addedQuest)
    {
        questList.Add(addedQuest);
        UpdateQuestBlocks();
    }

    //Called by player accepting to remove a quest from the quest list. 
    public void RemoveQuest(int index)
    {
        questList.Remove(questList[index]);
        UpdateQuestBlocks();
    }
}
