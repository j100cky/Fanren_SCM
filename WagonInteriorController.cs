using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

//This script is attached to the WagonInterior(closed door) game object. 
public class WagonInteriorController : MonoBehaviour
{
    [SerializeField] GameObject wagonArrived;
    [SerializeField] DialogContainer dialog;
    Vector3 lastPos; //Used to calculate position change of the wagon so the player can change accordingly. 

    private void Start()
    {
        //Player should not be allowed to walk around until the animation is finished. 
        GamePauseController.instance.SetPause(true);
        lastPos = transform.position;
        ResetGameData();
    }

    private void Update()
    {
        ControlPlayer();
    }

    //Reset the game status since this scene is only shown in new games. 
    public void ResetGameData()
    {
        GameSaveManager.instance.ResetGameStatus();
    }

    //This function allows the player to move with the wagon's tumbling. 
    public void ControlPlayer()
    {
        GameManager.instance.player.transform.position += transform.position - lastPos;
        lastPos = transform.position;
    }

    public void StartShowArrivedImage()
    {
        //The last animation frame will call the start of the script. 
        StartCoroutine(ShowArrivedImage());
    }

    public IEnumerator ShowArrivedImage()
    {
        wagonArrived.SetActive(true); //Show the WagonInterior(opened door).
        gameObject.GetComponent<SpriteRenderer>().enabled = false;//Hide the WagonInterior(closed door).
        yield return new WaitForSeconds(0.5f); //Wait for 0.5 second to create a lagged feeling. 
        gameObject.SetActive(false);//Deactivate the WagonInterior(closed door). *If this is called earlier, the coroutine will not start. 
        GameManager.instance.dialogSystem.Initialize(dialog); //Initiate the designated dialog. 
    }

}
