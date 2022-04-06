using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHitSkillScript : SkillController
{
    [SerializeField] List<ResourceNodeType> canHitNodesOfType;

    //This is called at the hitting frame of the animation. 
    public void HitRock()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, skillAttackBoxSize);
        foreach (Collider2D c in colliders)
        {
            //Debug.Log(c);
            ToolHit hit = c.GetComponent<ToolHit>(); //to detect the components in the script ToolHit. Store the info in the var "hit"
            if (hit != null)    //If the collider has the hit script attached...
            {
                Debug.Log("hit found");
                if (hit.CanBeHit(canHitNodesOfType) == true) //If the type of the object can be hit by the tool selected...
                {
                    Debug.Log("hit match");
                    hit.Hit(damage);
                }

            }
        }
    }

}
