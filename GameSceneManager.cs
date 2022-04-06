using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	public static GameSceneManager instance;
	
	private void Awake()
	{
		instance = this;
	}

	[SerializeField] ScreenTint screenTint; //Used to provide the screen tint animation. 
	public string currentScene;

	[SerializeField] List<GameObject> thingsToHideEventMode; //List of elements that should be hidded when entering an event. 

	[SerializeField] CameraConfiner cameraConfiner; //for camera confiner bounds
	AsyncOperation unload; //For evaluating whether scene switch is complete. 
	AsyncOperation load; //For evaluating whether scene switch is complete. 

	void Start()
	{
		//Update the currentScene. The currentScene will be unloaded during scene switch. 
		currentScene = SceneManager.GetActiveScene().name;
	}

	public void InitSwitchScene(string to, Vector3 targetPosition)
	{
		StartCoroutine(Transition(to, targetPosition));
	}

	//Use InitSwitchToEventScene() when we want to switch to an event. The elements will be hidded, that's the difference. 
	public void InitSwitchToEventScene(string to, Vector3 targetPosition)
    {
		StartCoroutine(TransitionToEventScene(to, targetPosition));
    }


	IEnumerator TransitionToEventScene(string to, Vector3 targetPosition)
    {
		//Start tinting dark. 
		screenTint.Tint();
		yield return new WaitForSeconds(1f / screenTint.speed + 0.1f);

		//Start the scene switch when the screen is completely dark. 
		SwitchScene(to, targetPosition);

		//Hide all the elements that we want to be hidded (e.g. toolbar, skillbar, hp, mp, exp bars). 
		if(thingsToHideEventMode != null)
        {
			for (int i = 0; i < thingsToHideEventMode.Count; i++)
			{
				thingsToHideEventMode[i].SetActive(false);
			}
		}


		//Scene switch takes some time. While they are still switching, don't untint the screen. 
		while (load != null && unload != null)
		{
			if (load.isDone) { load = null; }
			if (unload.isDone) { unload = null; }
			yield return new WaitForSeconds(0.1f);
		}
		//After scene switch is completed, set the currentScene to the active scene. 
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));

		cameraConfiner.UpdateBounds();

		//After everything is set, untint the screen. 
		screenTint.UnTint();
	}

	IEnumerator Transition(string to, Vector3 targetPosition)
	{
		screenTint.Tint();

		yield return new WaitForSeconds(1f / screenTint.speed + 0.1f);
		SwitchScene(to, targetPosition);

		while(load != null && unload != null)
		{
			if(load.isDone) {load = null;}
			if(unload.isDone) {unload = null;}
			yield return new WaitForSeconds(0.1f);
		}

		//Call the ResetNPCIsShown() function in the GameNPCManager script so that destroyed NPC will be re-instantiated when the 
		//scene is re-entered. 
		GameNPCManager3.instance.ResetNPCIsShown();

		//Set the player sprite active. Sometimes the player is hidden in the previous scene.
		if (GameManager.instance.player.activeInHierarchy == false)
		{
			GameManager.instance.player.SetActive(true);
		}

		//Show all the hidden elements if they were hidded. 
		for (int i = 0; i < thingsToHideEventMode.Count; i++)
		{
			if(thingsToHideEventMode[i].activeInHierarchy == false)
            {
				thingsToHideEventMode[i].SetActive(true);
			}

		}


		SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));

		cameraConfiner.UpdateBounds();

		screenTint.UnTint();
		//Resume the game
		GamePauseController.instance.SetPause(false);
	}

	public void SwitchScene(string to, Vector3 targetPosition)
	{
		//Give load the value so the while() loop can assess whether scene switch is completed. 
		load = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
		//Give unload the value so the while() loop can assess whether scene unload is completed. 
		unload = SceneManager.UnloadSceneAsync(currentScene);
		//Set the active scene to the scene that is trnasitioned to, so next time this scene will be unloaded, not the Essential scene. 
		currentScene = to;

		//Setting the camera correctly. 
		Transform playerTransform = GameManager.instance.player.transform;

		CinemachineBrain currentCamera = Camera.main.GetComponent<CinemachineBrain>();
		Debug.Log(currentCamera);
		currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
					playerTransform,
					targetPosition - playerTransform.position
					);

		//Setting the player's position according to the function's parameters. 
		playerTransform.position = new Vector3(
			targetPosition.x,
			targetPosition.y,
			playerTransform.position.z);
	}

	//Called by the return to main menu button 
	public void ReturnToMain()
    {
		//Save the game before exiting. 
		GameSaveManager.instance.Save();
		//Load the opening scene.
		SceneManager.LoadScene("Opening", LoadSceneMode.Single);
	}


	//Saving of scene data. 

	[Serializable]
	public class SaveCharScenePositionData
    {
		public string savedScene;
		public float[] savedPos = new float[3]; 

		public SaveCharScenePositionData(string currentScene, Vector3 position)
        {
			savedScene = currentScene;
			savedPos[0] = position.x;
			savedPos[1] = position.y;
			savedPos[2] = position.z;
        }

    }
	[SerializeField] string jsonString;

	public void Save()
    {
		SaveCharScenePositionData charScenePositionData = new SaveCharScenePositionData(currentScene, 
			GameManager.instance.player.transform.position);
		jsonString = JsonUtility.ToJson(charScenePositionData);
		System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerScenePosData.json", jsonString);
    }

	public void Load()
    {
		jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/PlayerScenePosData.json");
		if (jsonString == "" || jsonString == "{}" || jsonString == null) { return; }
		SaveCharScenePositionData loadedData = JsonUtility.FromJson<SaveCharScenePositionData>(jsonString);
		Vector3 playerPos = new Vector3(loadedData.savedPos[0], loadedData.savedPos[1],
			loadedData.savedPos[2]);
		string savedScene = loadedData.savedScene;
		InitSwitchScene(savedScene, playerPos);
	}
}
