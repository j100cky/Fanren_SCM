using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionTargetButton : MonoBehaviour
{
    public int myIndex;
    public string transitionTargetSceneName;
    public Vector3 targetPos;
    public Text targetText; 

    //Called by the panel, set the index of this button. 
    public void Set(int index)
    {
        myIndex = index; 
    }

    //Called by the panel, set the scene information of this button. 
    public void SetTransitionTarget(ScenePositionPairs pair)
    {
        transitionTargetSceneName = pair.sceneName;
        targetPos = pair.spawnPosition;
        string combinedText = transitionTargetSceneName + ", ·½Î»" + targetPos.x.ToString() + "," + targetPos.y.ToString();
        targetText.text = combinedText;
    }

    //Called by OnClick. Transition the player to the designated scene and position. 
    public void IntiateTransition()
    {
        GamePauseController.instance.SetPause(true);

        GameSceneManager.instance.InitSwitchScene(transitionTargetSceneName, targetPos);
    }

    //Called by the panel to refresh the list. 
    public void RemoveButton()
    {
        Destroy(gameObject);
    }

}
