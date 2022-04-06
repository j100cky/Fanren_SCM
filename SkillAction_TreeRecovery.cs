using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Data/Skill Action/Tree Recovery")]
public class SkillAction_TreeRecovery : ToolAction
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
        /*        if (GamePauseController.instance.GetCoolDown() == true)
                //Do not cast spell if it is in cooldown. Cooldown is set to false in the last frame of animation. 
                {
                    Debug.Log("Skill in cooldown");
                    return;
                }*/

        //If this buff does not exist in the character bufflist, instantiate the buff, and register the buff into the bufflist. 
        GameObject result = character.GetBuffObject(buffName);
        //Debug.Log(result);
        if (result == null)
        {
            character.UseMana(manaCost);
            //Create a new vector3 that is slightly lower in y-axis than player position so that the barrier can cover the feet. 
            Vector3 adjustedPlayerPosition = new Vector3(player.transform.position.x,
                player.transform.position.y - 0.2f, player.transform.position.z);

            //Instantiate the prefab and register the buff to the character buff list. 
            GameObject go = Instantiate(skillPrefab, player.transform);
            go.transform.position = adjustedPlayerPosition;
            character.RegisterBuffPairs(buffName, go);
        }
        //If this buff already exist in the bufflist, remove it from the list, and stops the effect. 
        else
        {
            result.GetComponent<TreeRecoverySkillScript>().RemoveRecovery();
            character.RemoveBuffPairs(buffName);
        }
    }
}
