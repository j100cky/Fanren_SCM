using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider2D))]

public class EnemyDrops : WeaponAttack
{
	//======specifies a few var for spawning drops============
	[SerializeField] public EnemyDataSheet enemy; 
	public List<ItemSlot> drops;
/*	[SerializeField] GameObject transitionPrefab; //Enemy may drop the transition prefab to next map with some chance when kill. 
	[SerializeField] float dropTransitionChance; //The chance that the enemy drops the exit.*/ 
	[SerializeField] float spread = 3f;
	[SerializeField] EnemyType enemyType;
	public float giveEXP;
	Animator anim; //Will allow the game object to play the animation if the condition is right judging from the Hit() function.
	public bool isDestroyed; //To stop the EnemyController Update() method from continuing.//

	
	GameObject player;
	Character character;




	private void Start()
	{
		anim = GetComponent<Animator>();
		player = GameManager.instance.player;
		character = player.GetComponent<Character>();
		LoadEnemyDropsData();
	}

	//Will tell this script what the drop and exp is for the enemy from the EnemyDataSheet. 
	private void LoadEnemyDropsData()
	{
		drops = enemy.drops;
		giveEXP = enemy.EXP;
	}

	//This function is called when the enemy dies. It involves dropping and giving exp. 
	public override void Hit()
	{
		for(int i = 0; i< drops.Count; i++) 
        {
			Vector3 position = new Vector3(transform.position.x + spread * UnityEngine.Random.value - spread / 2,
				transform.position.y + spread * UnityEngine.Random.value - spread / 2,
				transform.position.z);
			position.x += spread * UnityEngine.Random.value - spread / 2;
			position.y += spread * UnityEngine.Random.value - spread / 2;
			ItemSpawnManager.instance.SpawnItem(position, drops[i].item, drops[i].count); //Each time, spawn the item listed
                                                                                          //in the drops list. 
		}
		if(anim != null)
		{
			anim.SetBool("isDead", true);
		}
		SceneExitDropManager.instance.SpawnTransition(transform.position);
		Destroy(gameObject); //Destroy the enemy prefab. 
		character.GainBattleEXP(giveEXP); //Give player specifically the BattleEXP.//
		//Debug.Log(character.currentBattleEXP + "/" + character.maxBattleEXP);
		for(int i = 0; i < character.activeQuests.Count; i++)
        {
			//Will increase the currentAmount of the questGoal for each quest that requires killing of enemies.
			character.activeQuests[i].questGoal.KillEnemy(enemy.Name);  
		}
		isDestroyed = true;
	}

	public override bool CanBeHit(List<EnemyType> canBeHit)
	{
		return canBeHit.Contains(enemyType);
	}
}
