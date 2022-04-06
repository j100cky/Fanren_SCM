using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillbookDetailController : MonoBehaviour
{
    [SerializeField] Image detailedImage;
    [SerializeField] Text title;
    [SerializeField] Text skillbookQuality; 
    [SerializeField] Text level;
    [SerializeField] Text hpBenefit;
    [SerializeField] Text mpBenefit;
    [SerializeField] Text defenseBenefit;
    [SerializeField] Text mentalityBenefit;
    [SerializeField] Text woodBenefit;
    [SerializeField] Text fireBenefit;
    [SerializeField] Text groundBenefit;
    [SerializeField] Text metalBenefit;
    [SerializeField] Text waterBenefit;
    [SerializeField] NumberWordPairs numWordPairs;
    [SerializeField] List<SkillButton> skillButtons;
    [SerializeField] ProgressBar expBar;
    Skillbooks mainSkillbook;

    private void Awake()
    {
        numWordPairs.MakeDictionary();
    }

    private void Update()
    {
        SetVariables(mainSkillbook); //This makes sure that the values are up to date.
    }

    public void SetVariables(Skillbooks skillbook)
    {
        if(skillbook == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        mainSkillbook = skillbook; //Give reference for the class so that the Update() function can keep the exp bar updated. 

        detailedImage.sprite = skillbook.item.icon;
        title.text = skillbook.item.Name;

        //Determining the quality text of the skillbook. 
        string qualityDescription = ""; 
        SkillbookQualityType qualityType = skillbook.quality;
        if(qualityType == SkillbookQualityType.elementary)
        {
            qualityDescription = "初级";
        }
        else if (qualityType == SkillbookQualityType.junior)
        {
            qualityDescription = "中级";
        }
        else if(qualityType == SkillbookQualityType.senior)
        {
            qualityDescription = "高级";
        }

        string elementDescription = "";
        SkillbookElementType elementType = skillbook.elementType;
        if(elementType == SkillbookElementType.wood)
        {
            elementDescription = "木";
        }
        else if(elementType == SkillbookElementType.fire)
        {
            elementDescription = "火";
        }
        else if (elementType == SkillbookElementType.ground)
        {
            elementDescription = "土";
        }
        else if (elementType == SkillbookElementType.metal)
        {
            elementDescription = "金";
        }
        else if (elementType == SkillbookElementType.water)
        {
            elementDescription = "水";
        }

        string combinedDescription = qualityDescription + elementDescription + "属性功法";
        skillbookQuality.text = combinedDescription;

        //Determining the skillbook's levelup benefits.

        hpBenefit.text = "+" + skillbook.levelUpBenefits.health.ToString();
        mpBenefit.text = "+" + skillbook.levelUpBenefits.mana.ToString();
        defenseBenefit.text = "+" + skillbook.levelUpBenefits.defense.ToString();
        mentalityBenefit.text = "+" + skillbook.levelUpBenefits.mentality.ToString();
        woodBenefit.text = "+" + skillbook.levelUpBenefits.wood.ToString();
        fireBenefit.text = "+" + skillbook.levelUpBenefits.fire.ToString();
        groundBenefit.text = "+" + skillbook.levelUpBenefits.ground.ToString();
        metalBenefit.text = "+" + skillbook.levelUpBenefits.metal.ToString();
        waterBenefit.text = "+" + skillbook.levelUpBenefits.water.ToString();

        //Determining the skillbook's current level and current exp bar. 

        string temp = ("第" + numWordPairs.GetString(skillbook.currentLevel) + "层");
        level.text = temp;
        expBar.SetValues(skillbook.levelUpEXPs[skillbook.currentLevel], skillbook.currentEXP);

        //Determinning the skillbook's available skills.

        for(int i = 0; i< skillButtons.Count; i++)
        {
            //Resetting all the skillbuttons.
            skillButtons[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < skillbook.levelUpSkill.Count; i++)
        {
            skillButtons[i].gameObject.SetActive(true);
            skillButtons[i].ShowUnlockLevel(skillbook.levelUpSkill[i].level);
            //Determine whether to show active skill or unlearned skill sprit.
            if(skillbook.levelUpSkill[i].level > skillbook.currentLevel)
            {
                skillButtons[i].SetUnlearnedSkill(skillbook.levelUpSkill[i].skill);
            }
            else
            {
                skillButtons[i].SetSkill(skillbook.levelUpSkill[i].skill);
            }
           
        }

    }

}
