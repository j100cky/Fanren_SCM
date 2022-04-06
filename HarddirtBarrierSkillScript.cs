using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarddirtBarrierSkillScript : SkillController
{
    //The script already contains functions such as FinishAnimation().
    /*    Animator anim; */

    
    [SerializeField] float maxBarrierHealth = 100f;
    float currentBarrierHealth = 100f;
    float stage;
    string buffName;
    public void SetBuffName(string name)
    {
        buffName = name;
    }

    //This method is called by the TakeDamage() function in the Character script. The barrier will block damage from enemies
    //so that the player will not be harmed. 
    public override void BarrierTakeDamage(float damage)
    {
        currentBarrierHealth -= damage;
        PlayHitAnimation();
    }

    public override void PlayHitAnimation()
    {
        stage = currentBarrierHealth / maxBarrierHealth; 
        if(stage>0.75f)
        {
            anim.SetTrigger("isHit1"); //Play the first get hit animation with only a little crack.
        }
        else if(stage>0.5f && stage <0.75f)
        {
            anim.SetTrigger("isHit2");
        }
        else if(stage < 0.5f && stage > 0.25f)
        {
            anim.SetTrigger("isHit3");
        }
        else if(stage > 0 && stage < 0.5f)
        {
            anim.SetTrigger("isHit4");
        }
        else
        {
            //Destroy the skill prefab and remove this buff from the character's buff dictionary. 
            anim.SetTrigger("destroy");
            character.RemoveBuffPairs(buffName);  
        }
    }
}
