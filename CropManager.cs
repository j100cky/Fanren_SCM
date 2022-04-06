using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]

public class CropTile // this class will contain information about the crop. 
{
	public int growTimer;
	public Crop crop;
	public SpriteRenderer renderer;
	public int growStage;
	public float damage; //for crop damage over time
	public Vector3Int position;
	public bool isGrown; //Used by the TilemapWildCropsManager. When this is true, show the crop directly. When it is false, have a chance to show. 

	//To check if the crop has reached its harvestable state. 
	public bool Complete
	{
		get{
			if (crop == null) {return false;}
			return growTimer >= crop.timeToGrow;
		}
	}

	internal void Harvested()
	{
		growTimer = 0;
		growStage = 0;
		crop = null;
		damage = 0;
		renderer.gameObject.SetActive(false);
	}

	//This is used by the TilemanWildPlantManager. For wild plants, we dont want the crop to be removed.
    //Instead, we only want the sprite to go away and when the player re-enter (perhaps on another day), the plants will grow back. 
	internal void WildPlantHarvested()
    {
		growTimer = 0;
		growStage = 0;
		renderer.gameObject.SetActive(false);
    }
}

public class CropManager : MonoBehaviour
{
	public TilemapCropsManager cropsManager;
	public TilemapWildCropsManager wildCropManager;
	public bool roadToTownRandomized; 

	public void PickUp(Vector3Int position)
	{
		if(cropsManager != null) 
		{
			cropsManager.PickUp(position);
		}
		if(wildCropManager != null)
        {
			wildCropManager.PickUp(position);
        }


	}

	public bool Check(Vector3Int position)
	{
		if(cropsManager == null) 
		{
			Debug.LogWarning("No tilemap crops manager are referenced in the corps manager");
			return false;
		}
		return cropsManager.Check(position);
	}

	public void Seed(Vector3Int position, Crop toSeed)
	{

		if(cropsManager == null) 
		{
			Debug.LogWarning("No tilemap crops manager are referenced in the corps manager");
			return;
		}

		cropsManager.Seed(position, toSeed);
	}

	public void Plow(Vector3Int position)
	{
		if(cropsManager == null) 
		{
			Debug.LogWarning("No tilemap crops manager are referenced in the corps manager");
			return;
		}

		cropsManager.Plow(position);
	}

	//Saving the crops container data. 
	public void Save()
    {
		if(cropsManager == null)
        {
			Debug.Log("Tilemap Crops Manager is not found.");
			return;
        }
		


    }


}
