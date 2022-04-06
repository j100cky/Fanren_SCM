using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to control the setting active and setting inactive of the NPCs. 

public class SceneNPCController : MonoBehaviour
{
    [SerializeField] List<NPCMovementTargetScript> npcMovementScripts; //Use this to store and read all the NPCs in the scene. 
    DayTimeController clock; //Use this to compare time. 

    private void Start()
    {
        clock = GameManager.instance.timeController;
        
        //Reset all the schedule's isDirty variable to false.
        for(int i = 0; i < npcMovementScripts.Count; i++)
        {
            for (int j = 0; j < npcMovementScripts[i].npcSchedule.Count; j++)
            {
                npcMovementScripts[i].npcSchedule[j].isDirty = false;
            }
        }

    }

    /*private void Update()
    {
        //Circulate through all the NPCs in the scene.  
        for (int i = 0; i < npcMovementScripts.Count; i++)
        {
            //For each NPC, circulate through all its npcSchedules. 
            for (int j = 0; j < npcMovementScripts[i].npcSchedule.Count; j++)
            {
                //If the clock time matches the schedule, run the begin schedule code. 
                if(npcMovementScripts[i].npcSchedule[j].hour == clock.GetHour())
                {
                    //If one schedule is done playing, we don't want to play it again and again. 
                    if (npcMovementScripts[i].npcSchedule[j].isDirty == true) { return; }
                    //If the schedule involves a scene entering, set the NPC active. 
                    if(npcMovementScripts[i].npcSchedule[j].isEnteringScene == true)
                    {
                        npcMovementScripts[i].gameObject.SetActive(true);
                    }

                    //Call the BeginSchedule() so the NPC will start acting according to the schedule. 
                    npcMovementScripts[i].BeginSchedule(npcMovementScripts[i].npcSchedule[j]);

                    //Set the schedule to dirty after played. 
                    npcMovementScripts[i].npcSchedule[j].isDirty = true;
                }
                
                
*//*                //If the time matches and the npcSchedule marks the event to be entering scene, then set the NPC active. 
                if (npcMovementScripts[i].npcSchedule[j].hour == clock.GetHour() &&
                    npcMovementScripts[i].npcSchedule[j].isEnteringScene == true)
                {
                    npcMovementScripts[i].gameObject.SetActive(true);
                    npcMovementScripts[i].BeginSchedule(npcSchedule[j]);
                }*//*

                //If the action is finished (triggered by finishing the Move() function in the target script) and isExitingScene is marked, 
                //set the NPC inactive. 
                if (npcMovementScripts[i].isActionFinished == true &&
                    npcMovementScripts[i].npcSchedule[j].isExitingScene == true)
                {
                    npcMovementScripts[i].gameObject.SetActive(false);
                }
            }
        }
    }*/
}
