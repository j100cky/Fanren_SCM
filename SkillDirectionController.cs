using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDirectionController : MonoBehaviour
{
	Animator animator;
	Vector2 motionVector;
	bool isMoving;
	Vector2 lastMotionVector;
	GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
    	motionVector = player.GetComponent<CharacterController2D>().motionVector;  //Connect the motionVector here to that in the character controller script so that they will sync.
        lastMotionVector = player.GetComponent<CharacterController2D>().lastMotionVector;   //Do the same for the lastMotionVector as well.
        SetParameters();
    }

    private void SetParameters()
    {
    	animator.SetFloat("horizontal", motionVector.x);
        animator.SetFloat("vertical", motionVector.y);
        isMoving = motionVector.x != 0 || motionVector.y != 0;
        animator.SetBool("isMoving", isMoving);

        if(motionVector.x != 0 || motionVector.y != 0)
        {
            animator.SetFloat("lastHorizontal", motionVector.x);
            animator.SetFloat("lastVertical", motionVector.y);
        }
    }
}
