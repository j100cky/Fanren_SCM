using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwisterSkillScript : SkillController
{
    private GameObject target; 
        public void SetTarget(GameObject go)
    {
        target = go;
    }

    [SerializeField] float damage;
    [SerializeField] float speedReductionPercent;
    [SerializeField] float speedReductionDuration;


    private void Update()
    {
        MoveTowardsEnemy();
    }



    private void MoveTowardsEnemy()
    {
        if(target!=null)
        {
            Vector3 enemyPosition = target.transform.position;
            float distance = (enemyPosition - transform.position).sqrMagnitude;
            if(distance > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                        enemyPosition, 3f * Time.deltaTime);
            }
            else
            {
                HitTarget();
                GamePauseController.instance.SetCoolDown(false);
                Destroy(gameObject);
            }

        }



    }

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
            enemy.MovementSpeedMultiply(1f - speedReductionPercent / 100f, speedReductionDuration, buffName);
        }
    }

}
