using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Action/Fireball multiple")]
public class SkillAction_FireballMulti : ToolAction
{
    float playerAttackRange; //The range within which the player can cast spells. 
    //[SerializeField] float manaCost;

    //[SerializeField] GameObject skillPrefab; //Prefab for the animator. 


    public override void OnSkillUsed()
    {
        Character character = GameManager.instance.character;

        if (character.currentMana <= manaCost) //Do not cast spell if there is not enough mana. 
        {
            Debug.Log("no enough mana");
            //Play some animation like the mana bar shaking//
            return;
        }
        if (GamePauseController.instance.GetCoolDown() == true)
        //Do not cast spell if it is in cooldown. Cooldown is set to false in the last frame of animation. 
        {
            Debug.Log("Skill in cooldown");
            return;
        }

        playerAttackRange = character.useToolRange;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 worldPoint = new Vector2(character.transform.position.x, character.transform.position.y);
        float distance = (mousePos - worldPoint).sqrMagnitude;
        distance = Mathf.Sqrt(distance);

        if (distance > playerAttackRange) { return; } //Do not cast spell if the mouse position is outside of the player's attack range.

        GamePauseController.instance.SetCoolDown(true);
        GameObject go = Instantiate(skillPrefab);
        go.transform.position = mousePos;
    }
}
