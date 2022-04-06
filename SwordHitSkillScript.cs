using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitSkillScript : SkillController
{

    public Rigidbody2D rb;
    public float speed = 5f;

    private void Start()
    {
        Init();
        Shoot();
    }

    public void Shoot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //Determine the position of the mouse to the player.
        Vector2 direction = mousePos - new Vector2(player.transform.position.x, player.transform.position.y);
        direction = direction.normalized;
        rb.velocity = direction* speed; 
    }

    //This is called at the start of the prefab instantiation. 
    public override void SetDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        //Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        float horizontal = (mousePos.x - player.transform.position.x);
        anim.SetFloat("horizontal", horizontal);
    }

    //This is called at the specific frame when the sword reaches the longest point.
    public override void PlayHitAnimation()
    {

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, skillAttackBoxSize);

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
