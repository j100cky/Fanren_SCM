using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDialoguePanelController : MonoBehaviour
{
    [SerializeField] GameObject background;
    [SerializeField] public GameObject dialogue;
    [SerializeField] public float timePerLetter = 0.05f;
    [SerializeField] float visibleTextPercent;
    [Range(0f, 1f)]
    float totalTimeToType, currentTime, expTimer;
    string lineToShow = "";
    string readingProgressionString = "."; //This is used to show the passage of time during studying. 
    public bool isStudyInProgress; //Used as the condition to tell what clicking the left button does. 

    [SerializeField] GameObject yesOrNoPanel; //Referencing the YesOrNoDialogPanel.
    public bool pauseTypeOutText, pauseGiveEXP; //Used by text typing and EXP giving. They have to separate because we don't
                                                //want EXP gaining during the first dialog. 

    public float expPerSecond; //The value of this variable will be given by the SkillbookReading ToolAction.

    private void Start()
    {
        isStudyInProgress = false; //Set this to default at the beginning so left-clicking won't start anything. 
    }

    private void Update()
    {
        if(dialogue.activeInHierarchy != true) { return; } //If the panel is not active, nothing should be proceeded. 
        TypeOutText();
        //GiveEXP(expPerSecond); //This function calls the Character's GainEXP() function using the value given by the
                               //SkillbookReading script. 
        if(Input.GetMouseButtonDown(0))
        {        
            //Debug.Log(isStudyInProgress);
            if(visibleTextPercent <1f) //If the previous line is not finish...
            {
                if(isStudyInProgress == false) //...and if the panel is not showing the ellipsis...
                {
                    PushText(); //... then left-clicking means pushing the text to 100%. 
                }
                else //...but if the panel is showing the ellipsis (which mean studying)...
                {
                    //show a new panel asking whether the player wants to quit study.
                    yesOrNoPanel.SetActive(true);
                    pauseTypeOutText = true; //Set this to true so that the TypeOutText() will be skipped. 
                    pauseGiveEXP = true; //Set this to true so that the GiveEXP() will be skipped. 
                    //two buttons. If yes, quit study, if no, continue. 
                    //Debug.Log("Ask whether the player want to quit study");
                }

            }
            else //If the previous line is finished...
            {
                ShowStudyProgress(); //Move to the second dialog, which is the indication of studying. 
                isStudyInProgress = true; //Set this to true so that the next left click will not call PushText().
                //Make character to add experience. 
            }
            
        }

    }

    private void TypeOutText() //This function will calculate the percent of dialog to be shown based on how long time has passed. 
    {
        if (visibleTextPercent >= 1f) { return; }
        if(pauseTypeOutText == true) { return; }
        currentTime += Time.deltaTime;
        //Debug.Log(currentTime);
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f);
        UpdateText();
    }

    /*private void GiveEXP(float exp) //This function includes the condition that needs to be determined before calling
                                    //Character.GainEXP() and the calling of Character.GainEXP().
    {
        //if (visibleTextPercent >= 1f) { return; }
        if (pauseGiveEXP == true) { return; }
        if (isStudyInProgress == false) { return; }
        expTimer += Time.deltaTime;
        //Debug.Log(expTimer);
        while (expTimer > 1f)
        {
            GameManager.instance.character.GainEXP(exp);
            expTimer -= 1;
            if (Input.GetKeyDown("space"))
            {
                break;
            }
        }
    }*/

    void UpdateText() //This function will show the actual fraction of the dialog according to the percentage calculated. 
    {
        int letterCount = (int)(lineToShow.Length * visibleTextPercent);
        dialogue.GetComponent<Text>().text = lineToShow.Substring(0, letterCount);
    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

/*        if (currentTextLine >= currentDialog.line.Count)
        {
            Conclude();
        }
        else
        {
            CycleLine();
        }*/
    }

    public void Show(string itemDialog) //Will set this panel visible, set the percentage to 0, and time to 0. 
    {
        lineToShow = itemDialog; //Load in the text that the panel will be showing. 
        GamePauseController.instance.isPaused = true;
        pauseTypeOutText = false; //Reset the pauseTypeOutText so the new dialog can proceed. 
        timePerLetter = 0.05f; //Reset the timePerLetter everytime because the second dialog has a different timePerLetter value. 
        totalTimeToType = timePerLetter * lineToShow.Length; //Set the total time to type so that the visibleTextPercent can be calculated. 
        currentTime = 0f;
        visibleTextPercent = 0f;
        background.SetActive(true); //Set the panel background active.
        dialogue.SetActive(true); //Set the texts active. 

        //dialogue.GetComponent<Text>().text = itemDialog; 
        //Set the text to the itemDialog which is passed specifically by each SkillbookReading ToolAction.
    }

    public void ShowStudyProgress()
    {
        for(int i = 0; i < 100; i++)
        {
            readingProgressionString += ".";
        }
        lineToShow = readingProgressionString; 
        timePerLetter = 1f;
        totalTimeToType = timePerLetter * lineToShow.Length;
        currentTime = 0f;
        visibleTextPercent = 0f;
        pauseGiveEXP = false; //Set this to false so that the GiveEXP() function will can run. 
        //isStudyInProgress = true;
        
    }

    public void Hide() //This function will hide the game objects when the dialog is done and called when the left mouse button is clicked. 
    {
        GamePauseController.instance.isPaused = false;
        background.SetActive(false);
        dialogue.SetActive(false);
    }
}
