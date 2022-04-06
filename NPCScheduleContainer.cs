using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/NPC Schedule Container")]
public class NPCScheduleContainer : ScriptableObject
{
    public List<NPCSchedule> scheduleList;
    public bool isShownInScene;
    public GameObject npcPrefab;
    public GameObject npcInScene;
    public bool allScheduleFinished;
}
