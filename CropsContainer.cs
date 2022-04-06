using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script will store all information about crops including their crop, their position.
[CreateAssetMenu(menuName = "Data/Crops Container")]

public class CropsContainer : ScriptableObject
{
	public List<CropTile> crops;

	public CropTile Get(Vector3Int position)
	{
		return crops.Find(x => x.position == position);
	}

	public void Add(CropTile crop)
	{
		crops.Add(crop);
	}

	public void RemoveAll() //Used when the list needs to be reset (such as new game or loaded game.)
    {
		crops = new List<CropTile>();
    }
}
