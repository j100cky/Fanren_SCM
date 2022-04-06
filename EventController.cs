using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour
{
    public static EventController instance;

    void Awake()
    {
        instance = this;
    }

    [Serializable]
    public class EventStatus
    {
        public int eventID;
        public bool toPlay;
    }

    [SerializeField] public List<EventStatus> eventList; 

    public bool GetEventStatus(int id)
    {
        for(int i = 0; i<eventList.Count;i++)
        {
            if(eventList[i].eventID == id)
            {
                return eventList[i].toPlay;
                break;
            }
            else
            {
                continue;
            }
        }
        return false;
    }

    public void SetEventStatus(int id,  bool b)
    {
        for (int i = 0; i < eventList.Count; i++)
        {
            if (eventList[i].eventID == id)
            {
                eventList[i].toPlay = b;
                break;
            }
            else
            {
                continue;
            }
        }
        return;

     }


}
