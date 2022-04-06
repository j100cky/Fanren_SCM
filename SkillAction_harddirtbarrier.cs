using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Action/Harddirt barrier")]
public class SkillAction_harddirtbarrier : ToolAction
{

    //[SerializeField] float manaCost;
    //[SerializeField] GameObject skillPrefab;
    private GameObject player;
    private Character character;


    public override void OnSkillUsed()
    {
        player = GameManager.instance.player;
        character = player.GetComponent<Character>();

        if (character.currentMana <= manaCost) //Do not cast spell if there is not enough mana. 
        {
            Debug.Log("no enough mana");
            //Play some animation like the mana bar shaking//
            return;
        }
        //If the barrier does not exist, intanstiate it, and register it into the buff list.
        GameObject result = character.GetBuffObject(buffName);
        if(result == null)
        {
            character.UseMana(manaCost);
            //Create a new vector3 that is slightly lower in y-axis than player position so that the barrier can cover the feet. 
            Vector3 adjustedPlayerPosition = new Vector3(player.transform.position.x,
                player.transform.position.y - 0.2f, player.transform.position.z);
            //Instantiating the skill prefab which contains the script and animator. 
            GameObject go = Instantiate(skillPrefab, player.transform);
            go.transform.position = adjustedPlayerPosition;
            //Register the gameobject into the Character script as a buff. 
            character.RegisterBuffPairs(buffName, go);
            //Also tell the skill controller what the buff name is. 
            go.GetComponent<HarddirtBarrierSkillScript>().SetBuffName(buffName);
        }
        //If the barrier already exists, renew it. 
        else
        {
            //First remove the old buff. 
            character.RemoveBuffPairs(buffName);
            //Then instantiate the new one.
            character.UseMana(manaCost);
            Vector3 adjustedPlayerPosition = new Vector3(player.transform.position.x,
                player.transform.position.y - 0.2f, player.transform.position.z);
            GameObject go = Instantiate(skillPrefab, player.transform);
            go.transform.position = adjustedPlayerPosition;
            character.RegisterBuffPairs(buffName, go);
        }


    }


}
