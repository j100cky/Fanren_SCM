using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameMenu : MonoBehaviour
{
    [SerializeField] string essentialSceneName;

    AsyncOperation operation;

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

    //Function used by the load buttons. 
    public void LoadGame()
    {
        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/PlayerScenePosData.json");
        if (jsonString == "" || jsonString == "{}" || jsonString == null) { return; }
        SaveCharScenePositionData loadedData = JsonUtility.FromJson<SaveCharScenePositionData>(jsonString);
        Vector3 playerPos = new Vector3(loadedData.savedPos[0], loadedData.savedPos[1],
            loadedData.savedPos[2]);
        string savedScene = loadedData.savedScene;

        SceneManager.LoadScene(savedScene, LoadSceneMode.Single);
        SceneManager.LoadScene(essentialSceneName, LoadSceneMode.Additive);
        /*GameManager.instance.player.transform.position = playerPos;*/

    }
}
