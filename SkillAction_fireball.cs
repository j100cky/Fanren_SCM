using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Action/Fire ball single")]
public class SkillAction_fireball : ToolAction
{
    float playerAttackRange; //The range within which the player can cast spells. 

    public override void OnSkillUsed()
    {
        Character character = GameManager.instance.character;

        if (character.currentMana <= manaCost) //Do not cast spell if there is not enough mana. 
        {
            Debug.Log("no enough mana");
            //Play some animation like the mana bar shaking//
            return;
        }
        if(GamePauseController.instance.GetCoolDown() == true) 
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
        character.UseMana(manaCost);
        GamePauseController.instance.SetCoolDown(true);
        GameObject go = Instantiate(skillPrefab);
        go.transform.position = mousePos; 

        //The part of enemy detection and inflict damage is moved to the skill prefab's skill controller script. 
/*        Collider2D[] colliders = Physics2D.OverlapBoxAll(mousePos, attackSize, 0f);

        foreach (Collider2D c in colliders)
        {
            if(c.gameObject.tag == "Pet") { return; } //Do not hit the pet.
            EnemyDrops enemyDrops = c.GetComponent<EnemyDrops>();
            EnemyController enemy = c.GetComponent<EnemyController>();
            if(enemyDrops != null && enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        return;*/
    }
}
