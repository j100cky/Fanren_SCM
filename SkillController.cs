using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
	HPRecovery,
	MPRecovery,
	Barrier,
	Other
}

public class SkillController : MonoBehaviour //This script is more for skills that have lasting effects.
{
	[SerializeField] BuffType buffType; //Decclare the type of buff this skill will provide.
	[SerializeField] public Animator anim;
	[SerializeField] public float skillAttackBoxSize;
	[SerializeField] SkillDatasheet skillData;
	Animator interactingObjectAnimator;
	GameObject flyingMount;
	public GameObject player;
	public Character character;
	CharacterController2D charController;
	List<Transform> interactingObjects;
	List<Transform> brokenObjects;
	public ToolHit hit;
	public float damage;

	public float coolDownCounter; 

	public string buffName;
		public virtual void SetBuffName(string name)
		{
			buffName = name;
		}

	public void Init()
    {
		anim = GetComponent<Animator>();
		player = GameManager.instance.player;
		character = player.GetComponent<Character>();
		charController = player.GetComponent<CharacterController2D>();
		coolDownCounter = 0f;
		CalculateDamage();
		SetDirection();
	}

	private void Start()
	{

	}


/*	private void Update()
    {
		//Make sure that the cooldown can be removed on time.
		coolDownCounter += Time.deltaTime;
		Debug.Log(coolDownCounter);
		if(coolDownCounter >=3)
        {
			FinishAnimation();
        }
    }*/

	public virtual void CalculateDamage()
    {
		//Some prefab only want to use the FinishAnimation() function, so they won't have a skillData.
		if(skillData == null) { return; }

		float elementValue = 0f;
		if (skillData.elementType == SkillbookElementType.wood)
        {
			elementValue = character.wood;
        }
		else if(skillData.elementType == SkillbookElementType.fire)
        {
			elementValue = character.fire;

		}
		else if (skillData.elementType == SkillbookElementType.ground)
		{
			elementValue = character.ground;

		}
		else if (skillData.elementType == SkillbookElementType.metal)
		{
			elementValue = character.metal;

		}
		else if (skillData.elementType == SkillbookElementType.water)
		{
			elementValue = character.water;

		}

		damage = skillData.baseDamage + skillData.elementMultiplier * elementValue + skillData.manaMultiplier * character.GetMana();
    }

	public virtual void SetDirection()
    {
		if(anim.GetFloat("lastHorizontal") == null) { return; }
		anim.SetFloat("lastHorizontal", charController.lastMotionVector.x);
		anim.SetFloat("lastVertical", charController.lastMotionVector.y);
    }

	//Used for GatherResourceNode tool actions; 
	public virtual void SetToolHit(ToolHit toolHit)
    {
		hit = toolHit;
    }

	//Used for GatherResourceNode tool actions;
	public virtual void SetDamage(float damageValue)
    {
		damage = damageValue;
    }

	public virtual void SetAttackSize(float sizeValue)
    {
		skillAttackBoxSize = sizeValue;
	}

	//For skills that use OnTilemapAction (e.g. scythe).
	public virtual void SetTileMapReadController(Vector3Int gridPosition, TileMapReadController tileMapReadController)
	{ 

	}


	public BuffType GetBuffType()
    {
		return buffType;
    }

	public virtual void BarrierTakeDamage(float damage) 
		//This is overriden by skills that contain barrier properties, and it is called when the barreir is getting hit.
    {
		return;
    }

	public virtual void FinishAnimation()
	{
		if(gameObject != null)
		{
			Destroy(gameObject);
			ResetCoolDown();
		}

	}

	public void ResetCoolDown()//For animations that does not contain FinishAnimation() function, or those called after a few frames.
	{
		GamePauseController.instance.SetCoolDown(false);
	}

	public void ResetPause()
    {
		GamePauseController.instance.SetPause(false);

	}

	public virtual void PlayHitAnimation()	//This is to be called at the exact frame when the tool hits the object. It makes the object to play the isHit animation. 
	{
		
		interactingObjects = GameManager.instance.interactingObjectContainer.interactingObjects; //Passing the interacting object list from the GameManager to here.
		if(interactingObjects != null)
		{
			for(int i = 0; i<interactingObjects.Count; i++) //For each interacting object...
			{
				interactingObjects[i].GetComponent<Animator>().SetTrigger("isHit"); //trigger the isHit parameter. The interacting object will play the isHit animation.
			}
		}

		//GameManager.instance.GetComponent<GamePauseController>().isCoolingDown = false;
	}

	public virtual void PlayBreakAnimation()
	{
		
		brokenObjects = GameManager.instance.interactingObjectContainer.brokenObjects;
		if(brokenObjects != null)
		{
			for(int i = 0; i<brokenObjects.Count; i++) //For each broken object...
			{
				brokenObjects[i].GetComponent<Animator>().SetBool("isBroken", true); //Set the isBroken parameter to true so the object will play the broken animation.
				
			}		
		}

		//GameManager.instance.GetComponent<GamePauseController>().isCoolingDown = false;
	}

/*	public void StartCoolDown()
	{
		GameManager.instance.GetComponent<GamePauseController>().isCoolingDown = true;
	}*/




}
