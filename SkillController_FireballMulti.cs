using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController_FireballMulti : SkillController
{
    [SerializeField] Vector2 attackSize;
    [SerializeField] float damage; //The damage that this skill will do to enemy. 

    public override void PlayHitAnimation()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Collider2D[] colliders = Physics2D.OverlapBoxAll(mousePos, attackSize, 0f);

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Pet") { return; } //Do not hit the pet.
            EnemyDrops enemyDrops = c.GetComponent<EnemyDrops>();
            EnemyController enemy = c.GetComponent<EnemyController>();
            if (enemyDrops != null && enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        return;
    }

}
