using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetStatPanel : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Text level;
    [SerializeField] Text health;
    [SerializeField] Text mana;
    [SerializeField] Text PATT;
    [SerializeField] Text MATT;
    [SerializeField] Text EXP;

    public void Set(EnemyDataSheet enemyData) //This is called by the OnePetBlockPanel script, during the OnClick() of an button. 
    {
        name.text = enemyData.Name;
        level.text = enemyData.enemyLevel.ToString();
        health.text = enemyData.HP.ToString();
        mana.text = enemyData.MP.ToString();
        PATT.text = enemyData.PATT.ToString();
        MATT.text = enemyData.MATT.ToString();
        EXP.text = enemyData.EXP.ToString();
    }
}
