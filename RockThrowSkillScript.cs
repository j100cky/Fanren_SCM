using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowSkillScript : SkillController
{
    [SerializeField] string buffName = "RockThrow";
    private GameObject target;
    public void SetTarget(GameObject go)
    {
        target = go;
    }

    [SerializeField] float damage;
    [SerializeField] float stunDuration;
    public Rigidbody2D rb;
    public float speed = 3f; 

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
        rb.velocity = direction * speed;
    }

    //This is called when the rock hits the enemy. 
    public override void PlayHitAnimation()
    {

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 0.5f);

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

    private void Update()
    {
        DetermineTargetDistance();
    }

    private void DetermineTargetDistance()
    {
        if (target != null)
        {
            Vector3 enemyPosition = target.transform.position;
            float distance = (enemyPosition - transform.position).sqrMagnitude;
            if (distance < skillAttackBoxSize)
            {
                anim.SetTrigger("hit");
            }
            //Destroy the prefab when it misses the enemy and went too far. 
            float toPlayerDistance = (player.transform.position - transform.position).sqrMagnitude;
            if(toPlayerDistance > 20f)
            {
                FinishAnimation();
            }
        }
    }

    //This is called by the animation. 
    public void HitTarget()
    {

        EnemyDrops enemyDrops = target.GetComponent<EnemyDrops>();
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemyDrops == null || enemy == null)
        {
            return;
        }

        else
        {
            enemy.TakeDamage(damage);
            enemy.Stun(stunDuration, buffName);
        }
    }


}
