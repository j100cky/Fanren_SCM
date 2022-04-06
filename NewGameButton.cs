using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    AsyncOperation operation;

    //This function is called by onclick of the button in the new game menu. 
    public void TransitToNewGameScene()
    {
        SceneManager.LoadScene("WagonInterior", LoadSceneMode.Single);
        SceneManager.LoadScene("Essentials", LoadSceneMode.Additive);
    }
}
