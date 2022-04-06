using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class ResourceNode : ToolHit
{
	//======specifies a few var============
	//[SerializeField] GameObject pickUpDrop; //Used for item drop prefab. 
	[SerializeField] int dropCount = 5;
	[SerializeField] float spread = 3f;
	[SerializeField] Item item;
	[SerializeField] int itemCountInOneDrop = 1;
	[SerializeField] ResourceNodeType nodeType;
	[SerializeField] float resourceHealth; 
	Animator anim;
	//public float destroyDelay = 0.75f;

	
	GameObject player;
	//Character character;


	private void Start()
	{
		anim = GetComponent<Animator>();
		//player = GameObject.FindGameObjectWithTag("Player");
		//character = player.GetComponent<Character>();
	}


	public override void Hit(float damage)
	{
		if(resourceHealth > damage)
		{
			resourceHealth -= damage;
			
			GameManager.instance.interactingObjectContainer.interactingObjects.Add(transform);  //we will add it to the list.

			//Play the hit animation
			anim.SetTrigger("isHit");
			return;
		}
		//when the object's health is below 1 (i.e. broken)
		else
        {
			//Add into the broken object list. 
			GameManager.instance.interactingObjectContainer.brokenObjects.Add(transform);
			//Play the broken animation.
			anim.SetBool("isBroken", true);
			//Note that the destroy of the gameobject is called at the exact frame of the item broken animation, not here. 
		}
		

		//The following codes only run 
		
		

		/*if(anim != null)
		{
			anim.SetBool("isBroken", true);
		}*/
		//character.UseMana(1);
		//Destroy(gameObject,destroyDelay); //Destroy the tree when the button "hit" is pressed. 
	}

	public override bool CanBeHit(List<ResourceNodeType> canBeHit)
	{
		return canBeHit.Contains(nodeType);
	}

	//Called at the exact animation frame. 
	public void FinishAnimation()
	{
		if (gameObject != null)
		{
			//Remove the game object.
			Destroy(gameObject);
			//Remove the object from the container, 
			//Otherwise the object (e.g. rock) will remain on the scene when re-enter. 
			GameManager.instance.interactingObjectContainer.brokenObjects.Remove(transform);
			GameManager.instance.GetComponent<PlaceableObjectReferenceManager>().placeableObjectManager.RemoveObject(transform); 
		}

	}

	//Called at the exact animation frame.
	public override void ReleaseLoot()
	{
		while(dropCount > 0)
		{
			dropCount -= 1; //the total drop count is one less than the initial drop count.

			Vector3 position = transform.position;
			position.x += spread * UnityEngine.Random.value - spread / 2;
			position.y += spread * UnityEngine.Random.value - spread / 2;
			ItemSpawnManager.instance.SpawnItem(position, item, itemCountInOneDrop);
		}
	}
}
