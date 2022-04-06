using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public bool isSummoned = false;
    [SerializeField] public EnemyDataSheet pet;

    public float maxHealth;//pet max health
    public float currentHealth; //pet current health, will reduce after each damage.

    public float maxMana; //The pet's max mana.
    public float currentMana; //The pet's current mana. Will be reduced after every skill used.

    public float petMovementSpeed;
    private float nextMoveTime;
    public float petAttackRate;
    public float petAttackRange;
    public float petVisualRange;

    public float petPushBackDistance; //The multiplier determining how far the target is pushed back by a hit.//
    public float PATT; //Physical damage.
    public float MATT;

    List<ItemSlot> petLoots;
    Sprite petSprite;

    [SerializeField] float petSurroundDistance = 3f;
    [SerializeField] List<Vector3> petSurroundVectors;

    /*These are the private variables that are used for different calculation*/
    private float nextAttackTime = 0; //Time to attack. If tta < attack interval, the player is invulnerable, and tta += delta time. //

    float pushBack; //For enemy being pushed back, calculated by damage/maxHealth

    float distance; //distance between pet and enemy.//
    float horizontal; //Determining the position of pleyer to the enemy//
    float vertical; //Determining the position of pleyer to the enemy//

    float closestDistance = 100f;
    GameObject closestEnemy;

    Animator anim;
    GameObject player;
    Character character;

    public Vector3 attackOffset; //How much the enemy move forward or backward when attacking.//

    private void LoadPetData()
    {
        maxHealth = pet.HP;
        maxMana = pet.MP;
        PATT = pet.PATT;
        MATT = pet.MATT;
        petMovementSpeed = pet.enemyMovementSpeed;
        petAttackRate = pet.enemyAttackRate;
        petVisualRange = pet.enemyVisualRange;
        petAttackRange = pet.enemyAttackRange;
        petPushBackDistance = pet.enemyPushBackDistance;
        petLoots = pet.drops;
        petSprite = pet.enemyIcon;
        //animationContainer = pet.animationContainer;
    }

    private void Start()
    {
        player = GameManager.instance.player;
        //playerTransform = player.transform;
        //rbody = player.GetComponent<Rigidbody2D>();
        character = player.GetComponent<Character>();
        LoadPetData();
        anim = GetComponent<Animator>();
        Facing();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        //enemyDrops = GetComponent<EnemyDrops>();
        petSurroundVectors = new List<Vector3>() {
        new Vector3(0, 1, 0),
        new Vector3(0, 1, 0),
        new Vector3(-1,0,0),
        new Vector3(-1,0,0),
        new Vector3(-1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,-1,0),
        new Vector3(0,-1,0),
        new Vector3(0,-1,0),
        new Vector3(0,-1,0),
        new Vector3(1,0,0),
        new Vector3(1,0,0),
        new Vector3(1,0,0),
        new Vector3(1,0,0),
        new Vector3(0, 1, 0),
        new Vector3(0, 1, 0),
        };
    }


    private void Update()
    {   
        if (isSummoned == false) { return; } //Do not run the following code if this prefab is not a pet.
                                             //This is because both EnemyController and PetController scripts share one monster prefab. 
        //First, detect colliders around itself, using petVisualRange as the range. 
        DetectEnemy();
        //If no Enemy is detected within the visual range (if closestDistance is larger than visual range), then pet goes idle. 
        if(closestDistance > petVisualRange)
        {
            ReturnToPlayer();
        }
        //If the closestDistance is smaller than the visual range, the pet should move toward the Enemy. 
        else
        {
            //If the closestDistance is smaller than the petAttackRange, then the pet will play the attacking animation
            if (closestDistance <= petAttackRange)
            {
                if(Time.time >= nextAttackTime)
                {
                    anim.SetTrigger("Attack");
                    nextAttackTime = Time.time + 1f / petAttackRate;
                }
                else
                {
                    return;
                }

            }
            //If the closestDistance is larger than the petAttackRange, then the pet will move toward the enemy.
            else
            {
                MoveTowardsEnemy();
            }
        }
    }

    private void DetectEnemy()
    {
        closestDistance = 100f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, petVisualRange);
        foreach (Collider2D c in colliders)
        {
            //If the collider is not an Enemy-tag object, ignore it and move to the next collider. 
            if (c.gameObject.tag != "Enemy")
            {
                continue;
            }
            //If the collider is an Enemy, find the distance between the Enemy and self.
            distance = Vector3.Distance(transform.position, c.transform.position);
            //Update the variable closestDistance. This is used to determine the closest Enemy among all others. 
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = c.gameObject;
                //Debug.Log(c.gameObject.name);
            }
        }
    }


    private void Facing() //Controll the facing of the pet toward the closest enemy.
    {
        //If there is no closest enemy, the facing will be towards the player if returning to the player, or facing where it should if idling. 
        if (closestEnemy == null) 
        {
            float playerDistance = Vector3.Distance(player.transform.position, transform.position);
            //If the PetIdle() function is played, then facing is the same as the surrounding motion.
            if(playerDistance <= 2)
            {
                horizontal = player.GetComponent<CharacterController2D>().motionVector.x;
                vertical = 0;
            }
            //If the ReturnToPlayer() function is playing, then facing is the direction towards the player. 
            else
            {
                Vector3 playerDirection = (player.transform.position - transform.position).normalized;
                horizontal = playerDirection.x;
                vertical = playerDirection.y;
            }
        }
        //If there is a closestEnemy, the facing will be towards the closest enemy.
        else
        {
            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;
            horizontal = direction.x;
            vertical = direction.y;
        }

        //The code below describes the determination of facing taking into account the size of x and y vector. 
        if (horizontal < 0) //Facing left
        {
            if (vertical < 0) //Facing down
            {
                if (Mathf.Abs(horizontal) < Mathf.Abs(vertical))
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

    private void MoveTowardsEnemy()
    {
        if (Time.time >= nextMoveTime)
        {
            Facing();
            Vector3 offset = new Vector3(horizontal, vertical);
            transform.position += offset;
            nextMoveTime = Time.time + 1f / petMovementSpeed;
        }
    }

    private void ReturnToPlayer()
    {
        Vector3 playerDirection = (player.transform.position - transform.position).normalized;
        float playerDistance = Vector3.Distance(player.transform.position, transform.position);
        //If the pet is close enough to the player, run the Idle() function instead of keep trying to approach the player. 
        if(playerDistance <= 2) 
        {
            PetIdle();
            return;
        }
        else //If the pet not close enough to the player, keep moving towards it.
        {
            if (Time.time >= nextMoveTime)
            {
                Vector3 offset = new Vector3(playerDirection.x, playerDirection.y);
                transform.position += offset;
                //transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed*Time.deltaTime);
                nextMoveTime = Time.time + 1f / petMovementSpeed;
            }
        }       

    }

    int petSurroundCounter = 0; //This is used to indicate which direction the pet should move.
                                //Each number is determined by the list of Vector3 in petSurroundVectors
    private void PetIdle() //Walk around the player. 
    {
        Facing();
        if (Time.time >= nextMoveTime)
        {
            transform.position += petSurroundVectors[petSurroundCounter];
            nextMoveTime = Time.time + 1f / petMovementSpeed;
            petSurroundCounter++;
        }
        if (petSurroundCounter >= petSurroundVectors.Count) { petSurroundCounter = 0; } //Reset the counter so the pet will move in a loop. 
    }

    public void PetAttack() //This function is called by the animation frame's Attack() function. When the prefab is summoned as a pet,
                         //this function will be called instead of the Attack() function in the EnemyController script. 
    {
        if (closestDistance >= petAttackRange) { return; }; //Allow the enemy to flee at the right frame. 
        //If the closestEnemy is being destoyed, then set the closestDistance to a very big number, so the pet will start moving back to the player. 
        if (closestEnemy == null) 
        {
            DetectEnemy();
            if(closestEnemy == null)
            {
                closestDistance = 100f;
                return;
            }
            else
            {
                return;
            }
        }
        else //If the closestEnemy exist, then make the closestEnemy take damage.
        {
            closestEnemy.GetComponent<EnemyController>().TakeDamage(PATT);
            //isAttacking = false; //Reset the value of isAttacking so that Update() can continue to run. 
        }
    }

    public void TakeDamage(float damage) //This function is called by the Enemy attacking the pet. 
    {
        if (currentHealth <= 0 || currentHealth - damage <= 0)
        {
            MessageManager.instance.CallMsgPanel("Pet died.", transform, Color.red); 
            Destroy(gameObject);
        }
        else
        {
            currentHealth -= damage;
            MessageManager.instance.CallMsgPanel("-" + damage.ToString(), transform, Color.red); //Call the message panel text to show damage. 
            pushBack = 10 * damage / maxHealth;
            transform.position += new Vector3(horizontal * -pushBack, vertical * -pushBack); //Will change this pushback parameter to damage proportional to maxhealth//  
        }

    }
}
