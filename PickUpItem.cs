using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
	GameObject player; 
	[SerializeField] float speed; //the speed of the moving object. 
	[SerializeField] float pickUpDistance;
	[SerializeField] float ttl; //time to leave for the object to disappear if not picked up.
	Character character; 

	public Item item;
	public int count = 1;

	private void Awake()
	{
		player = GameManager.instance.player; //obtain the player's information from the GameManager script. 
		character = GameManager.instance.character; //obtain the player's character information for stats-related calculations. 
	}

	public void Set(Item item, int count)
	{
		this.item = item;
		this.count = count;

		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = item.icon;

	}

	private void Update()
	{
		//========object disappears after ttl reaches 0. 
		ttl -= Time.deltaTime;
		if(ttl < 1)
		{
			Destroy(gameObject);
			Debug.Log("disappears");
		}

		float distance = Vector3.Distance(transform.parent.position, player.transform.position); //calculate the distance between the object and the player. 
		if (distance > pickUpDistance)
		{
			return; //if the player is not close to the object, notthing happens.
		}

		transform.parent.position = Vector3.MoveTowards(transform.parent.position, 
			player.transform.position, 
			speed * Time.deltaTime); //use a function to move the object towards the player. 

		if(distance < 0.2f)
		{
			if(GameManager.instance.inventoryContainer != null) //when the game manager has the inventory container attached...
			{
				GameManager.instance.inventoryContainer.Add(item, count); //we will use the Add function in the InventoryContainer script,
																		  //which is to update the inventory with the item.
				UpdateQuestProgress();
			}
			else
			{
				Debug.LogWarning("No inventory container attached to the game manager!");
			}
			Destroy(gameObject);//when the object reaches the player, it disappears.

		}
	}

	private void UpdateQuestProgress()
    {
		for(int i = 0; i < character.activeQuests.Count; i++) //For each quest that has the same goal,
                                                              //increase the currentAmount for quest progression. 
        {
			character.activeQuests[i].questGoal.ItemCollected(item); 
		}
    }
}
