using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanelDeclineButton : MonoBehaviour
{
    [SerializeField] GameObject questPanel; 

    public void DeclineQuest()
    {
        questPanel.SetActive(false);
        GamePauseController.instance.isPaused = false; //Resume the game.
        Debug.Log("quest declined. Nothing happened");
    }
}
