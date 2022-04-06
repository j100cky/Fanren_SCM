using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
	[SerializeField] Text targetText;
	[SerializeField] Text nameText;
	[SerializeField] Image portrait;
	[SerializeField] NPCQuestController npcQuestController;
	[SerializeField] YesNoButtonController yesNoButtonController;
	[SerializeField] public GameObject yesButton;
	[SerializeField] public GameObject noButton;

	DialogContainer currentDialog;
	int currentTextLine;
	bool showYesNo = false; //For determining whether the yes/no buttons should be shown after the last line. 

//Declaring a few parameters for showing the dialog slowly.
	[SerializeField] float visibleTextPercent; 
	[Range(0f,1f)]
	[SerializeField] float timePerLetter = 0.05f;
	float totalTimeToType, currentTime;
	string lineToShow;
	bool isDialogFinished; 
		public bool GetIsDialogFinished()
		{
			return isDialogFinished;
		}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			PushText();
		}
		TypeOutText();
	}

	private void TypeOutText()
	{
		if(visibleTextPercent >= 1f) {return;}
		currentTime += Time.deltaTime;
		visibleTextPercent = currentTime/totalTimeToType;
		visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f);
		UpdateText();
	}

	void UpdateText()
	{
		int letterCount = (int)(lineToShow.Length * visibleTextPercent);
		targetText.text = lineToShow.Substring(0, letterCount);
	}

	private void PushText()
	{
		if(visibleTextPercent < 1f)
		{
			visibleTextPercent = 1f; 
			UpdateText();
			return;
		}

		if(currentTextLine >= currentDialog.line.Count)
		{
			Conclude(); 
		}
		else{
			CycleLine();
		}
	}

	void CycleLine()
	{
		lineToShow = currentDialog.line[currentTextLine];
		totalTimeToType = lineToShow.Length * timePerLetter;
		currentTime = 0f;
		visibleTextPercent = 0f;
		targetText.text = "";

		currentTextLine += 1;
	}

	public void Initialize(DialogContainer dialogContainer, bool showYesNo = false)
	{
		this.showYesNo = showYesNo;
		yesButton.SetActive(showYesNo);
		noButton.SetActive(showYesNo);
		GamePauseController.instance.SetPause(true);
		isDialogFinished = false;
		Show(true);
		currentDialog = dialogContainer;
		currentTextLine = 0;
		//targetText.text = currentDialog.line[currentTextLine];
		CycleLine();
		UpdatePortrait();
	}

	private void UpdatePortrait()
	{
		if(currentDialog.actor != null)
        {
			portrait.enabled = true;
			nameText.enabled = true;
			portrait.sprite = currentDialog.actor.portrait;
			nameText.text = currentDialog.actor.Name;
		}
        else
        {
			portrait.enabled = false;
			nameText.enabled = false;
		}


	}

	//We need this function so that the NPC can tell the DialogSystem which Quest they carries in the TalkInteract script. 
	public void GetNPCQuestController(NPCQuestController importedController) 
    {
		npcQuestController = importedController;
	}



	private void Conclude()
	{
		//If the dialog does not require a response, it will go away after the dialog is finished. 
		if(showYesNo == false)
        {
			isDialogFinished = true;
			//Debug.Log("The dialogue has ended.");
			GamePauseController.instance.isPaused = false; //Resume the game after interaction
			Show(false);	

        }
		//If the dialog requires a response, the showYesNo will be set true.And the Yes/No buttons will be set active. 
		else
        {
			yesNoButtonController.ShowButtons(true);

		}

		//If a questcontroller is imported, show the quest panel, and set the quest controller back to null. 
		if (npcQuestController != null)
		{
			npcQuestController.ShowPanel();
			npcQuestController = null; //Set the controller to null so that the next normal npc dialog can happen without quest. 
			return; //End the function so that trade panel will not appear. 
		}

		//If the dialog container has a trade panel, then show the trade panel after the dialog is finished. 
		if (currentDialog.tradePanel != null)
        {
			//Pause the game if changing from dialog panel to trade panel.
			GamePauseController.instance.isPaused = true; 

			//Show the panel. 
			GameManager.instance.tradePanel.GetComponent<TradeSystem>().SetTradeItemContainer(currentDialog.tradePanel);
		}
		else if (currentDialog.eventAfterInteraction != null)
        {

			GameObject go = Instantiate(currentDialog.eventAfterInteraction);
        }

	}

	public void Show(bool v)
	{
		gameObject.SetActive(v);
	}

}
