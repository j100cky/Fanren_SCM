using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/NPCSchedule")]

public class NPCSchedule : ScriptableObject
{
    [TextAreaAttribute(5, 20)]
    public string briefDescription; //A brief description for the developer to note what is the npc doing in this schedule. 

    public string sceneName; //The GameNPCManager will match this string with current active scene's name. 
    public int hour; //The time at which the event begin.
    public List<Vector3> targetPositions;
    public Vector3 currentPosition; //The GameNPCManager will use it to calculate each npc's position at any instance. 
    public Vector3 nextPosition; //The manager will use this to MoveTowards().
    //public GameObject npcPrefab; //The GameNPCManager will instantiate this prefab when the player is in the right scene. 
    //public GameObject npcInScene; //The manager wuill use this to control the position of the npc.
    public bool isEnteringScene; //If the NPC enters the scene (from inactive to active) set this to true. 
    public bool isExitingScene; //If the NPC needs to exit the scene after it reaches the destination, set this to true.
    //public bool isMoving; //The GameNPCManager will use this to determine whether to run the Move function or not. 
    public bool isMovingDone; //This is used to determine whether all the positions has been reached. 
    public bool isDirty = false; //When the schedule is played, mark this as true so it won't play repeatitively. 
    //public bool isNPCShown; //The manager will use this to determine whether to instantiate a prefab or not. 
    public bool inProgress; //The GameNPCManager will use this to determine whether to start an event or not. 
}
