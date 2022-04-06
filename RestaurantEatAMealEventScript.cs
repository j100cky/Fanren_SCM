using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantEatAMealEventScript : MonoBehaviour
{
    [SerializeField] Vector3 tablePosition;
    Animator playerAnimator;
    CharacterController2D charController;

    private void Start()
    {
        //Get the components.
        playerAnimator = GameManager.instance.player.GetComponent<Animator>();
        charController = GameManager.instance.player.GetComponent<CharacterController2D>();

        //Set the game mode to event mode to hide some UIs.
        GamePauseController.instance.SetEvent(true);

        //Move the player to the table.
        StartCoroutine(EventAction());


    }

    //Everything in the event is done in Coroutine. 
    private IEnumerator EventAction()
    {
        //Move the player to the table. 
        //Turn right and play the walking animation..
        charController.ResetParametersAllZero();
        charController.WalkRight();
        float distance3 = (GameManager.instance.player.transform.position - tablePosition).sqrMagnitude;
        while (distance3 > 0)
        {
            GameManager.instance.player.transform.position = Vector3.MoveTowards(GameManager.instance.player.transform.position,
                tablePosition, 2f * Time.deltaTime);
            distance3 = (GameManager.instance.player.transform.position - tablePosition).sqrMagnitude;
            yield return null;
        }

        //After the player is at the table, play some animation. 
        yield return new WaitForSeconds(3f);

        //Heal the player.
        float hp = GameManager.instance.character.GetMaxHealth();
        GameManager.instance.character.RestoreHealth(hp);
        yield return new WaitForSeconds(0.2f);

        //After the food is consumed, show the dialog.
        PlayDialog();


    }

    private void PlayDialog()
    {
        List<string> temp = new List<string>();
        temp.Add("一顿饱餐过后，你的体力恢复!");
        DialogContainer dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(temp);
        GameManager.instance.dialogSystem.Initialize(dialog);
        GamePauseController.instance.SetEvent(false); 
    }

}
