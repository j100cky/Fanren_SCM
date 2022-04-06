using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))] //This makes sure that when this 
//script is added only to other objects that have Rigidbody2D. 


public class CharacterController2D : MonoBehaviour
{

	Rigidbody2D rigidbody2d; 
	[SerializeField] public float speed;
    [SerializeField] public float originalSpeed = 2f;  
	public Vector2 motionVector; //Create a vector variable to contain the information
	//of the char's x and y axis. 
    public Vector2 lastMotionVector; 
    public bool moving; //to test if the char is in motion or not. If it is not, idle. This is used for the condition for the transition in the animator. 
    Animator animator;
    bool isPracticing; //Used to determine whether the player is practicing skillbooks. 
    [SerializeField] Actor actor; //Used for making the dialog.


    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); //get the information
        //of the character's rigidbody2D components. 
        animator = GetComponent<Animator>(); //get the information of the character's animator components. 
        speed = originalSpeed;
        animator.SetFloat("lastVertical", -1f);
    }

    private void Update()
    {
        //Not implementing moving when isPaused is true.//
        if (GamePauseController.instance.GetPause() == true) 
        {
/*            animator.SetFloat("horizontal", 0);
            animator.SetFloat("vertical", 0);*/
            return;
        } 

    	float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        motionVector = new Vector2(horizontal, 
    		vertical);

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        //==========for idle facing directions
        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);
        if(horizontal != 0 || vertical != 0 || Input.GetKeyDown(KeyCode.Escape))
        {
            //Ask if the player wants to exit practice mode as soon as a direction key is pressed. 
            if (isPracticing == true)
            {
                //Create the dialog.
                List<string> _dialog = new List<string>();
                _dialog.Add("停止打坐修炼吗？");
                DialogContainer dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
                dialog.MakeDialogContainer(_dialog, actor);
                //Show the dialog in dialog panel.
                GameManager.instance.dialogSystem.Initialize(dialog, true);
                //Add the OnYesClicked() function 
                GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.AddListener(OnYesClicked);
                Debug.Log("OnYesButton set");
                //Add the OnNoClicked() function to the no button.
                GameManager.instance.dialogSystem.noButton.GetComponent<Button>().onClick.AddListener(OnNoClicked);
            }
            else
            {
                lastMotionVector = new Vector2(
                    horizontal,
                    vertical).normalized;

                animator.SetFloat("lastHorizontal", horizontal);
                animator.SetFloat("lastVertical", vertical);
            }

        }
    }

    void FixedUpdate()
    {
        if (GamePauseController.instance.GetPause() == true)
        {
            rigidbody2d.velocity = new Vector2(0,0);
            return;
        }
        Move(); //use the function Move().
    }

    private void Move()
    {
    	rigidbody2d.velocity = motionVector * speed; //the "velocity" component of RigidBody2D
    	//is fill in with the data in the motionVector variable. 
    }

    public float GetSpeed()
    {
        return speed;
    }
    public void ChangeMovementSpeed(float value)
    {
        speed = value;
    }
    public void ResetMovementSpeed()
    {
        speed = originalSpeed;
    }

    //Called by the skillbook practice button. 
    public void PracticeSkillbook()
    {
        animator.SetTrigger("practiceSkillbook");
        animator.SetBool("isPracticing", true);
        isPracticing = true;
    }

    //Called when level up instead of actively stopping skills. 
    public void StopPracticingSkills()
    {
        //Set isPractice = false so that player can go back to move. 
        isPracticing = false;
        //Stop the practicing coroutine in the SkillbookPracticeManager script. 
        GameManager.instance.player.GetComponent<SkillbookPracticeManager>().StopPracticing();
    }

    //Related to skillbook practicing. 
    private void OnYesClicked()
    {
        //Set isPractice = false so that player can go back to move. 
        isPracticing = false;
        //Trigger the animator to play the going back animation.
        animator.SetBool("isPracticing", false);
        //Stop the practicing coroutine in the SkillbookPracticeManager script. 
        GameManager.instance.player.GetComponent<SkillbookPracticeManager>().StopPracticing();
        //Hide the panels and cancel listeners. 
        GameManager.instance.dialogSystem.Show(false);
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnYesClicked);
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnNoClicked);
    }
    //Related to skillbook practicing. 
    private void OnNoClicked()
    {
        //If No is clicked, hide the dialog panel and do nothing.
        GameManager.instance.dialogSystem.Show(false);
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnYesClicked);
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnNoClicked);
        return;
    }

    //Called by the last frame of the finish practicing animation. 
    public void ResetParameters()
    {
        GamePauseController.instance.SetPause(false);
        animator.SetBool("moving", false);
        animator.SetFloat("lastHorizontal", 0);
        animator.SetFloat("lastVertical", -1);
        animator.SetFloat("horizontal", 0);
        animator.SetFloat("vertical", 0);
    }

    //Used by events.
    public void ResetParametersAllZero()
    {
        animator.SetBool("moving", false);
        animator.SetFloat("lastHorizontal", 0);
        animator.SetFloat("lastVertical", 0);
        animator.SetFloat("horizontal", 0);
        animator.SetFloat("vertical", 0);
    }

    public void WalkRight()
    {
        animator.SetBool("moving", true);
        animator.SetFloat("lastHorizontal", 1);
        animator.SetFloat("lastVertical", 0);
        animator.SetFloat("horizontal", 1);
        animator.SetFloat("vertical", 0);
    }




}
