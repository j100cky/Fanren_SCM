using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarPanel : MonoBehaviour
{
    public Skillbooks mainSkillbook; //Obtain the information of the skillbook to access its skills.
    [SerializeField] List<SkillButton> skillButtons; //The list of hidden skillbuttons. 
    int numberOfButtons; //Need to be defined by accessing to the skillbook's skills variable.
    [SerializeField] MainSkillbookButton mainSkillbookButton; //The gameobject for the main skillbook button.

    [SerializeField] SkillBarController skillBarController; //For using the scrolling feature from the skillbar controller.
    public SkillDatasheet currentSkill; //Will be determined and be read by the SpellsCharacterController script when casting spell.  

    [SerializeField] GameObject showSkillbarButton; //For showing the button that will enable skillbar to show. 

    private void Start()
    {
        //If main skillbook is not set, do not show the skill panel. 
        if(mainSkillbook == null)
        {
            //gameObject.SetActive(false);
            return;
        }
        //If a mainskill is set, update the main skillbook sprite and the skill buttons. 
        else
        {
            SetIndex();
            skillBarController.onChange += Highlight;
            Highlight(0);
            mainSkillbookButton.Set(mainSkillbook);
            SetUpSkillButtons();
        }
    }

    //This will be called by the "Practice" button in the skill panel. 
    public void SetMainSkillbook(Skillbooks skillbook)
    {
        gameObject.SetActive(true);
        mainSkillbook = skillbook;
        mainSkillbookButton.Set(skillbook);
        SetUpSkillButtons();
        //Set the index for each button. This is essential for the whole thing to work. 
        SetIndex();
        //Set up the Scrolling highlight.
        skillBarController.onChange += Highlight;
        Highlight(0);
    }


    //Set up the number of active skill buttons accroding to the number of skills provided by a skillbook.
    public void SetUpSkillButtons()
    {
        //Resetting all the buttons. 
        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].gameObject.SetActive(false);
        }

        //Then make active the number of skills in the skill list of the main skillbook.
        for(int i=0; i< mainSkillbook.levelUpSkill.Count; i++)
        {
            //If the skill is learned, update both the icon and the SkillDatasheet. 
            if(mainSkillbook.currentLevel >= mainSkillbook.levelUpSkill[i].level)
            {
                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].SetSkill(mainSkillbook.levelUpSkill[i].skill);
            }
            //If the skill is unlearned, update the sprite only (with gray), but not update the SkillDatasheet.
            else
            {
                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].SetUnlearnedSkill(mainSkillbook.levelUpSkill[i].skill);
            }


            
        }
    }

    //Give each children button an index. 
    private void SetIndex()
    {
        if(skillButtons == null) { return; }
        for(int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].SetIndex(i);
        }
    }

    //Script for highlighting the current skill.
    int currentSelectedSkillIndex;

    public void Highlight(int id) //This function will be used by the SkillBarController script.
                                  //That script will pass the number based on the mouse wheel to this id variable.
    {
        //Debug.Log("highlight used");
        skillButtons[currentSelectedSkillIndex].Highlight(false); //Get the previous skill inactive.
        currentSelectedSkillIndex = id; //Set the new currentSelectedSkill as the id, which is determined by the mouse wheel
                                        //in the SkillBarController. 
        currentSkill = skillButtons[id].skillDatasheet; //Update the what the current skill is so that the
                                                        //SpellCharacterController can cast the correct spell. 
        skillButtons[currentSelectedSkillIndex].Highlight(true); //Get the new skill highlighted.
    }

    //Called by the hide skillbar button.
    public void HideSkillBar()
    {
        showSkillbarButton.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ShowSkillBar()
    {
        showSkillbarButton.SetActive(true);
        gameObject.SetActive(true);
    }



}
