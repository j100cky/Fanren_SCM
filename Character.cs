using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public enum CharacterRole
{
    Farmer, 
    Miner, 
    Baker,
    Tamer, 
    Exorcist,
    Trader
}




public class Character : MonoBehaviour, IPersistant
{
    public string Name = "韩立";

    public CharacterRole role;

    public HealthBar healthBar;
    public float maxHealth;
    public float currentHealth;
    public GameObject HPRecoveryEffect; 

    public ManaBar manaBar;
    public float maxMana;
    public float currentMana;
    public GameObject MPRecoveryEffect;

    public float power = 1;
    public float intel = 1;
    public float defense = 1;
    public float mentality = 1;

    //Five elemetns. 
    public float wood;
    public float fire;
    public float ground;
    public float metal;
    public float water;


    public SpriteRenderer spriteRenderer; //To access the sprite's color.//
    Color damageColor1 = new Color(0.5f, 0, 0); //Red//
    Color damageColor2 = new Color(0.5f, 0.5f, 0.5f); //Gray//
    Color originalColor = new Color(1, 1, 1); //White//

    public float useToolRange = 5f; //This is the range within which a tool/weapon can be used.//

    public float maxEXP; //The exp needed to level up.
    public float currentEXP = 0;
    public EXPBar expBar;

    public float currentBattleEXP; //The exp that will be obtained after each enemy killed. 
    public float maxBattleEXP = 100f;

    public float currentForagingEXP;
    public float maxForagingEXP = 100f;

    public float currentCraftEXP;
    public float maxCraftEXP = 100;

    public int playerLevel;
    public int battleLevel;
    public int forageLevel;
    public int craftLevel;

    //[SerializeField] Text levelText;
    //[SerializeField] Image levelImage;

    [SerializeField] int cash = 10000; //The amount of money that the player carries.
    public int GetCashValue()
    {
        return cash; 
    }
    [SerializeField] public int chip = 10000; //The amount of casino chip that the player carries.

/*    [SerializeField] List<GameObject> buffGameObjects; //A list of game objects that carries the buff entity. e.g. shield from skills. 
        public List<GameObject> GetBuffList()
        {
            return buffGameObjects;
        }
        public void RegisterBuff(GameObject go)
        {
            buffGameObjects.Add(go);
        }
        public void RemoveBuff(GameObject go)
        {
            buffGameObjects.Remove(go);
            Destroy(go);
        }*/

    [SerializeField] Dictionary<string, GameObject> buffPairs = new Dictionary<string, GameObject>();
    public GameObject GetBuffObject(string name)
    {
        buffPairs.TryGetValue(name, out GameObject result);
        return result;
    }

    public void RegisterBuffPairs(string name, GameObject go)
    {
        buffPairs.Add(name, go);
        Debug.Log(name + " buff added");
    }
    public void RemoveBuffPairs(string name)
    {
        buffPairs.TryGetValue(name, out GameObject result);
        result.GetComponent<SkillController>().FinishAnimation(); //This function includes the destroy of the game object. 
        buffPairs.Remove(name);
        Debug.Log(name+" buff removed");
    }

    [SerializeField] BoundaryBreakButtonScript boundaryBreakButton; 

    [SerializeField] public List<Quest> activeQuests; //To store the quests that the player is having right now. 
    //[SerializeField] public List<levelPair> levelPairList;




    void Start()
    {

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        manaBar.SetMaxMana(maxMana);
        currentMana = maxMana;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //expBar.SetMaxEXP(maxEXP, currentEXP);
        //levelText.text = playerLevel.ToString();
        RandomizeFiveElements();

    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Restrain the value of current health to between 0 and maxHealth.
        currentMana = Mathf.Clamp(currentMana, 0, maxMana); //Restrain the value of current mana to between 0 and maxHealth.

        if (currentEXP >= maxEXP)
        {
            LevelUp();
        }
        if (currentBattleEXP >= maxBattleEXP)
        {
            BattleLevelUp();
        }
        if (currentForagingEXP >= maxForagingEXP)
        {
            ForagingLevelUp();
        }
        if(currentCraftEXP >= maxCraftEXP)
        {
            CraftLevelUp();
        }


    }

    //This randomize the five elements of the player. This should be run only once and set the player's five element for life. 
    private void RandomizeFiveElements()
    {
        //First, zero the stats. 
        wood = 0f;
        fire = 0f;
        ground = 0f;
        metal = 0f;
        water = 0f;
        
        List<float> elements = new List<float> { wood, fire, ground, metal, water };

        float totalSum = 0f;

        while (totalSum < 0.999f)
        {
            for (int i = 0; i < 5; i++)
            {
                float a = UnityEngine.Random.Range(0f, (1f - totalSum) / 5);
                elements[i] = elements[i] + a;
                totalSum = totalSum + a;
            }
            //Update the stats. 
            wood = Mathf.Round(elements[0]*100);
            fire = Mathf.Round(elements[1]*100);
            ground = Mathf.Round(elements[2]*100);
            metal = Mathf.Round(elements[3]*100);
            water = Mathf.Round(elements[4]*100);
        }

    }


    //HP functions here

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void IncreaseMaxHealth(float value) //This function can be used from certain items, skills, or consumables.//
    {
        maxHealth += Mathf.Ceil(value);

    }

    public void RestoreHealth(float value)
    {
        if(value == 0)
        { 
            return; 
        }

        //Don't overflow the hp.
        if(currentHealth + value >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += Mathf.Ceil(value);
        }

        MessageManager.instance.CallMsgPanel("+" + value.ToString(), transform, Color.green);
        healthBar.SetHealth(Mathf.Ceil(currentHealth));
        //Play animation.//
        GameObject go = Instantiate(HPRecoveryEffect, gameObject.transform.position, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        //First, check if the buff list contain a barrier buff.
        if(buffPairs != null)
        {
            foreach(var buff in buffPairs)
            {
                SkillController skillCtrl = buff.Value.GetComponent<SkillController>();
                if(skillCtrl != null)
                {
                    //Check buff type by getting the BuffType enum from the SkillController script. 
                    if (skillCtrl.GetBuffType() == BuffType.Barrier)
                    {
                        //If the buff is a Barrier type, the barrier will take damage. And the TakeDamage() function is ended. player unharmed.
                        skillCtrl.BarrierTakeDamage(damage);
                        return;
                    }
                    else
                    {
                        //If the current buff is not a barrier, move to the next buff. 
                        continue;
                    }
                }
            }

        }
        //Do the following when there is no buff or none of the buffs is a barrier type.
        float finalDamage = damage - defense;
        finalDamage = Mathf.Clamp(finalDamage, 0f, damage - defense);
        currentHealth -= finalDamage;
        MessageManager.instance.CallMsgPanel("-" + finalDamage.ToString(), transform, Color.red);
        healthBar.SetHealth(Mathf.Ceil(currentHealth));
        StartCoroutine("ColorChange");
        CallMOAction();
    }


    private void CallMOAction()
    {
        //We need to loop through all the prefabs that are instantiated and play their animations. 
        foreach(CarryingEffects effect in GetComponentsInChildren<CarryingEffects>()) 
        {
                effect.OnActive();
        }
    }


    IEnumerator ColorChange()
    {
        spriteRenderer.color = damageColor1;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = damageColor2;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }


    //MP Functions here. 
    public float GetMana()
    {
        return currentMana;
    }

    public void UseMana(float manaCost)
    {
        currentMana -= Mathf.Ceil(manaCost);
        manaBar.SetMana(currentMana);
    }

    public void RestoreMana(float value)
    {
        if (value == 0)
        {
            return;
        }
        //Don't overflow the mana.
        if(currentMana + value >= maxMana)
        {
            currentMana = maxMana;
        }
        else 
        { 
            currentMana += Mathf.Ceil(value); 
        }
        MessageManager.instance.CallMsgPanel("+" + value.ToString(), transform, Color.blue);
        manaBar.SetMana(currentMana);
        GameObject go = Instantiate(MPRecoveryEffect, gameObject.transform.position, Quaternion.identity);
    }

    public void ChangeMaxMana(float value)
    {
        maxMana += Mathf.Ceil(value);
        manaBar.SetMaxMana(maxMana);

    }

    //Five element functions here
    public void ChangeWoodValue(float value)
    {
        wood = wood + value;
    }

    public void ChangeFireValue(float value)
    {
        fire = fire + value;
    }

    public void ChangeGroundValue(float value)
    {
        ground = ground + value;
    }

    public void ChangeMetalValue(float value)
    {
        metal = metal + value;
    }

    public void ChangeWaterValue(float value)
    {
        water = water + value;
    }

    public void GainBattleEXP(float value)
    {
        currentBattleEXP += value;
        MessageManager.instance.CallMsgPanel("Battle EXP+" + value.ToString(), transform, Color.magenta);
    }

    public void GainForagingEXP(float value)
    {
        currentForagingEXP += value;
        MessageManager.instance.CallMsgPanel("Foraging EXP+" + value.ToString(), transform, Color.magenta);
    }

    public void GainCraftEXP(float value)
    {
        currentCraftEXP += value;
        MessageManager.instance.CallMsgPanel("Crafting EXP+" + value.ToString(), transform, Color.magenta);
    }

    public void LevelUp()
    {
        MessageManager.instance.CallMsgPanel("Level Up!", transform, Color.magenta);
        playerLevel += 1;
        //levelText.text = playerLevel.ToString();
        //currentEXP = currentEXP - maxEXP; //To obtain the residual EXP.//
        //maxEXP = maxEXP * 2;
        //expBar.SetMaxEXP(maxEXP, currentEXP);
        //maxHealth = Mathf.Ceil(maxHealth + maxHealth * 0.5f);
        //healthBar.SetMaxHealth(maxHealth);
        //currentHealth = maxHealth; //Give the player the full status when level up.//
        //maxMana = Mathf.Ceil(maxMana * 2f);
        //manaBar.SetMaxMana(maxMana);
        //currentMana = maxMana; //Give the player the full status when level up.//
        useToolRange += 0.5f; //Increase the attack range.//
        boundaryBreakButton.ShowButton(); //Determine whether to show the "Break Boundary" button or not. 
        //Play level up animation
        gameObject.GetComponent<Animator>().SetTrigger("LevelUp");
    }

    //This is called when boundary break is successful. 
    public void StageUp()
    {
        MessageManager.instance.CallMsgPanel("突破了!", transform, Color.magenta);
        playerLevel += 1;
        maxHealth = Mathf.Ceil(maxHealth + maxHealth * 0.5f);
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth; //Give the player the full status when level up.//
        maxMana = Mathf.Ceil(maxMana * 2f);
        manaBar.SetMaxMana(maxMana);
        currentMana = maxMana; //Give the player the full status when level up.//
        useToolRange += 0.5f; //Increase the attack range.//
    }

    public void BattleLevelUp()
    {
        if (battleLevel >= 4) { return; } //Set the maximum level to 4.
        MessageManager.instance.CallMsgPanel("Battle Level Up!", transform, Color.magenta);        
        battleLevel += 1;
        currentBattleEXP = currentBattleEXP - maxBattleEXP; //Clear the exp counter. 
        maxBattleEXP = maxBattleEXP * 2;
        //other benefits of increasing battle level to be added.
    }

    public void ForagingLevelUp()
    {
        if (forageLevel >= 4) { return; } //Set the maximum level to 4.
        MessageManager.instance.CallMsgPanel("Foraging Level Up!", transform, Color.magenta);
        forageLevel += 1;
        currentForagingEXP = currentForagingEXP - maxForagingEXP; //Clear the exp counter. 
        maxForagingEXP = maxForagingEXP * 2;
        //other benefits of increasing foraging level to be added.
    }

    public void CraftLevelUp()
    {
        if (craftLevel >= 4) { return; }
        MessageManager.instance.CallMsgPanel("Crafting Level Up!", transform, Color.magenta);
        craftLevel += 1;
        currentCraftEXP = currentCraftEXP - maxCraftEXP;
        maxCraftEXP = maxCraftEXP * 2;
        //Add other benefits to the crafting process.
    }

    public void ChangePower(float value)//This function can be used from certain items, skills, or consumables.//
    {
        power = power + value;
    }

    public void ChangeIntel(float value)//This function can be used from certain items, skills, or consumables.//
    {
        intel = intel + value;
    }

    public void ChangeDefense(float value)//This function can be used from certain items, skills, or consumables.//
    {
        defense = defense + value;
    }

    public void ChangeMentality(float value)//This function can be used from certain items, skills, or consumables.//
    {
        mentality = mentality + value;
    }

    public void AddMoney(int amount)//This function can be used from certain items, skills, or consumables.//
    {
    	cash = cash + amount;
    }

    public void LoseMoney(int amount)
    {
    	cash = cash - amount;
    }

    public void SetCash(int value)
    {
        cash = value;
    }

    public void AddChip(int amount)
    {
        chip = chip + amount;
    }

    public void LoseChip(int amount)
    {
        chip = chip - amount;
    }

    //For other events to complete the quest in the activeQuest list. 
    public void CompleteQuest(Quest quest)
    {
        if(quest.questGoal.goalType == GoalType.Collect) //If the quest requires the collection of something, remove items from bag.
        {
            GameManager.instance.inventoryContainer.Remove(quest.questGoal.targetItem, quest.questGoal.targetAmount);
            quest.isQuestActive = false; //Reset the quest's isQuestActive. 
            activeQuests.Remove(quest); //Remove the quest from character's list. 
        }
        else //If teh quest is not collect, simply remove the quest. 
        {
            activeQuests.Remove(quest);
        }

    }

    //Called by other scripts to reset char status (e.g. new game).
    public void ResetCharStatus()
    {
        RandomizeFiveElements();
        maxHealth = 10f;
        currentHealth = 10f;
        maxMana = 5f;
        currentMana = 5f;
        activeQuests = new List<Quest>();
        cash = 100;
        power = 1f;
        intel = 1f;
        defense = 1f;
        chip = 0;
        playerLevel = 0;
    }



    //Below are the codes for saving character status progress. 
    [Serializable]
    public class SaveActiveQuestData
    {
        public int questID;
        public int currentAmount;

        public SaveActiveQuestData(int questID, int currentAmount)
        {
            this.questID = questID;
            this.currentAmount = currentAmount;
        }

    }

/*    [Serializable]
    public class ListOfQuestData
    {
        public List<SaveActiveQuestData> questDatas;
        public ListOfQuestData()
        {
            questDatas = new List<SaveActiveQuestData>();
        }
    }*/

    [Serializable]
    public class SaveCharacterData
    {
        public float savedMaxHealth;
        public float savedCurrentHealth;
        public float savedMaxMana;
        public float savedCurrentMana;
        public float savedPower;
        public float savedIntel;
        public float savedDefense;
        public float savedMentality;
        public float savedMaxExp;
        public float savedCurrentExp;
        public int savedCash;
        public int savedPlayerLevel;

        public float savedWood;
        public float savedFire;
        public float savedGround;
        public float savedMetal;
        public float savedWater;

        public List<SaveActiveQuestData> questDatas;


        //public float[] savedCharPos; //Position will be covered by GameSceneManager

        public SaveCharacterData(float mH, float cH, float mM, float cM, float pow,
            float intl, float def, float ment, float mE, float cE, int cas, int pyLevel, 
            float wood, float fire, float ground, float metal, float water, List<Quest> activeQuests)
        {
            savedMaxHealth = mH;
            savedCurrentHealth = cH;
            savedMaxMana = mM;
            savedCurrentMana = cM;
            savedPower = pow;
            savedIntel = intl;
            savedDefense = def;
            savedMentality = ment;
            savedMaxExp = mE;
            savedCurrentExp = cE;
            savedCash = cas;
            savedPlayerLevel = pyLevel;
            savedWood = wood;
            savedFire = fire;
            savedGround = ground;
            savedMetal = metal;
            savedWater = water;

            questDatas = new List<SaveActiveQuestData>();
            for (int i = 0; i<activeQuests.Count; i++)
            {
                questDatas.Add(new SaveActiveQuestData(activeQuests[i].questID, activeQuests[i].questGoal.currentAmount));
            }

        }

    }

    public string Save()
    {
        SaveCharacterData charSave = new SaveCharacterData(maxHealth, currentHealth, maxMana, currentMana, 
            power, intel, defense, mentality, maxEXP, currentEXP, cash, playerLevel, wood, fire, ground, metal, water, activeQuests);

        return JsonUtility.ToJson(charSave);

    }



    public void Load(string jsonString)
    {
        if (jsonString == "" || jsonString == "{}" || jsonString == null) { return; }
        SaveCharacterData charLoad = JsonUtility.FromJson<SaveCharacterData>(jsonString);
        maxHealth = charLoad.savedMaxHealth;
        currentHealth = charLoad.savedCurrentHealth;
        maxMana = charLoad.savedMaxMana;
        currentMana = charLoad.savedCurrentMana;
        power = charLoad.savedPower;
        intel = charLoad.savedIntel;
        defense = charLoad.savedDefense;
        mentality = charLoad.savedMentality;
        maxEXP = charLoad.savedMaxExp;
        currentEXP = charLoad.savedCurrentExp;
        cash = charLoad.savedCash;
        playerLevel = charLoad.savedPlayerLevel;
        wood = charLoad.savedWood;
        fire = charLoad.savedFire;
        ground = charLoad.savedGround;
        metal = charLoad.savedMetal;
        water = charLoad.savedWater;

        activeQuests = new List<Quest>();
        for(int i = 0; i < charLoad.questDatas.Count; i++)
        {
            activeQuests.Add(GameManager.instance.questDB.quests[charLoad.questDatas[i].questID]);
            activeQuests[i].questGoal.currentAmount = charLoad.questDatas[i].currentAmount;
        }
    }

    [SerializeField] string charStatusJSONString;

    public void Saving()
    {
        charStatusJSONString = Save();
        System.IO.File.WriteAllText(Application.persistentDataPath + "/CharacterStatus.json", charStatusJSONString);
    }

    public void Loading()
    {
        charStatusJSONString = System.IO.File.ReadAllText(Application.persistentDataPath + "/CharacterStatus.json");
        Debug.Log("data loaded ");
        Load(charStatusJSONString);
    }

}
