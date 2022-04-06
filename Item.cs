using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum itemCategory
{
	Weapon,
	Tools,
	Medicine,
	Crops,
	Material,
	HeatSource,
	Recipe,
	Skillbook,
	Seeds,
	Misc,
	Helm, 
	Armor,
	Boots,
	MagicObject,
	Dan
}


/*[Serializable]
public class SkillbookStatus
{
	public List<float> levelUpEXP;
	public float currentEXP;
	public float perTickEXP;
}
*/

[CreateAssetMenu(menuName = "Data/Item")]

public class Item : ScriptableObject
{
	public string Name;
	public int ID;
	public itemCategory category;
	//public SkillbookStatus skillbookStatus;//For setting how much EXP per level for skillbooks and accessing  current exp for this skillbook.
	[TextArea(5, 10)]
	public string itemDescription;
	public bool isStackable;
	public Sprite icon;

    public float maxLingqi;
    public float currentLingqi;
    public float lingqiRate;  //Rate of lingqi gained per second fo crops. 

    public ToolAction onAction; //for the action of the item.
	public ToolAction onTileMapAction; //for animation on the tile maps (e.g. plowing, seeding).
	public ToolAction onItemUsed;
	public CarryingEffects carryingEffects; //The effects this item provides when it is equipped. 
	public Crop crop; //If this item is a seed, we will need the info about the crop it grows into.
	//public bool hasRange; Don't need this since we are using the vlue of attackRange as the condition in SkillCastPreview.//
	public float attackSize;
	public float craftingTime; //The time needed to craft.

	public bool isConsumable; //For triggering the ItemConsumptionController's methods.//
	public float restoreHP; //Will be used by the itemConsumptionController script to determine how much HP, MP, or EXP given by this item.//
	public float restoreMP;
	public float giveEXP;

	public int purchasePrice;
	public int sellingPrice;

	public bool iconHighlight; //For determining whether the item has a preview on the map when selected.
	public GameObject itemPrefab; //For placing the item onto the map.

	public List<SkillDatasheet> skills; //For skillbooks, there will be a series of skills provided by this skillbook item.

	public float craftingTimeMultiplier; //For accelerating crafting speed.
	public float craftingQualityMultiplier; //For making a better quality craft. 
	public float giveCraftEXP; //The amount of EXP that it gives when one of this item is crafted.

	//The following parameters are more for MO's, unless some items permenantly increase palyer status. 
	public float increaseMaxHealth;
	public float increaseMaxMana;
	public float increasePower;
	public float increaseIntel;
	public float increaseDefense;
	public float increaseMentality;

}

