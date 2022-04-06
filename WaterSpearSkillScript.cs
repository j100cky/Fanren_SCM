using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpearSkillScript : SkillController
{
    [SerializeField] private List<GameObject> targets;
        public void SetTarget(GameObject go)
        {
            targets.Add(go);
        }
    
    [SerializeField] float damage;

    public void HitTarget() // Will be called by the right animation frame.
    {
        foreach(GameObject target in targets)
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
            }
        }

    }
}
