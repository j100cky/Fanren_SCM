using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionType
{
	Warp,
	Scene,
	RandomScene
}


//Create a new class for the scene-position pair that will be used for random scene transition. 
[Serializable]
public class ScenePositionPairs
{
	public string sceneName;
	public Vector3 spawnPosition;

	public ScenePositionPairs(string name, Vector3 pos)
    {
		sceneName = name;
		spawnPosition = pos;
    }

}

public class Transition : MonoBehaviour
{
    [SerializeField] public TransitionType transitionType;
    [SerializeField] public string sceneNameToTransition;
	[SerializeField] public Vector3 targetPosition;
	//A list of scene-position pairs for random scene transition. 
	[SerializeField] List<ScenePositionPairs> randomSceneNamesToTransit;
	//Index that will be generated randomly and determines which scene-position pair to use for random scene transition. 
	private int sceneIndex; 

	Transform destination;

	//A constructor that will be called by other script if it wants to create a transition. 
	public Transition(TransitionType type, string name, Vector3 pos)
    {
		transitionType = type;
		sceneNameToTransition = name;
		targetPosition = pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.GetChild(1);
    }

	internal void InitiateTransition(Transform toTransition)
	{
		switch (transitionType)
		{
			case TransitionType.Warp:

				Cinemachine.CinemachineBrain currentCamera = 
					Camera.main.GetComponent<Cinemachine.CinemachineBrain>();

				currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
					toTransition,
					destination.position - toTransition.position
					);

				toTransition.position = new Vector3(
					destination.position.x,
					destination.position.y,
					toTransition.position.z);
				break;

			case TransitionType.Scene:

				GamePauseController.instance.SetPause(true);
				GameSceneManager.instance.InitSwitchScene(sceneNameToTransition, targetPosition);
				break;

			case TransitionType.RandomScene:

				GamePauseController.instance.SetPause(true);
				//Reset the scene index to another random number. 
				sceneIndex = UnityEngine.Random.Range(0, randomSceneNamesToTransit.Count);
				//Initiate scene change. 
				GameSceneManager.instance.InitSwitchScene(randomSceneNamesToTransit[sceneIndex].sceneName, 
					randomSceneNamesToTransit[sceneIndex].spawnPosition);
				break;
		}

		
	}
}
