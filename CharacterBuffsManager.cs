using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffData
{
    public string buffName;
    public BuffType buffType;
    public float value;
    public BuffData(string name, BuffType type, float f)
    {
        buffName = name;
        buffType = type;
        value = f;
    }
}

public class CharacterBuffsManager : TimeAgent
{
    Character character;
    public List<float> hpRecovery; //A list of numbers contributed from different hp buffs. 
    public List<float> mpRecovery; //A list of numbers contributed from different mp buffs. 
    public CharBuffContainer buffContainer;
    private float update; 

    private void Start()
    {
        character = GameManager.instance.character;
        update = 0.0f;
/*        onTimeTick += Tick;
        Init();*/
    }

    private void Update()
    {
        update += Time.deltaTime;
        if (update > 1.0f)
        {
            update = 0;
            //Debug.Log(update);
            character.currentHealth += NaturalHPRecovery();
            character.currentMana += NaturalMPRecovery();
        }
    }
 /*   public void Tick()
    {
        NaturalHPRecovery();
        NaturalMPRecovery();
    }*/

    //This function will increase player HP after incorporating all buffs. 
    public float NaturalHPRecovery()
    {
        float total = 0f;

        //Hard-add recovery from wood status into the list. 
        WoodRecovery();

        //Calculate total recovery value. 
        for (int i = 0; i < buffContainer.buffDataList.Count; i++)
        {
            if(buffContainer.buffDataList[i].buffType == BuffType.HPRecovery)
            {
                total = total + buffContainer.buffDataList[i].value;
            }
        }
        return total;
    }
    //This function will increase player MP after incorporating all buffs. 
    public float NaturalMPRecovery()
    {
        float total = 0f;

        //Hard-add recovery from water status into the list. 
        WaterRecovery();

        //Calculate total recovery value. 
        for (int i = 0; i < buffContainer.buffDataList.Count; i++)
        {
            if (buffContainer.buffDataList[i].buffType == BuffType.HPRecovery)
            {
                total = total + buffContainer.buffDataList[i].value;
            }
        }
        return total;
    }

    //Specifically calculating the hp recovery contributed by char. wood.
    private void WoodRecovery()
    {

        bool isWoodRegistered = false;
        for (int i = 0; i < buffContainer.buffDataList.Count; i++)
        {
            if (buffContainer.buffDataList[i].buffName == "woodRecovery")
            {
                //Update the value. And let the function know that woodRecovery is already registered. 
                buffContainer.buffDataList[i].value = 0.1f * character.wood;
                isWoodRegistered = true;
                break;
            }
            else
            {
                continue;
            }
        }
        //If the list has been screened and woodRecovery was not found. Register it. 
        if (isWoodRegistered == false)
        {
            buffContainer.buffDataList.Add(new BuffData("woodRecovery", BuffType.HPRecovery, 0.1f * character.wood));
        }
    }

    //Specifically calculating the mp recovery contributed by char. water.
    private void WaterRecovery()
    {
        bool isWaterRegistered = false;
        for (int i = 0; i < buffContainer.buffDataList.Count; i++)
        {
            if (buffContainer.buffDataList[i].buffName == "waterRecovery")
            {
                //Update the value. And let the function know that waterRecovery is already registered. 
                buffContainer.buffDataList[i].value = 0.1f * character.water;
                isWaterRegistered = true;
                break;
            }
            else
            {
                continue;
            }
        }

        //If the list has been screened and waterRecovery was not found. Register it. 
        if (isWaterRegistered == false)
        {
            buffContainer.buffDataList.Add(new BuffData("waterRecovery", BuffType.MPRecovery, 0.1f * character.water));
        }
    }
}
