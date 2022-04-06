using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Skill Action/Water Spear")]

public class SkillAction_WaterSpear : ToolAction
{
    float playerAttackRange; //The range within which the player can cast spells. 
    //[SerializeField] float manaCost; //mana cost of using the spell.
    //[SerializeField] GameObject skillPrefab; //Prefab for the animator. 
    //[SerializeField] int skillAttackSize = 3; //The attack box radius that the mouse will use to determine the closest enemy. Should be very small. 

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

        //Calculate whether the mouse is pointing to a out-of-range enemy.
        playerAttackRange = character.useToolRange;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Vector2 worldPoint = new Vector2(character.transform.position.x, character.transform.position.y);
        float distance = (mousePos - worldPoint).sqrMagnitude;
        distance = Mathf.Sqrt(distance);

        if (distance > playerAttackRange) { return; } //Do not cast spell if the mouse position is outside of the player's attack range.

        //Detect enemy at mouse position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, skillAttackSize);
        foreach(Collider2D c in colliders)
        {
            if (c == null || c.tag != "Enemy")
            {
                Debug.Log("no target enemy");
                return;
            }
            else
            {
                Debug.Log(c);
                //Instantiate the prefab.
                GamePauseController.instance.SetCoolDown(true);
                GameObject go = Instantiate(skillPrefab);
                go.transform.position = mousePos;
                go.GetComponent<WaterSpearSkillScript>().SetTarget(c.gameObject);
            }
        }
        


        //The attacking script will be coded in the TwisterSkillScript script.             
    }
}
