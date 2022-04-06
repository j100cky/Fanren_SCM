using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSingleSkillController : SkillController
{

    public override void PlayHitAnimation()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePos, skillAttackBoxSize);

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
