using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public float speed;
    public bool isMoving;
    Vector2 lastDirection; 

    private void Start()
    {
        //Get the animator component from this NPC's prefab.
        anim = GetComponent<Animator>();
        //Set the default facing to facing towards the player. 
        anim.SetFloat("vertical", -1f);
    }

    //Call this function to stop walking and play the Idle animations.  
    public void Idle(float horizontal, float vertical)
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
        anim.SetBool("moving", false);
    }

    //Call this function to start playing the walking animation. 
    public void Walk(float horizontal, float vertical)
    {
        anim.SetFloat("horizontal", 0f);
        anim.SetFloat("vertical", 0f);
        anim.SetFloat("horizontal", horizontal);
        anim.SetFloat("vertical", vertical);
        anim.SetBool("moving", true);
    }

    //Called by outside functions to set animation.
    public void WalkToTarget(Vector3 position)
    {
        float h = DetermineDirection(position).x;
        float v = DetermineDirection(position).y;
        DetermineLastDirection(position);
        Walk(h, v);
    }

    private void DetermineLastDirection(Vector3 position)
    {
        Vector2 lastVector = DetermineDirection(position);
        if (lastVector.x == 0 && lastVector.y == 0)
        {
            return;
        }
        else 
        {
            lastDirection = lastVector;
        }
    }

    //Called by outside functions.
    public void IdleAtLastDirection()
    {
        Idle(lastDirection.x, lastDirection.y);
    }

    //Call this function to move the NPC from one spot to another. Interacting with the NPC will pause the movement. 
    public IEnumerator MoveTo(Vector3 position)
    {
        float distance = Vector3.Distance(position, transform.position);
        Vector2 lastMotionVector = new Vector2();
        while(distance > 0.01f) //The distance is set to 0.01 so that the facing for the Idle() will be correct for the lastMotion vector.
        {

            isMoving = true;
            //When the player interacts with the NPC, game will pause, and the NPC's movement will pause, too.
            if (GamePauseController.instance.GetPause() == true)
            {
                Idle(0f, -1f); //Stop the NPC moving when it is interacted by the player. 
                yield return null;
            }
            //Resume from the pause when the interaction is over. 
            else
            {
                Walk(DetermineDirection(position).x, DetermineDirection(position).y);
                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                distance = Vector3.Distance(position, transform.position);
                lastMotionVector = DetermineDirection(position);
            }

            yield return null;
        }
        //When the NPC reaches the target position, isMoving will turn false so that the NPC will take on the next action.
        isMoving = false;
        //Change to Idle. 
        Idle(DetermineDirection(position).x, DetermineDirection(position).y);
    }

    //This function is used to determine the facing of the NPC.The return values can be used with Walk() and Idle() functions. 
    public Vector2 DetermineDirection(Vector3 position)
    {
        Vector3 difference = position - transform.position;
        float hDiff = difference.x;
        float vDiff = difference.y;
        return new Vector2(hDiff, vDiff);
    }


}
