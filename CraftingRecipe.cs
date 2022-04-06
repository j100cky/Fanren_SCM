using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Recipe")]

public class CraftingRecipe : ScriptableObject

{
	public List<ItemSlot> elements; //elements are the raw materials needed to craft.
	public ItemSlot output; //Output is the item theplayer obtain after crafting.
}
