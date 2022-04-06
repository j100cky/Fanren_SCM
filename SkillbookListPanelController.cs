using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillbookListPanelController : MonoBehaviour
{
    [SerializeField] List<OneSkillbookPanelScript> skillbookPanels;
    [SerializeField] SkillbookContainer skillbookContainer;
    [SerializeField] GameObject practiceButton;
    [SerializeField] SkillbookDetailController skillbookDetailController;
    [SerializeField] Actor actor; //Used for creating dialog. 
    [SerializeField] Skillbooks mainSkillbook;
    int selectedPanelIndex;
    int skillbookNumber;
    [SerializeField] float tick; // Used for getting skillbook exp.
    
    private void Start()
    {
        UpdateList();
    }

    private void OnEnable()
    {
        UpdateList();
    }

    public void UpdateList()
    {

        DetermineNumber();
        SetPanels();
        SetIndex();
        Set();
    }

    //Determine the number of itemslots in the container. 
    private void DetermineNumber()
    {
        //First reset skillbookNumber. 
        skillbookNumber = 0;
        for (int i = 0; i < skillbookContainer.skillbooks.Count; i++)
        {
            if(skillbookContainer.skillbooks[i] != null)
            {
                skillbookNumber++;
            }
        }
    }

    //Set certain number of panels active according to how many items in the container. 
    private void SetPanels()
    {
        for (int i = 0; i < skillbookNumber; i++)
        {
            skillbookPanels[i].gameObject.SetActive(true);
        }
    }

    //Setting the index of each OneSkillbookPanel.
    private void SetIndex()
    {
        for(int i = 0; i < skillbookNumber; i++)
        {
            skillbookPanels[i].SetIndex(i);
        }
    }

    //Set the icon and name of each panel. 
    private void Set()
    {
        for(int i = 0; i < skillbookNumber; i++)
        {
            skillbookPanels[i].Set(skillbookContainer.skillbooks[i]);
        }
    }

    //This is called when a OneSkillbookPanel is clicked. Called in the OneSkillbookPanelController script.
    public void OnSkillbookPanelClicked(Skillbooks skillbookItemSlot, int myIndex)
    {
        ClearHighlight();
        ShowPracticeButton();
        ShowDetailPreview(skillbookItemSlot);
        SetSelectedSkillbookPanel(myIndex);
    }

    //Resetting the highlight before showing the next highlight. This is called by the OneSkillbookPanelScript. 
    public void ClearHighlight()
    {
        for(int i = 0; i < skillbookNumber; i++)
        {
            skillbookPanels[i].Highlight(false);
        }
        //Will change the sprite of the highlighter. 
    }

    //Showing the Practice button so that the player can change skillbook. This is called when a panel is clicked. 
    public void ShowPracticeButton()
    {
        practiceButton.SetActive(true);
    }

    //When the panel is clicked, tell the panel controller which panel is being clicked. 
    //This number will be used to determine which itemslot/panel the main skillbook is.
    public void SetSelectedSkillbookPanel(int index)
    {
        selectedPanelIndex = index;
    }

    //Showing the detailed preview. 
    public void ShowDetailPreview(Skillbooks skillbookItemSlot)
    {
        skillbookDetailController.SetVariables(skillbookItemSlot); //Will change it to a different sprite later. 
    }

    //Show the main skillbook stamp. This is called by the PracticeButton
    public void SetMainSkillbook()
    {
        //Clear the previous main skillbook stamp for each skillbook panel.
        for (int i = 0; i < skillbookPanels.Count; i++)
        {
            skillbookPanels[i].ShowStamp(false);
        }
        //Set the stamp on the selected skillbook panel. 
        skillbookPanels[selectedPanelIndex].ShowStamp(true);
        //Tell the script what the main skillbook is. 
        mainSkillbook = skillbookContainer.skillbooks[selectedPanelIndex];
        //Tell the skillbar panel what the main skillbook is. 
        //GameManager.instance.skillBarPanel.SetMainSkillbook(mainSkillbook);
    }

    //Show the dialog confirming whether to practice. This is called by the StudyButton
    public void ConfirmPractice()
    {
        //Create the dialog.
        List<string> _dialog = new List<string>();
        _dialog.Add("¼´½«½øÈë´ò×ø×´Ì¬£¬ÐÞÁ¶" + mainSkillbook.item.Name + "Âð£¿");
        DialogContainer dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(_dialog, actor);
        //Show the dialog in dialog panel.
        GameManager.instance.dialogSystem.Initialize(dialog, true);
        //Add the OnYesClicked() function 
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.AddListener(OnYesClicked);
        //Add the OnNoClicked() function to the no button.
        GameManager.instance.dialogSystem.noButton.GetComponent<Button>().onClick.AddListener(OnNoClicked);
    }


    private void OnYesClicked()
    {
        //Set the Skill page inactive. 
        GameManager.instance.player.GetComponent<InventoryController>().HidePanels();

        //Play the sitting down animation. 
        GameManager.instance.player.GetComponent<CharacterController2D>().PracticeSkillbook();
        //Start gaining EXP
        GameManager.instance.player.GetComponent<SkillbookPracticeManager>().PracticingSkillbook(mainSkillbook);
        //Remove the onClick() function for the button. 
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnYesClicked);
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnNoClicked);
        //Set the dialog panel inactive.
        GameManager.instance.dialogSystem.Show(false);
        
    }

    private void OnNoClicked()
    {
        //Set the dialog panel inactive.
        GameManager.instance.dialogSystem.Show(false);
        //Canceling the listener. 
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnYesClicked);
        GameManager.instance.dialogSystem.yesButton.GetComponent<Button>().onClick.RemoveListener(OnNoClicked);
    }

    //The codes below relate to saving progress.

    [Serializable]
    public class SaveSkillbookData
    {
        public int itemID;
        public int currentLevel;
        public float currentEXP;

        public SaveSkillbookData(int id, int status, int clevel, float cEXP)
        {
            itemID = id;
            currentLevel = clevel;
            currentEXP = cEXP;
        }
    }

    [Serializable]
    public class ListToSave
    {
        public List<SaveSkillbookData> skillbookDatas;

        public ListToSave()
        {
            skillbookDatas = new List<SaveSkillbookData>();
        }
    }

    //Saving skillbook list data. This is called by the GameSaveManager script. 
    public void Save()
    {
        ListToSave listToSave = new ListToSave();

        for(int i = 0; i < skillbookContainer.skillbooks.Count; i++)
        {
            Debug.Log(skillbookContainer.skillbooks[i]);
            if(skillbookContainer.skillbooks[i] == null)
            {
                break;
            }
            else
            {
                listToSave.skillbookDatas.Add(new SaveSkillbookData(skillbookContainer.skillbooks[i].item.ID,
                    skillbookContainer.skillbooks[i].currentLevel, skillbookContainer.skillbooks[i].currentLevel,
                    skillbookContainer.skillbooks[i].currentEXP));
            }
        }

        System.IO.File.WriteAllText(Application.persistentDataPath + "/SkillbookListData.json", JsonUtility.ToJson(listToSave));
    }

    //Loading skillbook list data. 
    public void Load()
    {
        string jsonString = System.IO.File.ReadAllText(Application.persistentDataPath + "/SkillbookListData.json");
        ListToSave listToLoad = JsonUtility.FromJson<ListToSave>(jsonString);
        for(int i = 0; i < listToLoad.skillbookDatas.Count; i++)
        {
            if(listToLoad.skillbookDatas[i].itemID == -1)
            {
                skillbookContainer.skillbooks[i].item = null;
                skillbookContainer.skillbooks[i].currentLevel = 0;
                skillbookContainer.skillbooks[i].currentLevel = 0;
                skillbookContainer.skillbooks[i].currentEXP = 0f;
            }
            else
            {
                skillbookContainer.skillbooks[i].item = GameManager.instance.itemDB.items[listToLoad.skillbookDatas[i].itemID];
                skillbookContainer.skillbooks[i].currentLevel = listToLoad.skillbookDatas[i].currentLevel;
                skillbookContainer.skillbooks[i].currentLevel = listToLoad.skillbookDatas[i].currentLevel;
                skillbookContainer.skillbooks[i].currentEXP = listToLoad.skillbookDatas[i].currentEXP;
            }
        }
    }

}
