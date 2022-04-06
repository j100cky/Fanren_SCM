using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_RedKing : CarryingEffects
{
    [SerializeField] Animator anim;
    //[SerializeField] GameObject skillCastPreview;
    
    [SerializeField] int damage;
    [SerializeField] int coolDown;
    [SerializeField] Vector2 attackBox = new Vector2(6.5f, 3);


    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    public override void OnEquiped(Transform transform)
    {
        GameObject o = Instantiate(gameObject, transform);
        o.transform.position = transform.position; //Make the object follow the player. 
        GameManager.instance.statusPanel.GetComponent<StatusPanelController>().UpdateEquippedPrefabs(o);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            OnActive();
        }
    }

    public override void OnActive() //For attacking MOs, the OnActive() function is called automatically, after each coolDown timer. 
    {
        StartCoroutine(CoolDownCounter());
    }

    IEnumerator CoolDownCounter()
    {
        yield return new WaitForSeconds(coolDown);
        anim.SetBool("isActive", true);//Play the Active animation after waiting for coolDown. 
        DamageEnemies();
    }

    public void OnAnimationEnds()
    {
        anim.SetBool("isActive", false);
    }



    private void DamageEnemies()
    {
        Vector2 centerPosition = new Vector2(GameManager.instance.player.transform.position.x + 5, 
            GameManager.instance.player.transform.position.y+1);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPosition, attackBox, 0f);
        foreach (Collider2D c in colliders)
        {
            EnemyDrops enemyDrops = c.GetComponent<EnemyDrops>();
            EnemyController enemy = c.GetComponent<EnemyController>();
            if (enemy != null && enemyDrops != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
