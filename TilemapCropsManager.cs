using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCropsManager : TimeAgent
{
	[SerializeField] CropsContainer container;

	[SerializeField] TileBase plowed;
	[SerializeField] TileBase seeded;
	Tilemap targetTilemap;

	[SerializeField] GameObject cropsSpritePrefab;

	private void Start()
	{
		GameManager.instance.GetComponent<CropManager>().cropsManager = this;	//Give reference to the CropManager 
		targetTilemap = GetComponent<Tilemap>(); //Since this script is attached to CropsTilemap, we don't need to GameObject.Find().
		onTimeTick += Tick;
		Init();
		VisualizeMap(); //Restore the tiles that are stored in the CropsContainer. 
	}

	private void VisualizeMap() //Restore the tiles that are stored in the CropsContainer. 
	{
		for(int i = 0; i < container.crops.Count; i++)
		{
			VisualizeTile(container.crops[i]);
		}
	}

	private void OnDestroy() //When leaving the scene, nullify the sprites of the crops that are stored in the container...for reasons I don't understand.
	{
		for(int i = 0; i < container.crops.Count; i++)
		{
			container.crops[i].renderer = null;
		}
	}



	public void Tick() //For calculating the growth stage of crops.
	{
		if(targetTilemap == null) {return;}

		foreach(CropTile cropTile in container.crops)
		{
			if(cropTile.crop == null) {continue;}
			
			cropTile.damage += 0.02f;
			if(cropTile.damage > 100f) //Crops will disappear after some period of time.//
			{
				cropTile.Harvested();
				targetTilemap.SetTile(cropTile.position, plowed);
				Debug.Log("The crop disappeared due to damage");
				continue;
			}

			if(cropTile.Complete)
			{
				//Debug.Log("I am done growing");
				continue;
				//cropTile.crop = null; 
			}
			
			cropTile.growTimer += 1;

			if(cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
			{
				cropTile.renderer.gameObject.SetActive(true);
				cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];

				cropTile.growStage += 1;

			}
		}
	}

	internal bool Check(Vector3Int position) //Check if the crops container contains a tile at the position
                                             //being checked. If true, that means the tile is registered. 
	{
		return container.Get(position) != null;
	}

	public void Plow(Vector3Int position) //Plow the tile if the position is not registered. 
	{
		if(Check(position) == true) {return;}
		CreatePlowedTile(position);
	}

	public void Seed(Vector3Int position, Crop toSeed) //Seed the tile...
	{
		CropTile tile = container.Get(position);

		if(tile == null) {return;} //If the position is not registered, then don't seed. 

		targetTilemap.SetTile(position, seeded); //Only when the tile is registered , meaning plowed, will it replace the plowed tile to seeded tile. 

		tile.crop = toSeed; //Assign the Crop to the tile's crop variable. 
	}

	public void VisualizeTile(CropTile cropTile) //Setting the tile's sprite. 
	{

		if(cropTile.crop != null) //This crop will be checked in the crop container list. If not null,
                                  //meaning plowed, then seed the tile. If null, then plow the tile. 
		{
			targetTilemap.SetTile(cropTile.position, seeded);
		}
		else
		{
			targetTilemap.SetTile(cropTile.position, plowed);
		}


		if(cropTile.renderer == null) //When exit a scene, the rederer will be set null. When re-enter, here it restores the renderer.
		{
			GameObject go = Instantiate(cropsSpritePrefab, transform);
			go.transform.position = targetTilemap.CellToWorld(cropTile.position);
			go.transform.position -= Vector3.forward * 0.1f;	
			go.SetActive(false);
			cropTile.renderer = go.GetComponent<SpriteRenderer>();	//Assign the sprite of the GO to the new crop, but in this case it is null.
		}

		//If the crop is growing, we need to retore the gameobject that shows the growing plant at the correct growing state. 
		bool growing = 
			cropTile.crop != null && 
			cropTile.growTimer >= cropTile.crop.growthStageTime[0];
			//growing is true only when there is a crop assigned to cropTile and that the growTimer is larger than the initial time of the crop growth.

		cropTile.renderer.gameObject.SetActive(growing); 
		if (growing == true)
		{
			cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage-1]; //Restore the sprite of the game object
                                                                                    //with the corresponding growth stage of the plant. 
		}
	}

	//This is plowing the tile for the first time, different from reading the container and visualizing the map.
	private void CreatePlowedTile(Vector3Int position)
	{

		//Debug.Log("croptilemap found");
		CropTile crop = new CropTile(); 
		container.Add(crop);	//Add a new crop and its position into the crops list.

		

		crop.position = position;	//Assign the position of the crop to the new crop in the list.

		VisualizeTile(crop);

		targetTilemap.SetTile(position, plowed);	//Change the tile on this position into the plowed tile.
	}

	internal void PickUp(Vector3Int gridPosition)
	{

		Vector2Int position = (Vector2Int)gridPosition;

		CropTile tile = container.Get(gridPosition);
		if(tile == null) {return;}

		if(tile.Complete)
		{
			ItemSpawnManager.instance.SpawnItem(targetTilemap.CellToWorld(gridPosition), 
				tile.crop.yield, 
				tile.crop.count);

			GameManager.instance.character.GainForagingEXP(tile.crop.giveForagingEXP); //Increase the foraging exp of the
                                                                                            //player based on the value in the crops datasheet.

			tile.Harvested(); //Set the growthtime and stage to 0...

			VisualizeTile(tile);
		}
	}

}
