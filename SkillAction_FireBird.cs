using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Action/Fire Bird")]
public class SkillActionScript_FireBird : ToolAction
{
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

        else
        {
            character.UseMana(manaCost);
            //Create a new vector3 that is slightly lower in y-axis than player position so that the barrier can cover the feet. 
            Vector3 adjustedPlayerPosition = new Vector3(player.transform.position.x,
                player.transform.position.y - 0.2f, player.transform.position.z);
            //Instantiating the skill prefab which contains the script and animator. 
            GameObject go = Instantiate(skillPrefab, player.transform);
            go.transform.position = adjustedPlayerPosition;
        }
    }
}
