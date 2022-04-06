using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests")]
public class Quest : ScriptableObject
{
    public string questName;
    public int questID;
    public string description;
    public string questNPC;
    public int goldReward;
    public List<Item> itemRewards;
    public QuestGoal questGoal;

    public bool isQuestActive; //Used by other scripts to determine whether this quest is active for the player. 
}
