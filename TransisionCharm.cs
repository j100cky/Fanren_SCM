using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransisionCharm : MonoBehaviour
{
    public string sceneNameToTransitTo; //This is usually the home of the player. 
    public string sceneNameToTransitBack; //This is the scene name of the current scene. 
    public Vector3 targetPosition; //This is the home position. 
    public Vector3 backPosition; //This is the position of the transition charm at current scene. 

    private void Start()
    {
        sceneNameToTransitTo = "MainScene";

        //Save the current scene name. 
        sceneNameToTransitBack = GameSceneManager.instance.currentScene; 
        //Save the position that the player will come back. 
        backPosition = transform.position;

        // Add the current location to the TransitionTargetManager's list.
        TransitionTargetManager.instance.AddTarget(sceneNameToTransitBack, backPosition);

        //Refresh the transition list. 
        TransitionTargetManager.instance.transitionMapController.panel.RefreshButtons();

    }

    //When player enters the transition area, they will be sent home or other designated scene. 
    public void IntiateTransition(Transform toTransition)
    {
        GamePauseController.instance.SetPause(true);

        GameSceneManager.instance.InitSwitchScene(sceneNameToTransitTo, targetPosition);
    }
}
