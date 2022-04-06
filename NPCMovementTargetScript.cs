using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementTargetScript : MonoBehaviour
{
    [SerializeField] DayTimeController clock; //Access to the clock. 
    [SerializeField] NPCMovementController npcMovementController; //Access to the MoveTo() and other functions that controls the NPC.
    public List<NPCSchedule> npcSchedule; //Read a list of NPC schedules that provide information on when and where it needs to move. 
    //[SerializeField] bool isRepeating; 
    bool isActing = false;
    public bool isActionFinished = false; //This is read by the sceneNPCController so that it knows when to set inactive. 


    void Start()
    {
        npcMovementController = GetComponent<NPCMovementController>();
        clock = GameManager.instance.timeController;
    }

    public void BeginSchedule(NPCSchedule npcSchedule)
    {
        if(isActing == true) { return; }
        StartCoroutine(Move(npcSchedule));
        //This is used to tell the Update() not to keep starting the coroutine. 
        isActing = true;
    }

    private IEnumerator Move(NPCSchedule npcSchedule)
    {
        //Copy the target positions from the npc schedule. 
        List<Vector3> targetPositions = npcSchedule.targetPositions;
        
        //For each position, call the MoveTo() function in the NPCMovementController. 
        for (int i = 0; i < targetPositions.Count; i++)
        {
            StartCoroutine(npcMovementController.MoveTo(targetPositions[i])); 
            //Prevent the function to be called repetitively during Moving.
            while(npcMovementController.isMoving == true)
            {
                yield return null;
            }
        }
        //Set isActing to false when the act is done so that the BeginSchedule() can resume. 
        isActing = false;
        //Set isActionFinished to true so that the SceneNPCController can use this to determine whether to SetActive(false). 
        isActionFinished = true;
    }
}
