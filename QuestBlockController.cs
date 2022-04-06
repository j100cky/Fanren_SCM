using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestBlockController : MonoBehaviour, IPointerClickHandler
{
    public int questBlockIndex; 
    [SerializeField] public Text questTitle;
    [SerializeField] public Text questDescription;
    [SerializeField] public Text questNPC;
    [SerializeField] public Text questProgress;
    [SerializeField] public Text questReward;
    [SerializeField] public GameObject questCompleteButton;
    bool isNPCQuestBlock;
    Quest quest;
    [SerializeField] Image shinging;
    [SerializeField] QuestDeskPanelController questPanel; 

    //This is for setting the NPC's quest list e.g. the crafting quest desk. 
    public void SetAsNPCBlock(Quest quest, int index)
    {
        isNPCQuestBlock = true;
        questBlockIndex = index;
        this.quest = quest;
        questPanel = GetComponentInParent<QuestDeskPanelController>();
        if(questPanel == null)
        {
            Debug.Log("panel controller not found");
        }
        questTitle.text = quest.questName;
        questDescription.text = quest.description;
        questNPC.text = quest.questNPC;
        questReward.text = quest.goldReward.ToString() + "gold";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //If this is not a block on the desk's quest list, do nothing. 
        if(isNPCQuestBlock == false)
        {
            return;
        }
        else
        {
            //Transfer this quest to the player's quest list. 
            GameManager.instance.character.activeQuests.Add(quest);
            //Remove the block from the panel. 
            StartCoroutine(Response());
        }
    }

    //This function flashes the panel and destroy. This is a response to clicking. 
    public IEnumerator Response()
    {
        Color c = new Color(1f, 1f, 1f, 1f);
        shinging.enabled = true;
        yield return new WaitForSeconds(0.1f);
        shinging.enabled = false;
        yield return new WaitForSeconds(0.1f);
        questPanel.RemoveQuest(questBlockIndex);
        //Destroy();
    }

}
