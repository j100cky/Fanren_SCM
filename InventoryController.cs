using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
	[SerializeField] GameObject panel;
	[SerializeField] GameObject toolbarPanel;
	public bool blockPreviews = false;//Read by the SkillCastPreviewScript so that preview will be inactive when the inventory is opened.//

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))//Toggling the inventory page.
		{
			panel.SetActive(!panel.activeInHierarchy);
			panel.GetComponent<PanelGroup>().Show(1); //Make the main panel appear first.
			panel.GetComponentInChildren<ButtonPanel>().panelButtons[1].ActivateMe(); //Show the inventory panel button.
			blockPreviews = !blockPreviews;
			toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);//Hide the toolbar when the panel is active.
			GamePauseController.instance.isPaused = 
				!GamePauseController.instance.isPaused; //Pause the game while the panel is active. 
		}

		if(Input.GetKeyDown(KeyCode.E)) //Toggling the equipment page.
		{
			panel.SetActive(!panel.activeInHierarchy); //Make the main panel appear first.
			panel.GetComponent<PanelGroup>().Show(0); //Show the status page. 
			panel.GetComponentInChildren<ButtonPanel>().panelButtons[0].ActivateMe();//Show the status panel button.
			toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy); //Hide the toolbar when the panel is active. 
			GamePauseController.instance.isPaused = 
				!GamePauseController.instance.isPaused; //Pause the game while the panel is active. 
		}

		if(Input.GetKeyDown(KeyCode.K)) //Toggling the skill page.
        {
			ShowSkillPanel();
		}

		if (Input.GetKeyDown(KeyCode.P)) //Toggle the pet page
		{
			panel.SetActive(!panel.activeInHierarchy); //Make the main panel appear first.
			panel.GetComponent<PanelGroup>().Show(3); //Show the pet page. 
			panel.GetComponentInChildren<ButtonPanel>().panelButtons[3].ActivateMe();//Show the pet panel button.
			toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy); //Hide the toolbar when the panel is active. 
			GamePauseController.instance.isPaused = 
				!GamePauseController.instance.isPaused; //Pause the game while the panel is active. 
		}

		if(Input.GetKeyDown(KeyCode.Q)) //Toggle the quest page
		{
			panel.SetActive(!panel.activeInHierarchy); //Make the main panel appear first.
			panel.GetComponent<PanelGroup>().Show(4); //Show the quest page.
			panel.GetComponentInChildren<ButtonPanel>().panelButtons[4].ActivateMe();//Show the quest panel button.
			toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy); //Hide the toolbar when the panel is active. 
			GamePauseController.instance.isPaused = 
				!GamePauseController.instance.isPaused; //Pause the game while the panel is active. 
		}

		if(Input.GetKeyDown(KeyCode.M)) //Toggle the map page
		{
			panel.SetActive(!panel.activeInHierarchy); //Make the main panel appear first.
			panel.GetComponent<PanelGroup>().Show(5); //Show the map page. 
			panel.GetComponentInChildren<ButtonPanel>().panelButtons[5].ActivateMe();//Show the map panel button.
			toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy); //Hide the toolbar when the panel is active. 
			GamePauseController.instance.isPaused = 
				!GamePauseController.instance.isPaused; //Pause the game while the panel is active. 
		}


		if(Input.GetKeyDown(KeyCode.Escape))
		{
			HidePanels();

		}
	}

	//This will be called by both pressing K or clicking on the main skillbook.
	public void ShowSkillPanel()
    {
		panel.SetActive(!panel.activeInHierarchy); //Make the main panel appear first.
		panel.GetComponent<PanelGroup>().Show(2); //Show the skill page. 
		panel.GetComponentInChildren<ButtonPanel>().panelButtons[2].ActivateMe();//Show the skills panel button.
		toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy); //Hide the toolbar when the panel is active. 
		GamePauseController.instance.isPaused =
		!GamePauseController.instance.isPaused; //Pause the game while the panel is active. 
	}

	public void HidePanels()
    {
		panel.SetActive(false); //set the object inactive.
		toolbarPanel.SetActive(true);   //Show the toolbar.
		GamePauseController.instance.isPaused = false; //Resume
		GameManager.instance.itemPageController.Clear(); //Hide the item page so that when the player closes an inventory,
														 //the item page will be gone too.
	}
}
