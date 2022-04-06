using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script contains all the saved transition target that will be used by transition area. 

public class TransitionTargetManager : MonoBehaviour
{
    public TransitionTargetList targetLists;
    public TransitionMapController transitionMapController;
    public GameObject transitionMapPanel;
    public string savedTransitionListJSON;

    public static TransitionTargetManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddTarget(string sceneName, Vector3 pos)
    {
        targetLists.pairs.Add(new ScenePositionPairs(sceneName, pos));
    }

    public void ResetTargets()
    {
        targetLists.pairs = new List<ScenePositionPairs>();
    }

 //Saving the transition target lists. 

    [System.Serializable] 
    public class SaveTransitionTargetData
    {
        public string sceneName;
        public float[] pos;

        public SaveTransitionTargetData(ScenePositionPairs scenePositionPair)
        {
            pos = new float[3];
            sceneName = scenePositionPair.sceneName;
            pos[0] = scenePositionPair.spawnPosition.x;
            pos[1] = scenePositionPair.spawnPosition.y;
            pos[2] = scenePositionPair.spawnPosition.z;
        }
    }

    [System.Serializable]
    public class SaveTransitionTargetListData
    {
        public List<SaveTransitionTargetData> targetDataList;

        public SaveTransitionTargetListData(TransitionTargetList targetLists)
        {
            targetDataList = new List<SaveTransitionTargetData>();
            for(int i = 0; i < targetLists.pairs.Count; i++)
            {
                targetDataList.Add(new SaveTransitionTargetData(targetLists.pairs[i]));
            }
        }

    }

    public void Save()
    {
        SaveTransitionTargetListData transitionTargetsSave = new SaveTransitionTargetListData(targetLists);
        savedTransitionListJSON = JsonUtility.ToJson(transitionTargetsSave);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/TransitionTargetsList.json", savedTransitionListJSON);
    }

    public void Load()
    {
        string loadTemp = System.IO.File.ReadAllText(Application.persistentDataPath + "/TransitionTargetsList.json");
        
        if (loadTemp == "" || loadTemp == "{}" || loadTemp == null) 
        {
            Debug.Log("transition list save file not found.");
            return; 
        }

        else
        {
            SaveTransitionTargetListData loadTransitionTargetListData = JsonUtility.FromJson<SaveTransitionTargetListData>(loadTemp);
            targetLists.pairs = new List<ScenePositionPairs>();
            for (int i = 0; i < loadTransitionTargetListData.targetDataList.Count; i++)
            {
                string name = loadTransitionTargetListData.targetDataList[i].sceneName;
                Vector3 pos = new Vector3();
                pos.x = loadTransitionTargetListData.targetDataList[i].pos[0];
                pos.y = loadTransitionTargetListData.targetDataList[i].pos[1];
                pos.z = loadTransitionTargetListData.targetDataList[i].pos[2];
                targetLists.pairs.Add(new ScenePositionPairs(name, pos));
            }

        }
    }

}

