using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapons")]

public class Weapons : ScriptableObject
{
	public string Name;
	public bool isStackable;
	public Sprite icon;
	public ToolAction onAction; //for the action of the item.
	public ToolAction onTileMapAction; //for animation on the tile maps (e.g. plowing, seeding).
	public ToolAction onItemUsed;
}