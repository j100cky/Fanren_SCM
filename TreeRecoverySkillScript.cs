using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRecoverySkillScript : SkillController
{
    [SerializeField] float recoveryValue; //The amount of HP that is recovered each second. 
    //[SerializeField] float durationTime;
    //private float endTime; 
    private bool isActive;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameManager.instance.player;
        character = player.GetComponent<Character>();
        //endTime = Time.time + durationTime;  //Set the value of endTime. When endTime is met, the gameObject will disappear. 
        
    }


    public IEnumerator RecoverEverySecond() //This should be called by the animator. After the summoned animation is finished playing. 
    {
        //For the animation loop, this function should only be called once. 
        if(isActive == true)
        {
            yield return null;
        }
        isActive = true; 
        while(isActive == true)
        {
            character.RestoreHealth(recoveryValue); 
            yield return new WaitForSeconds(1f);
        }
    }

    public void RemoveRecovery()
    {
        isActive = false;
    }




}
