using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is a place holder for the children components of the NPCQuestPanel panel. This is for ease of access.
public class NPCQuestPanel : MonoBehaviour
{
    [SerializeField] public Image portrait;
    [SerializeField] public Text questTitle;
    [SerializeField] public Text questDescription;
    [SerializeField] public Text rewardAmount;
    [SerializeField] public Button accept;
    [SerializeField] public Button decline;

    [SerializeField] public Quest quest; 

    public void GetQuest(Quest importingQuest)
    {
        quest = importingQuest;
    }
}
