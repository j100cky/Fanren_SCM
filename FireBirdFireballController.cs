using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBirdFireballController : MonoBehaviour
{
    GameObject target; //The firealls will go towards the target. 
        public void SetTarget(GameObject go)
        {
            target = go;
        }
    
    GameObject host; //The fireballs will follow the host. 
    
    float damage; //Damage of each fireball.
        public void SetDamage(float value)
        {
            damage = value;
        }

    float travelSpeed;
        public void SetTravelSpeed(float value)
        {
            travelSpeed = value;
        }

    [SerializeField] Animator anim;


    
    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if(target != null)
        {
            float distance = (gameObject.transform.position - target.transform.position).sqrMagnitude; 
            if(distance > 0.5f) //When the distance is larger than 0.5f, the ball keeps moving towards the targer. 
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,
                    target.transform.position, travelSpeed*Time.deltaTime); 
            }
            else
            {
                anim.SetTrigger("explode"); 
            }
        }
        else 
        {
            anim.SetTrigger("explode");
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
        }
    }

    public void FinishAnimation()
    {
        Destroy(gameObject);
    }

    private void FollowHost()
    {
        gameObject.transform.position = host.transform.position;
    }

}
