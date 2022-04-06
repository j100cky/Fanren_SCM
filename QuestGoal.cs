using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public int targetAmount; //The amount of enemy killed required or item collection required. 
    public int currentAmount; // The amount of requested thing that the player currently have. 
    public GoalType goalType;
    public Item targetItem;
    public string targetEnemy; //To determine the target item or enemy name needed to collect or kill so it is easier to measure. 

    public bool IsQuestComplete()
    {
        return (currentAmount >= targetAmount);
    }

    public void KillEnemy(string killedEnemy) //This will be called when the enemy is dead. 
    {
        if(goalType == GoalType.Hunt)
        {
            if(killedEnemy == targetEnemy)
            {
                //Debug.Log("killed enemy is:" + killedEnemy + "/" + "targetEnemy is:" + targetEnemy);
                currentAmount++;
                //Debug.Log("currentAmount is:"+currentAmount);

            }

        }
    }

    public void ItemCollected(Item pickedUpItem) //This will be called when the item is picked up.
    {
        if(goalType == GoalType.Collect)
        {
            if(pickedUpItem == targetItem)
            {
                currentAmount++;
            }
            
        }
    }

    public void UpdateTargetItemNumber(int number) //This will be called by ... to provide
                                                                    //the number of existing item that matches the target. 
    {
        if(goalType == GoalType.Collect) //Added this condition so that when enemies are killed, this function
                                         //will not be called to update the currentAmount.
        {
            currentAmount = number;
        }

    }

}

public enum GoalType //I will use this enum to determine functions which will used at differene scenarios.
{
    Hunt,
    Collect
}
