using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWalkSkillScript : SkillController
{
    [SerializeField] float speedIncreasePercentage; //The amount of speed increase in percentage. 
    [SerializeField] float durationTime;
    CharacterController2D charController; 
    private float endTime;
    private bool isRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameManager.instance.player;
        character = player.GetComponent<Character>();
        charController = player.GetComponent<CharacterController2D>();
        //Check whether the buff already exist in the bufflist. If yes, delete it and reset it. If no, simply create it.
    }



    public void IncreaseSpeed() //This is called by the first frame of idel animation. 
    {
        if(isRunning == true) { return;  }
        isRunning = true;
        float newSpeed = charController.GetSpeed();
        newSpeed = newSpeed * (1f + speedIncreasePercentage / 100f);
        charController.ChangeMovementSpeed(newSpeed);
        //Use a local counter to manage the disappearing of the animation.  
    }

    public void ResetSpeed()
    {
        charController.ResetMovementSpeed();
    }

}
