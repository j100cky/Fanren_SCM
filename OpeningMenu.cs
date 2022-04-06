using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OpeningMenu : MonoBehaviour
{
    [SerializeField] string essentialSceneName;
    [SerializeField] string startingSceneName;

    AsyncOperation operation;

    //Function for the exit button. 
    public void ExitGame()
    {
        Debug.Log("Exiting the game!");
        Application.Quit();
    }

    //Function used by the New Game button.
    public void StartNewGame()
    {
        SceneManager.LoadScene(startingSceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(essentialSceneName, LoadSceneMode.Additive);
    }

}
