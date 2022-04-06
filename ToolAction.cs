using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolAction : ScriptableObject
{
	public string buffName;
	[SerializeField] public GameObject skillPrefab; //For the instantiation of skill animation.
	[SerializeField] public float manaCost;//For the mana cost of using the item.
	[SerializeField] public float skillAttackSize; //For the effect size of the skill.

	public virtual bool OnApply(Vector2 worldPoint) //Generally for tools that are used in the world, like axe, hammer, most weapons.//
	{
		Debug.LogWarning("OnApply is not implemented");
		return true;
	}

	public virtual bool OnApplyToTileMap(Vector3Int gridPosition, //For tools that are used on tiles, like a shovel.//
		TileMapReadController tileMapReadController, 
		Item item)
	{
		Debug.LogWarning("OnApplyToTileMap is not implemented");
		return true;
	}

	public virtual void OnItemUsed(Item usedItem, ItemContainer inventory) //Use consumables.//
	{
		
	}

	public virtual void OnSkillUsed()
    {

    }

	public virtual void SetSkillPrefab(GameObject prefab)
    {

    }

}
