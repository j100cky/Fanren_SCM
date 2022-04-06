using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelUpBenefit
{
    public float health;
    public float mana;
    //public float power;
    //public float intel;
    public float defense;
    public float mentality;
    public float wood;
    public float fire;
    public float ground;
    public float metal;
    public float water;

}

//This class determines what skill will be learned at which level of the skillbook.
[Serializable]
public class LevelUpSkills
{
    public int level;
    public SkillDatasheet skill;
}

public enum SkillbookElementType
{
    wood, 
    fire, 
    ground, 
    metal,
    water
}

public enum SkillbookQualityType
{
    elementary,
    junior,
    senior
}

//Skillbooks are like items, but with a few more variables. 
[CreateAssetMenu(menuName = "Data/Skillbook")]
public class Skillbooks : ScriptableObject
{
    public Item item;
    public SkillbookElementType elementType; //This is the element type of the skillbook - 木火土金水
    public SkillbookQualityType quality; //This is the quality of the skillbook - 初级，中级，高级
    //public int itemStatus; //What is this?
    public int currentLevel; //Current level of the skillbook. 
    public int maxLevel; //the maximum number of levels provided by this book. 
    public float currentEXP; //The current exp that the player has on this skillbook.
    public List<float> levelUpEXPs; //The amount of exp needed to level up for each level. 
    public LevelUpBenefit levelUpBenefits; //The attributes that will be granted at each level up. 
    public float perTickEXP; //The amount of exp gain for each tick (usually 1 second). 
    public List<LevelUpSkills> levelUpSkill;//A list of skills that can be learned from this skillbook at different levels.

    public void AddStatus()
    {
        Character playerStatus = GameManager.instance.player.GetComponent<Character>();
        //int lvl = itemStatus;
        playerStatus.IncreaseMaxHealth(levelUpBenefits.health);
        playerStatus.ChangeMaxMana(levelUpBenefits.mana);
        //playerStatus.ChangePower(levelUpBenefits.power);
       //playerStatus.ChangeIntel(levelUpBenefits.intel);
        playerStatus.ChangeDefense(levelUpBenefits.defense);
        playerStatus.ChangeMentality(levelUpBenefits.mentality);
        playerStatus.ChangeWoodValue(levelUpBenefits.wood);
        playerStatus.ChangeFireValue(levelUpBenefits.fire);
        playerStatus.ChangeGroundValue(levelUpBenefits.ground);
        playerStatus.ChangeMetalValue(levelUpBenefits.metal);
        playerStatus.ChangeWaterValue(levelUpBenefits.water);
    }

    //Called by events other than SkillbookPracticing. (such as killing enemies)
    public void ChangeEXP(float value)
    {
        if (currentEXP + value >= levelUpEXPs[currentLevel])
        {
            LevelUp();
        }
        else
        {
            currentEXP = currentEXP + value;
            //Set the EXP in the progress bar right. 
/*            GameManager.instance.player.GetComponent<SkillbookPracticeManager>().mainSkillbookProgressBar.SetValues(levelUpEXPs[currentLevel],
                currentEXP);*/
        }

    }

    public void LevelUp()
    {
        //Skillbook's item status level up
        currentLevel++;
            //Set currentEXP to 0
            currentEXP = 0;
            //give player the status gain. 
            AddStatus();
            //Play the level up animation.
            GameManager.instance.player.GetComponent<Character>().LevelUp();
            //Refresh the SkillBar's skill buttons in case a new skill is learned. 
            GameManager.instance.skillBarPanel.SetUpSkillButtons();
            //Set the EXP in the progress bar right. 
/*            GameManager.instance.player.GetComponent<SkillbookPracticeManager>().mainSkillbookProgressBar.SetValues(levelUpEXPs[currentLevel], 
                currentEXP);*/
    }

}
