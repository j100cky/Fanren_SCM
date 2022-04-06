using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script stores a list of crops container (one for each scene) so that all of then can be saved and loaded. 

public class CropsContainerListController : MonoBehaviour
{
    public CropsContainerList containerList;
    public static CropsContainerListController instance;

    private void Awake()
    {
        instance = this;
    }

    //Called when all containers are to be reset (such as new game).
    public void ResetAll()
    {
        for(int i = 0; i < containerList.cropsContainerList.Count; i++)
        {
            containerList.cropsContainerList[i].RemoveAll();
        }
    }

    //Saving and Loading crops in each CropsContainer. 

    [System.Serializable]
    public class CropsTileSaveData
    {
        public int cropID; //For the identity of crop. 
        public int[] pos; //For croptile's position. 
        public int growthTimer;
        public int growthStage;
        public bool isGrown;

        public CropsTileSaveData(CropTile cropTile)
        {
            if(cropTile.crop!= null) 
            {
                cropID = cropTile.crop.cropID;
                pos = new int[3];
                pos[0] = cropTile.position.x;
                pos[1] = cropTile.position.y;
                pos[2] = cropTile.position.z;
                growthTimer = cropTile.growTimer;
                growthStage = cropTile.growStage;
                isGrown = cropTile.isGrown;
            }
            else //In the case of plowed but not seeded land. 
            {
                cropID = -1;
                pos = new int[3];
                pos[0] = cropTile.position.x;
                pos[1] = cropTile.position.y;
                pos[2] = cropTile.position.z;
            }


        }
    }

    [System.Serializable]
    public class CropsContainerSaveData
    {
        public List<CropsTileSaveData> cropsTileListSave;
        public CropsContainerSaveData(CropsContainer container)
        {
            cropsTileListSave = new List<CropsTileSaveData>();
            for (int i = 0; i < container.crops.Count; i++)
            {
                cropsTileListSave.Add(new CropsTileSaveData(container.crops[i]));
            }
        }

    }

    [System.Serializable]
    public class CropsContainerListSaveData
    {
        public List<CropsContainerSaveData> cropContainerListSave;
        public CropsContainerListSaveData(CropsContainerList containerList)
        {
            cropContainerListSave = new List<CropsContainerSaveData>();
            for (int i = 0; i < containerList.cropsContainerList.Count; i++)
            {
                cropContainerListSave.Add(new CropsContainerSaveData(containerList.cropsContainerList[i]));
            }
        }
    }

    public void Save()
    {
        CropsContainerListSaveData savedData = new CropsContainerListSaveData(containerList);
        string jsonString = JsonUtility.ToJson(savedData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "CropsContainerList" + ".json", jsonString);
        Debug.Log("CropsTile data saved");
    }

    public void Load()
    {
        string loadTemp = System.IO.File.ReadAllText(Application.persistentDataPath + "CropsContainerList" + ".json");
        if (loadTemp == "" || loadTemp == "{}" || loadTemp == null)
        {
            Debug.Log("cropscontainerlist json not found");
            return;
        }
        //Clear the current crops container. 
        for(int i = 0; i < containerList.cropsContainerList.Count; i++)
        {
            containerList.cropsContainerList[i].RemoveAll();
        }

        CropsContainerListSaveData loadData = JsonUtility.FromJson<CropsContainerListSaveData>(loadTemp);
        //First loop through the list of containers. 
        for (int i = 0; i < loadData.cropContainerListSave.Count; i++)
        {
            //For each container, loop through its list of croptiles
            for (int j = 0; j < loadData.cropContainerListSave[i].cropsTileListSave.Count; j++)
            {
                //Load the data of each croptile into the corresponding cropcontainer.
                if(loadData.cropContainerListSave[i].cropsTileListSave[j].cropID == -1) //In the case of plowed but not seeded. 
                {
                    CropTile tempCropTile = new CropTile();
                    tempCropTile.crop = null;
                    tempCropTile.position = new Vector3Int(loadData.cropContainerListSave[i].cropsTileListSave[j].pos[0],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[1],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[2]);
                    containerList.cropsContainerList[i].Add(tempCropTile);
                    //containerList.cropsContainerList[i].crops[j].crop = null;

/*                    containerList.cropsContainerList[i].crops[j].position =
                        new Vector3Int(loadData.cropContainerListSave[i].cropsTileListSave[j].pos[0],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[1],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[2]);*/
                }
                else
                {
                    CropTile tempCropTile = new CropTile();
                    tempCropTile.crop =
                        GameManager.instance.cropDB.cropsList[loadData.cropContainerListSave[i].cropsTileListSave[j].cropID];

                    tempCropTile.position =
                        new Vector3Int(loadData.cropContainerListSave[i].cropsTileListSave[j].pos[0],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[1],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[2]);

                    tempCropTile.growTimer = loadData.cropContainerListSave[i].cropsTileListSave[j].growthTimer;
                    tempCropTile.growStage = loadData.cropContainerListSave[i].cropsTileListSave[j].growthStage;
                    tempCropTile.isGrown = loadData.cropContainerListSave[i].cropsTileListSave[j].isGrown;
                    containerList.cropsContainerList[i].Add(tempCropTile);

/*                    containerList.cropsContainerList[i].crops[j].crop = 
                        GameManager.instance.cropDB.cropsList[loadData.cropContainerListSave[i].cropsTileListSave[j].cropID];

                    containerList.cropsContainerList[i].crops[j].position =
                        new Vector3Int(loadData.cropContainerListSave[i].cropsTileListSave[j].pos[0],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[1],
                        loadData.cropContainerListSave[i].cropsTileListSave[j].pos[2]);

                    containerList.cropsContainerList[i].crops[j].growTimer = loadData.cropContainerListSave[i].cropsTileListSave[j].growthTimer;
                    containerList.cropsContainerList[i].crops[j].growStage = loadData.cropContainerListSave[i].cropsTileListSave[j].growthStage;
                    containerList.cropsContainerList[i].crops[j].isGrown = loadData.cropContainerListSave[i].cropsTileListSave[j].isGrown;*/
                }

            }
        }
        Debug.Log("CropsTile data loaded");
    }
}

