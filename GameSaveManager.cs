using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    [SerializeField] ItemPanel inventoryPanel;
    [SerializeField] SkillbookListPanelController skillbookPanel;

    public static GameSaveManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Save()
    {
        //Saving player status.
        GameManager.instance.player.GetComponent<Character>().Saving();
        //Saving player inventory.
        inventoryPanel.SaveContainerData();
        //Saving player's skillbook list data
        skillbookPanel.Save();
        //Saving player's scene.
        GameSceneManager.instance.Save();
        //Saving game time. 
        GameManager.instance.timeController.Save();
        //Saving transition targets list. 
        TransitionTargetManager.instance.Save();
        //Saving placeable objects. 
        GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().Save();
        //Saving skillbar data
        GameManager.instance.skillBarOnScreen.Save();
        //Saving croptiles. 
        CropsContainerListController.instance.Save();
    }

    public void Load()
    {
        //Hiding the TopPanel
        GameManager.instance.topPanel.SetActive(false);
        //Loading player status.
        GameManager.instance.player.GetComponent<Character>().Loading();
        //Loading the player inventory data
        inventoryPanel.LoadContainerData();
        //Loading the player's skillbook list data.
        skillbookPanel.Load();
        //Loading player scene and position.
        GameSceneManager.instance.Load();
        //Loading game time. 
        GameManager.instance.timeController.Load();
        //Loading transition target lists. 
        TransitionTargetManager.instance.Load();
        //Loading placeable objects.
        GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().Load();
        //Loading skillbar data
        GameManager.instance.skillBarOnScreen.Load();
        //Loading croptiles. 
        CropsContainerListController.instance.Load();
    }

    //Called by options such as starting a new game. 
    public void ResetGameStatus()
    {
        //Reset inventory container. 
        GameManager.instance.inventoryContainer.ClearAll();
        //Reset char status. 
        GameManager.instance.character.ResetCharStatus();
        //Reset skill container.
        GameManager.instance.skillbookContainer.ResetSkillbookContainer();
        //Reset pet panel. 
        GameManager.instance.petPanel.GetComponent<PetPanel>().ResetPetList();
        //Reset saved transition targets. 
        TransitionTargetManager.instance.ResetTargets();
        //Reset all placeable objects. 
        GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().RemoveAll();
        //Reset all croptiles. 
        CropsContainerListController.instance.ResetAll();

    }
}
