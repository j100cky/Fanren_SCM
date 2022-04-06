using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNPCManager3 : MonoBehaviour
{
    public static GameNPCManager3 instance;

    [SerializeField] List<NPCScheduleContainer> npcList;
    DayTimeController clock;
    string activeSceneName;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        clock = GetComponent<DayTimeController>();
        ResetStatus();
    }

    void Update()
    {
        if(GamePauseController.instance.GetPause() == true) { return; }
        activeSceneName = SceneManager.GetActiveScene().name;
        CheckSchedule();
    }

    //Determine whether to move the NPC's position or not. 
    public void CheckSchedule()
    {
        //Cycle through all NPC schedule containers
        for(int i = 0; i < npcList.Count; i++)
        {
            //For each npc schedule container, cycle through each schedule. 
            for (int j = 0; j < npcList[i].scheduleList.Count; j++)
            {
                //Pass the schedule to the variable n. 
                NPCSchedule n = npcList[i].scheduleList[j];

                //Check for in progress
                if(n.inProgress == true)
                {//If it is in progress, check whether the progress is done. 
                    if(n.isDirty == true)
                    {//If the progress is done, chece whether the schedule involves scene exit. 
                        if(n.isExitingScene == true)
                        {//If yes, destroy the npc object
                            Destroy(npcList[i].npcInScene);

                            //Set inProgress to false. 
                            n.inProgress = false;

                            //Set isShownInScene to false
                            npcList[i].isShownInScene = false;
                        }
                        else
                        {//If the schedule does not involve scene exit.
                         //Simply set inProgress to false. 
                            n.inProgress = false;

                            //Play the idle animation properly. 
                            npcList[i].npcInScene.GetComponent<NPCMovementController>().IdleAtLastDirection();
                        }
                    }
                    else
                    {//If the progress is not done, call Move() to change the NPC position. 
                        //And check whether the NPC object needs to be instantiated. 
                        Move(n); 
                        if(n.sceneName == activeSceneName)
                        {//If scene names are matched, check whether the object is already shown in scene. 
                            if(npcList[i].isShownInScene == true)
                            {//If the npc is already shown, update the npc's position. 
                                npcList[i].npcInScene.transform.position = n.currentPosition;


                                //Play the walking animations.
                                npcList[i].npcInScene.GetComponent<NPCMovementController>().WalkToTarget(n.nextPosition);

                                continue;
                            }
                            else
                            {//If the NPC is not shown, show the NPC
                                GameObject go = Instantiate(npcList[i].npcPrefab);
                                
                                //Set the npc to the currentPosition determined by the schedule. 
                                go.transform.position = n.currentPosition;

                                //give reference to the schedule container's npcInScene variable. 
                                npcList[i].npcInScene = go;

                                //Set isShownInScene to true so it won't instantiate again. 
                                npcList[i].isShownInScene = true;
                            }
                        }
                        else
                        {//If scene names don't match, don't do any thing and move to the next schedule. 
                            continue;
                        }
                    }
                }
                else
                {//if the schedule is not in progress, first check for isDirty. 
                    if(n.isDirty == true)
                    {//if the schedule is not in progress and is dirty, that means the schedule has been run. Skip this. 
                        continue;
                    }
                    else
                    {//If not dirty, check whether the time matches. 
                        if(n.hour == clock.GetHour())
                        {//If the time match, start the schedule. 
                            //Set in progress to true. 
                            n.inProgress = true;

                            //set the current position of the schedule. 
                            n.currentPosition = n.targetPositions[0];

                        }
                        else
                        {//if the time doesn't match, skip it. 
                            continue;
                        }
                    }
                }
            }
        }
    }

    public void Move(NPCSchedule n)
    {
        //Check whether currentPosition == nextPosition. 
        if(n.currentPosition == n.nextPosition)
        {//if yes, check whether the current position reaches the end. 
            int posNumber = 0; 
            
            //Find which targetposition it is currently at. 
            for(int i = 0; i < n.targetPositions.Count; i++)
            {
                if(n.nextPosition == n.targetPositions[i])
                {
                    posNumber = i;
                }
            }

            if(posNumber+1 == n.targetPositions.Count)
            {//If the currentPosition reaches the end, this schedule is finished, and set isDirty to true. 
                n.isDirty = true;
                return;
            }
            else
            {//If the currentPosition does not reach the end, make next position to the next in the targetPos list. 
                n.nextPosition = n.targetPositions[posNumber + 1];
            }
        }
        else
        {//If current position has not reach the next position, move towards it. 
            n.currentPosition = Vector3.MoveTowards(n.currentPosition, n.nextPosition, 2f * Time.deltaTime);
        }
    }

    private void ResetStatus()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].isShownInScene = false;
            npcList[i].npcInScene = null;
            npcList[i].allScheduleFinished = false;
            for (int j = 0; j < npcList[i].scheduleList.Count; j++)
            {
                npcList[i].scheduleList[j].inProgress = false;
                npcList[i].scheduleList[j].isDirty = false;
                npcList[i].scheduleList[j].isMovingDone = false;
                npcList[i].scheduleList[j].nextPosition = npcList[i].scheduleList[j].targetPositions[0];
            }
        }
    }

    //Used when scene is exited in the GameSceneManager. 
    public void ResetNPCIsShown()
    {
        for (int j = 0; j < npcList.Count; j++)
        {
            npcList[j].isShownInScene = false;
            //Destroy(npcList[j].npcInScene);
        }

    }
}
