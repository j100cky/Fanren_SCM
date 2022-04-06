using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBirdSkillScript : SkillController
{

    float fireBirdAttackRange;
    [SerializeField] float fireballDamage;
    [SerializeField] float fireballTravelSpeed;
    [SerializeField] GameObject fireballPrefab;
    bool isCoolingDown; 

    public void SetFireballDamage(float value)
    {
        fireballDamage = value;
    }

    void Update()
    {
        if(isCoolingDown == true) { return; }
        StartCoroutine(DetectAndSummonFireball());
    }


/*    public void HitEnemy()
    {
        fireBirdAttackRange = character.useToolRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, fireBirdAttackRange);
        if(colliders != null)
        {
            anim.SetTrigger("attack");
            foreach (Collider2D c in colliders)
            {
                if (c.gameObject.tag == "Pet") { return; } //Do not hit the pet.
                EnemyDrops enemyDrops = c.GetComponent<EnemyDrops>();
                EnemyController enemy = c.GetComponent<EnemyController>();
                if (enemyDrops != null && enemy != null)
                {
                    enemy.TakeDamage(fireballDamage);
                }
            }
        }
        else
        {
            return;
        }

    }*/

    public IEnumerator DetectAndSummonFireball()
    {
        isCoolingDown = true;
        fireBirdAttackRange = character.useToolRange;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(character.transform.position, fireBirdAttackRange);
        if (colliders != null)
        {
            foreach (Collider2D c in colliders)
            {
                if (c.gameObject.tag == "Pet" || c.gameObject.tag == "Player") { continue ; } //Do not hit the pet.
                Vector3 position = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                    transform.position.y + 3f + Random.Range(-0.5f, 0.5f), transform.position.z);
                GameObject go = Instantiate(fireballPrefab, position, Quaternion.identity);
                go.GetComponent<FireBirdFireballController>().SetTarget(c.gameObject); //Register the target enemy that the fireball should go to.
                go.GetComponent<FireBirdFireballController>().SetDamage(fireballDamage); //Register the damage of each fireball to the fireball prefab. 
                go.GetComponent<FireBirdFireballController>().SetTravelSpeed(fireballTravelSpeed); 
            }
            
        }
        yield return new WaitForSeconds(1f); //Waiting time between attacks.
        isCoolingDown = false;


    }

}
