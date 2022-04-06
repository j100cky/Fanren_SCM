using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseController : MonoBehaviour //This script will list the scenarios which we want to game to pause. 
{
	private bool isEventMode; //When isEventMode is true, game is always paused. 
	public bool isPaused; //This bool is set from other scripts, such as inventory controller, etc, and read by scripts like GatherResourceNode ToolActions.//
	public bool isCoolingDown; //While this is true, ToolActions cannot be used. 
	public bool isShiftPressed; //Pressing shift allows many other functions, including bulk sell.

	public static GamePauseController instance;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
    {
		isShiftPressed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		//When either shift key is pressed down, isShiftPressed will remain true.
		if(isEventMode == true)
        {
			isPaused = true;
        }
	}

	public void SetEvent(bool b)
    {
		isEventMode = b;
    }

	public void SetPause(bool b)
    {
		isPaused = b;
    }

	public void SetCoolDown(bool b)
    {
		isCoolingDown = b;
    }

	public bool GetPause()
    {
		return isPaused;
    }

	public bool GetCoolDown()
    {
		return isCoolingDown;
    }

}
