using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isSummonedAsPet = false;

    [SerializeField] public EnemyDataSheet enemy; 
    [SerializeField] Transform playerTransform;

    /*    There are enemy's stats that will be given values during start and during update. */
    Sprite enemySprite;
    GameObject animationContainer; 
    
    public float maxHealth;//enemy max health
    public float currentHealth; //enemy current health, will reduce after each damage.

    public SpriteRenderer spriteRenderer; //To access the sprite's color so that each time the enemy takes damage it shows a flashing effect.
    Color damageColor1 = new Color(0.5f,0,0); //Red//
    Color damageColor2 = new Color(0.5f, 0.5f, 0.5f); //Gray//
    Color originalColor = new Color(1,1,1); //White//

    public float maxMana; //The enemy's max mana.
    public float currentMana; //The enemy's current mana. Will be reduced after every skill used. 


    public float enemyMovementSpeed; 
    private float nextMoveTime;
    public float enemyAttackRate;
    public float enemyAttackRange;
    public float enemyVisualRange;
    //[SerializeField] Rigidbody2D rbody;
    //
    public float enemyPushBackDistance; //The multiplier determining how far the player is pushed back by a hit.//
    public float PATTToPlayer; //Physical damage.
    public float MATTToPlayer; //Magical damage.

    List<ItemSlot> enemyLoots; 

    EnemyDrops enemyDrops; //For using the Hit() function when enemy dies.//

    private float closestDistance; //For closest object determination.  
    private GameObject closestObject; //For recording the closest object (player or pet).

/*These are the private variables that are used for different calculation*/
    private float nextAttackTime = 0; //Time to attack. If tta < attack interval, the player is invulnerable, and tta += delta time. //
    
    float pushBack; //For enemy being pushed back, calculated by damage/maxHealth
    

    GameObject player;
    Character character;
    
    float distance; //distance between player and enemy.//
    float horizontal; //Determining the position of pleyer to the enemy//
    float vertical; //Determining the position of pleyer to the enemy//
    Animator anim;
    //Vector3 offset;

    public Vector3 attackOffset; //How much the enemy move forward or backward when attacking.//

    bool isSpawning; //This is set true during before the Appear animation finishes. 

    bool isStun; //This is modified by spells that hit the enemy. 
    
    

    private void LoadEnemyData()
    {
        maxHealth = enemy.HP;
        maxMana = enemy.MP;
        PATTToPlayer = enemy.PATT;
        MATTToPlayer = enemy.MATT;
        enemyMovementSpeed = enemy.enemyMovementSpeed;
        enemyAttackRate = enemy.enemyAttackRate;
        enemyVisualRange = enemy.enemyVisualRange;
        enemyAttackRange = enemy.enemyAttackRange;
        enemyPushBackDistance = enemy.enemyPushBackDistance;
        enemyLoots = enemy.drops;
        enemySprite = enemy.enemyIcon;
        animationContainer = enemy.animationContainer;
    }

    private void Start()
    {
        
    	player = GameManager.instance.player;
        playerTransform = player.transform;
        character = player.GetComponent<Character>();
        LoadEnemyData();
        anim = GetComponent<Animator>();
        Facing();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        enemyDrops = GetComponent<EnemyDrops>();
        closestDistance = 100f;
        isSpawning = true; 
    }

    private void FixedUpdate()
    {
        if(GamePauseController.instance.GetPause() == true) { return; }
        if(isSummonedAsPet == true) { return; }
    	if(enemyDrops.isDestroyed == true){return;}
        if(isStun == true) { return; }
        if(isSpawning == true) { return; }
        DetectColliders();
        //If nothing is within the visual range of the enemy, the enemy will remain idle.
        if(closestDistance > enemyVisualRange)
        {
            return;
        }
        else
        {
            //When the object is close enough within the attack range of the enemy, the enemy will stop and attack.
            if(closestDistance <= enemyAttackRange)
            {
                if(closestObject == null) { return; }
                if(Time.time >= nextAttackTime)
                {
                    anim.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1f / enemyAttackRate;
                }
                else
                {
                    return;
                }
            }
            //When the object is within visual range but outside attack range, the enemy will move towards it.
            else
            {
                Move();
            }
        }
    }  

    //This function is called by the last frame of the Appearing animation. 
    public void FinishSpawning()
    {
        isSpawning = false;
    }

    private void DetectColliders()
    {
        closestDistance = 100f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyVisualRange);
        foreach (Collider2D c in colliders)
        {
            //If the collider is a player or pet object, find the distance between the object and self.
            if (c.gameObject.tag == "Player")
            {
                distance = Vector3.Distance(transform.position, c.transform.position);
                //Update the variable closestDistance. This is used to determine the closest Enemy among all others. 
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = c.gameObject;
                }
            }
            //If the collider is a pet, find the distance between the object and self.
            else if (c.gameObject.tag == "Pet")
            {
                distance = Vector3.Distance(transform.position, c.transform.position);
                //Update the variable closestDistance. This is used to determine the closest Enemy among all others. 
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = c.gameObject;
                }
            }
            else // If the collider is neither a pet or a player, ignore this collider. 
            {
                continue;
            }
        }
    }
    
    private void Facing()
    {
        if(closestObject == null) { return; }
        Vector3 direction = (closestObject.transform.position-transform.position).normalized;
        horizontal = direction.x;
        vertical = direction.y;
        if(horizontal < 0) //Facing left
        {
            if(vertical < 0) //Facing down
            {
                if(Mathf.Abs(horizontal) < Mathf.Abs(vertical))
                {
                    anim.SetInteger("Facing", 0); //Facing down
                }    
                else
                {
                    anim.SetInteger("Facing", 1); //Facing left
                }
            }
            else
            {
                if (Mathf.Abs(horizontal) < Mathf.Abs(vertical))
                {
                    anim.SetInteger("Facing", 2); //Facing Up
                }
                else
                {
                    anim.SetInteger("Facing", 1); //Facing left
                }
            }

        }
        else
        {
            if (vertical < 0) //Facing down
            {
                if (Mathf.Abs(horizontal) < Mathf.Abs(vertical))
                {
                    anim.SetInteger("Facing", 0); //Facing down
                }
                else
                {
                    anim.SetInteger("Facing", 3); //Facing right
                }
            }
            else
            {
                if (Mathf.Abs(horizontal) < Mathf.Abs(vertical))
                {
                    anim.SetInteger("Facing", 2); //Facing Up
                }
                else
                {
                    anim.SetInteger("Facing", 3); //Facing right
                }
            }
        }
        
        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
    }
    private void Move()
    {
        /*        if(Time.time >= nextMoveTime)
                {
                    Facing();
                    Vector3 offset = new Vector3(horizontal, vertical);
                    transform.position += offset;
                    nextMoveTime = Time.time + 1f/enemyMovementSpeed;
                }*/
        Facing();
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, enemyMovementSpeed * Time.deltaTime);

    }

    private void PushPlayer()
    {
        player.transform.position += new Vector3(horizontal*enemyPushBackDistance, vertical*enemyPushBackDistance);
    }

    public void Attack() //This function is called in the animation key frame.
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        if(isSummonedAsPet == false) //When this prefab is an enemy, the Attack() function will inflict damage to the player.
        {
            if (closestDistance >= enemyAttackRange) { return; }//Allow the player to escape from a hit by getting out of the attack range at the last frame. 
            if (closestObject == null)  // If the pet or player is being destroyed, reset closestObject and closestDistance, and return.
            {
                DetectColliders();
                if (closestObject == null)
                {
                    closestDistance = 100f;
                    return;
                }
                else
                {
                    return;
                }
            }
            if (closestObject.tag == "Player")
            {
                character.TakeDamage(PATTToPlayer);
                PushPlayer();
                return;
            }
            else if(closestObject.tag == "Pet")
            {
                closestObject.GetComponent<PetController>().TakeDamage(PATTToPlayer);
                return;
            }
            else
            {
                return;
            }
        }
        else //When the prefab is used as a pet, run the PetAttack() function instead of the Attack() function.
        {
            gameObject.GetComponent<PetController>().PetAttack(); 
        }
     
    }

    public void TakeDamage(float damage) //The enemy taking damage.
    {
        if(currentHealth <= 0 || currentHealth - damage <= 0)
        {
            enemyDrops.Hit();//This includes the death animation and dropping.//
            //Give EXP to the main skillbook
            player.GetComponent<SkillbookPracticeManager>().GetMainSkillbook().ChangeEXP(enemy.EXP);
        }
        else{
            currentHealth -= damage;
            MessageManager.instance.CallMsgPanel("-"+damage.ToString(), transform, Color.red); //Call the message panel text to show damage. 
            anim.SetTrigger("isHit");
            pushBack = 10*damage/maxHealth;
            transform.position += new Vector3(horizontal*-pushBack, vertical*-pushBack); //Will change this pushback parameter to damage proportional to maxhealth//  
        }

    }


    [SerializeField] List<string> buffs; 

    public void AddBuffs(string buffName)
    {
        buffs.Add(buffName);
    }
    public void RemoveBuffs(string buffName)
    {
        buffs.Remove(buffName);
    }

    public void MovementSpeedMultiply(float value, float duration, string buffName)
    {
        StartCoroutine(MovementSpeedMultiplyCountDown(value, duration, buffName));
    }

    public void Stun(float duration, string buffName)
    {
        StartCoroutine(StunCoroutine(duration, buffName));
    }

    public IEnumerator MovementSpeedMultiplyCountDown(float value, float duration, string buffName)
    {
        if (buffs.Contains(buffName))
        {
            yield return null;
        }
        else
        {
            AddBuffs(buffName);
            //Play the slowed animation. 
            float originalSpeed = enemyMovementSpeed;
            enemyMovementSpeed = enemyMovementSpeed * value;
            yield return new WaitForSeconds(duration);
            enemyMovementSpeed = originalSpeed;
            RemoveBuffs(buffName);
        }

    }

    public IEnumerator StunCoroutine(float duration, string buffName)
    {
        if (buffs.Contains(buffName)) { yield return null; }
        else
        {
            AddBuffs(buffName);
            //Play the stun animation. 
            isStun = true;
            yield return new WaitForSeconds(duration);
            isStun = false;
            RemoveBuffs(buffName);
        }
    }

}
