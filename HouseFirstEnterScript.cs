using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the HouseFirstEnter event, and controls the dialogs, time, and animations in this event. 
public class HouseFirstEnterScript : MonoBehaviour
{
    [SerializeField] int eventID = 2;
    [SerializeField] List<DialogContainer> dialogs;//The list of dialogs that will be shown in this event. The order matters. 
    [SerializeField] GameObject masterMo;
    [SerializeField] List<Vector3> targets;
    [SerializeField] Vector3 moEnterSpot;
    [SerializeField] Vector3 moApproachSpot;
    [SerializeField] GameObject player;
    [SerializeField] GameObject wok;
    [SerializeField] GameObject luggage; //containing the luggage that is being thrown out. 
    private int dialogIndex;
    private int targetIndex;
    Animator playerAnimator;
    NPCMovementController moController;
    [SerializeField] Animator luggageAnimator;

    void Start()
    {
        //If this event has been played, do not play again.
        if (EventController.instance.GetEventStatus(eventID) == false) 
        {
            gameObject.SetActive(false);
            return; 
        }  

        dialogIndex = 0;
        targetIndex = 0;
        GamePauseController.instance.SetEvent(true);
        StartCoroutine(Mo_Dialog_1());
        player = GameManager.instance.player;
        playerAnimator = GameManager.instance.player.GetComponent<Animator>();
        moController = masterMo.GetComponent<NPCMovementController>();
        luggage.SetActive(false);
    }

    //Mo's first dialog.
    private IEnumerator Mo_Dialog_1()
    {
        yield return new WaitForSeconds(1f);
        //Player moves away from the door.
        float distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        playerAnimator.SetBool("moving", true); //Set moving animation. 
        playerAnimator.SetFloat("horizontal", -1); //Player walks to the left. 
        while (distance > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;
        StopMovingAndResetFacing();
        playerAnimator.SetBool("moving", false); //Stops walking. 
        playerAnimator.SetFloat("lastHorizontal", 1); //Set facing to right. 
        yield return new WaitForSeconds(1f);
        //Mo enters the room.
        masterMo.GetComponent<SpriteRenderer>().enabled = true;
        moController.Idle(0f, 1f); //Set facing to Away.
        //Mo walks up a little and turns to the left. 
        yield return new WaitForSeconds(0.5f);
        moController.Walk(0f, 1f);
        Vector3 newPos = new Vector3(masterMo.transform.position.x, 
            masterMo.transform.position.y + 0.4f, masterMo.transform.position.z);
        float distance_1 = (masterMo.transform.position - newPos).sqrMagnitude;
        while(distance_1 > 0)
        {
            masterMo.transform.position = Vector3.MoveTowards(masterMo.transform.position, newPos, 2.5f * Time.deltaTime);
            distance_1 = (masterMo.transform.position - newPos).sqrMagnitude;
            yield return null;
        }
        moController.Idle(-1f, 0f);



        yield return new WaitForSeconds(1f);
        //Mo starts talking. 
        GameManager.instance.dialogSystem.Initialize(dialogs[dialogIndex]);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        dialogIndex++;
        //Mo exit the room. 
        Vector3 doorPortal = new Vector3(0f, -4.8f, 0);
        float distance_2 = (masterMo.transform.position - doorPortal).sqrMagnitude;
        moController.Walk(0f, -1f); //Walks Down. 
        while (distance_2 > 0)
        {
            masterMo.transform.position = Vector3.MoveTowards(masterMo.transform.position, doorPortal, 2.5f * Time.deltaTime);
            distance_2 = (masterMo.transform.position - doorPortal).sqrMagnitude;
            yield return null;
        }
        masterMo.GetComponent<SpriteRenderer>().enabled = false;//It is better to use this method to hide mo instead of set inactive. 
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PlayerMovements_1());
    }

    //The player moves to the bed side and drops off the luggage. 
    private IEnumerator PlayerMovements_1()
    {
        //Player moving up.
        float distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetBool("moving", true); //Player moving. 
        playerAnimator.SetFloat("vertical", 1); //Player facing up.
        while(distance > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;

        //Player moving to the right.
        float distance2 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetBool("moving", true); //Player moving
        playerAnimator.SetFloat("horizontal", 1); //Player moving right. 
        while(distance2 > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, 
                targets[targetIndex], 3f * Time.deltaTime);
            distance2 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetFloat("lastHorizontal", 1); //Set facing to right.
        yield return new WaitForSeconds(0.5f);
        luggageAnimator.SetTrigger("throw");//player throws boxes out. 
        yield return new WaitForSeconds(0.7f);
        luggage.SetActive(true);
        yield return new WaitForSeconds(1f);

        //Start the next event. 
        StartCoroutine(Player_Dialog_1());
    }

    //Player finds the books on the bookshelf interesting and starts reading them.
    private IEnumerator Player_Dialog_1()
    {
        //Player moves up to the bookshelf.
        float distance2 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing();
        playerAnimator.SetBool("moving", true); //Player moving. 
        playerAnimator.SetFloat("vertical", 1); //Player facing left. 
        while (distance2 > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance2 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;

        //Player moves left to the bookshelf.
        float distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing();
        playerAnimator.SetBool("moving", true); //Player moving. 
        playerAnimator.SetFloat("horizontal", -1); //Player facing left. 

        while (distance > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;



        StopMovingAndResetFacing(); //Reset facing.
        playerAnimator.SetFloat("lastVertical", 1); //Player facing up, the bookshelf. 
        yield return new WaitForSeconds(1f);

        //Plays the question mark speech bubble.
        MessageManager.instance.CallSpeechBubble(player.transform, 1);
        yield return new WaitForSeconds(1f);

        //Player talks about the book. 
        GameManager.instance.dialogSystem.Initialize(dialogs[dialogIndex]);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        dialogIndex++;

        //Player face left, sees the wok, and plays the ellipse speech bubble.
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetFloat("lastHorizontal", -1);
        yield return new WaitForSeconds(0.5f);
        MessageManager.instance.CallSpeechBubble(player.transform, 3);
        yield return new WaitForSeconds(1f);

        //Player walks towards the wok. 
        float distance3 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetBool("moving", true);
        playerAnimator.SetFloat("horizontal", -1);
        while (distance3 > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance3 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;

        //Player play the exclamation mark speech bubble.
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetFloat("lastHorizontal", -1);
        MessageManager.instance.CallSpeechBubble(player.transform, 0);
        yield return new WaitForSeconds(1f);

        //Player moves up to get closer to the wok. 
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetBool("moving", true);
        playerAnimator.SetFloat("vertical", 1); //Moving up.
        float distance4 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        while (distance4 > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance4 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;
        StopMovingAndResetFacing();
        playerAnimator.SetFloat("lastHorizontal", -1); //Player facing the wok(left).

        yield return new WaitForSeconds(1f);

        //Plays the ellipse speech bubble. 
        MessageManager.instance.CallSpeechBubble(player.transform, 3);
        yield return new WaitForSeconds(2f);

        //Play the wok animation. 
        wok.GetComponent<Animator>().SetTrigger("fly");
        yield return new WaitForSeconds(3f);

        //Play the like speech bubble. 
        MessageManager.instance.CallSpeechBubble(player.transform, 2);
        yield return new WaitForSeconds(2f);

        //Start the next event.
        StartCoroutine(Mo_Dialog_2());
    }

    //Mo gets into the room and sees the flying wok. 
    private IEnumerator Mo_Dialog_2()
    {
        //Mo shows up at the door.
        masterMo.GetComponent<SpriteRenderer>().enabled = true;
        moController.Idle(0f, 1f); //Facing up.
        yield return new WaitForSeconds(0.5f);

        //Play the exclamation speech bubble on Mo.
        MessageManager.instance.CallSpeechBubble(masterMo.transform, 0);
        yield return new WaitForSeconds(1f);

        //Mo walks fast towards the player. 
        Vector3 moStoppingPoint = moApproachSpot;
        float distance = (masterMo.transform.position - moStoppingPoint).sqrMagnitude;
        moController.Walk(0f, 1f); //Walking up.
        while (distance > 0f)
        {
            masterMo.transform.position = Vector3.MoveTowards(masterMo.transform.position,
                moStoppingPoint, 6f * Time.deltaTime);
            distance = (masterMo.transform.position - moStoppingPoint).sqrMagnitude;
            yield return null;
        }
        moController.Idle(0f, 1f); //Stop the walking animation.



        //Mo's dialog. 
        GameManager.instance.dialogSystem.Initialize(dialogs[dialogIndex]);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        dialogIndex++;

        //Player's mana dropping to zero. 
        while(player.GetComponent<Character>().GetMana() > 0)
        {
            player.GetComponent<Character>().UseMana(1f);
            yield return null;
        }
        //Play the exclamation mark speech bubble
        MessageManager.instance.CallSpeechBubble(player.transform, 0);
        yield return new WaitForSeconds(1f);

        //Play wok coming down animation
        wok.GetComponent<Animator>().SetTrigger("comeDown");
        //Mo's dialog
        yield return new WaitForSeconds(1f);
        GameManager.instance.dialogSystem.Initialize(dialogs[dialogIndex]);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        dialogIndex++;

        //Mo continues talking. 
        StartCoroutine(Mo_Dialog_3());
    }

    //Mo moves to the exit and his next dialog about going to the mountain next morning. 
    private IEnumerator Mo_Dialog_3()
    {
        yield return new WaitForSeconds(1f);

        //Mo moves to the exit.
        Vector3 moStoppingPoint = new Vector3(0f, -3f, 0);
        float distance = (masterMo.transform.position - moStoppingPoint).sqrMagnitude;
        moController.Walk(0f, -1f); //Walking down. 
        while (distance > 0f)
        {
            masterMo.transform.position = Vector3.MoveTowards(masterMo.transform.position,
                moStoppingPoint, 3f * Time.deltaTime);
            distance = (masterMo.transform.position - moStoppingPoint).sqrMagnitude;
            yield return null;
        }
        //Mo turns around. 
        moController.Idle(0f, 1f);
        yield return new WaitForSeconds(0.5f);
        

        //Mo starts talking about going to the mountain tomorrow. 
        GameManager.instance.dialogSystem.Initialize(dialogs[dialogIndex]);
        while (GameManager.instance.dialogSystem.GetIsDialogFinished() == false)
        {
            yield return new WaitForSeconds(1f);
        }
        dialogIndex++;
        yield return new WaitForSeconds(1f);

        //Mo moves further into the exit and disappears. 
        Vector3 moStoppingPoint2 = moEnterSpot;
        float distance2 = (masterMo.transform.position - moStoppingPoint2).sqrMagnitude;
        moController.Walk(0f, -1f);
        while (distance2 > 0f)
        {
            masterMo.transform.position = Vector3.MoveTowards(masterMo.transform.position,
                moStoppingPoint2, 3f * Time.deltaTime);
            distance2 = (masterMo.transform.position - moStoppingPoint2).sqrMagnitude;
            yield return null;
        }
        masterMo.SetActive(false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(Player_Movement_2());
    }

    //Player moves to the bed and goes to sleep. 
    private IEnumerator Player_Movement_2()
    {
        //Player walks towards the bed first part. 
        float distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetBool("moving", true);
        playerAnimator.SetFloat("vertical", -1); //Player moving down. 
        while (distance > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;
        //Player walks towards the bed second part
        float distance2 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
        StopMovingAndResetFacing(); //Reset facing. 
        playerAnimator.SetBool("moving", true);
        playerAnimator.SetFloat("horizontal", 1);

        while (distance2 > 0f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                targets[targetIndex], 3f * Time.deltaTime);
            distance2 = (player.transform.position - targets[targetIndex]).sqrMagnitude;
            yield return null;
        }
        targetIndex++;
        StopMovingAndResetFacing();//Stop moving at the bed. 
        playerAnimator.SetFloat("lastHorizontal", 1); //Set facing to right. 
        //The player sleeps.
        GamePauseController.instance.SetPause(true);
        GameSceneManager.instance.InitSwitchScene("HanLi's Room", new Vector3(5.7f, -1.6f, 0f));

        //Set this event to played. 
        EndEvent();
    }

    private void EndEvent()
    {
        EventController.instance.SetEventStatus(2, false);
        GamePauseController.instance.SetEvent(false);
    }

    private void StopMovingAndResetFacing()
    {
        playerAnimator.SetBool("moving", false);
        playerAnimator.SetFloat("lastVertical", 0);
        playerAnimator.SetFloat("lastHorizontal", 0);
        playerAnimator.SetFloat("vertical", 0);
        playerAnimator.SetFloat("horizontal", 0);
    }
}
