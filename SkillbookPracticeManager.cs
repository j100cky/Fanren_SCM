using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillbookPracticeManager : MonoBehaviour
{
    [SerializeField] float tick; //For how often the player gets EXP.
    //[SerializeField] public ProgressBar mainSkillbookProgressBar; //To update the EXP progress bar on the main skillbook.
    private IEnumerator coroutine; //This is needed to temporarily store the coroutine otherwise it can't be stopped.
    [SerializeField] Actor actor; //For the dialog.
    [SerializeField] Skillbooks mainSkillbook; //For other script's reference. 

    public Skillbooks GetMainSkillbook()
    {
        return mainSkillbook;
    }

    public void PracticingSkillbook(Skillbooks mainSkillbook)
    {
        coroutine = Practice(mainSkillbook);
        StartCoroutine(coroutine);
    }

    public void StopPracticing()
    {
        StopCoroutine(coroutine);
    }

    private IEnumerator Practice(Skillbooks importedSkillbook)
    {

        mainSkillbook = importedSkillbook;
        float maxEXP = mainSkillbook.levelUpEXPs[mainSkillbook.currentLevel];
        //Set the progress bar. 
        //mainSkillbookProgressBar.SetValues(maxEXP, mainSkillbook.currentEXP);
        while (mainSkillbook.currentLevel < mainSkillbook.levelUpEXPs.Count)
        {
            //Don't practice if the game is paused. 
            if (GamePauseController.instance.GetPause() == true)
            {
                Debug.Log("no practice because game is paused");
                yield return null;
            }
            else
            {
                //If the next EXP added will make the total greater than the level up EXP of the skillbook, level up.
                if (mainSkillbook.currentEXP + mainSkillbook.perTickEXP >= maxEXP)
                {
                    //Skillbook's item status level up
                    mainSkillbook.currentLevel++;
                    //Set currentEXP to 0.
                    mainSkillbook.currentEXP = 0;
                    //Update maxEXP to the next level.
                    maxEXP = mainSkillbook.levelUpEXPs[mainSkillbook.currentLevel];
                    //Update the EXP progress bar.
                    //mainSkillbookProgressBar.SetMaxValue(maxEXP);
                    //Add the status to characters. 
                    mainSkillbook.AddStatus();
                    //Refresh the SkillBar's skill buttons in case a new skill is learned. 
                    //GameManager.instance.skillBarPanel.SetUpSkillButtons();
                    //If the player is practicing a skillbook that has a higher level than the player, set the player's level to item status. 
                    if (mainSkillbook.currentLevel >= GameManager.instance.player.GetComponent<Character>().playerLevel)
                    {
                        //Get out of practicing mode.
                        GameManager.instance.player.GetComponent<Animator>().SetBool("isPracticing", false);
                        yield return new WaitForSeconds(2.5f);
                        //Level up the playerLevel. 
                        GameManager.instance.player.GetComponent<Character>().LevelUp();
                        yield return new WaitForSeconds(1f);
                        LevelUpMessage(mainSkillbook);
                        GameManager.instance.player.GetComponent<CharacterController2D>().StopPracticingSkills();
                        break;
                    }

                }
                //If not, just add the EXP.
                else
                {
                    //Add EXP
                    CalculateEXPGain();
                    /*                Debug.Log("exp" + mainSkillbook.perTickEXP);
                                    Debug.Log("current exp is" + mainSkillbook.currentEXP;);*/
                    //Update the EXP progress bar. 
                    //mainSkillbookProgressBar.SetCurrentValue(mainSkillbook.currentEXP);
                }
                yield return new WaitForSeconds(tick);
            }
        }

            
    }

    //Calculate the true EXP gain after take into account of the character's element stats. 
    public void CalculateEXPGain()
    {
        if(mainSkillbook.elementType == SkillbookElementType.wood)
        {
            mainSkillbook.currentEXP = mainSkillbook.currentEXP + mainSkillbook.perTickEXP + 
                mainSkillbook.perTickEXP*(GameManager.instance.character.wood*2f)/100f;
            Debug.Log("The skillbook is " + mainSkillbook.elementType.ToString() + ". So each tick exp is added by " +
                (mainSkillbook.perTickEXP * (GameManager.instance.character.wood * 2f) / 100f));
        }
        else if(mainSkillbook.elementType == SkillbookElementType.fire)
        {
            mainSkillbook.currentEXP = mainSkillbook.currentEXP + mainSkillbook.perTickEXP +
                mainSkillbook.perTickEXP * (GameManager.instance.character.fire * 3f) / 100f;
        }
        else if(mainSkillbook.elementType == SkillbookElementType.ground)
        {
            mainSkillbook.currentEXP = mainSkillbook.currentEXP + mainSkillbook.perTickEXP +
                mainSkillbook.perTickEXP * (GameManager.instance.character.ground * 2f) / 100f;
        }
        else if(mainSkillbook.elementType == SkillbookElementType.metal)
        {
            mainSkillbook.currentEXP = mainSkillbook.currentEXP + mainSkillbook.perTickEXP +
                mainSkillbook.perTickEXP * (GameManager.instance.character.metal * 1.5f) / 100f;
        }
        else if(mainSkillbook.elementType == SkillbookElementType.water)
        {
            mainSkillbook.currentEXP = mainSkillbook.currentEXP + mainSkillbook.perTickEXP +
                mainSkillbook.perTickEXP * (GameManager.instance.character.water * 2f) / 100f;
        }
    }

    public void LevelUpMessage(Skillbooks skillbook)
    {
        //Play some animation. 
        List<string> _dialog = new List<string>();
        _dialog.Add("经过苦修,终于悟透了 " + skillbook.item.Name + " 之第" + skillbook.currentLevel + "层");
        _dialog.Add("踏入了 " + 
            GameManager.instance.statusPanel.GetComponent<StatusPanelController>().levelPairList[skillbook.currentLevel].levelName + 
            " 境界");
        DialogContainer dialog = (DialogContainer)ScriptableObject.CreateInstance(typeof(DialogContainer));
        dialog.MakeDialogContainer(_dialog, actor);
        //Show the dialog in dialog panel without yes/no button.
        GameManager.instance.dialogSystem.Initialize(dialog, false);
    }
}
