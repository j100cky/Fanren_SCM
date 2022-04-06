using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapWildCropsManager : MonoBehaviour
{
    [SerializeField] CropsContainer container; //Container that indicates all the possible grids for wild plant growth. 
    [SerializeField] List<Crop> cropsThatGrow; //Fill in the list of all crops that will grow in this scene. 
    [SerializeField] TileBase seeded;
    Tilemap targetTilemap;
    [SerializeField] GameObject cropsSpritePrefab;

    private void Awake()
    {

        //Randomize crops for the crop container. 
        if (GameManager.instance.GetComponent<CropManager>().roadToTownRandomized == false)
        {
            RandomizeCrop();
            GameManager.instance.GetComponent<CropManager>().roadToTownRandomized = true;
        }
    }


    private void Start()
    {
        //Give reference to the GameManager's . 
        GameManager.instance.GetComponent<CropManager>().wildCropManager = this;
        //Get the CropsTilemap component that this script is attached to. 
        targetTilemap = GetComponent<Tilemap>();
        //Show the tiles.
        VisualizeMap();
    }

    //On each awake, randomize the crops into each CropTile in the CropsContainer.
    private void RandomizeCrop()
    {
        for (int i = 0; i < container.crops.Count; i++)
        {
            int num = Random.Range(0, cropsThatGrow.Count);
            container.crops[i].crop = cropsThatGrow[num];
        }
    }

    //Determine which CropTile is to be shown, and calls the VisualizeTile() to show the CropTile.
    private void VisualizeMap()
    {
        for(int i = 0; i < container.crops.Count; i++)
        {
            if(container.crops[i].isGrown == true)
            {
                VisualizeTile(container.crops[i]);
                continue;
            }
            else
            {
                float rand = Random.Range(0f, 1f);
                if (rand >= 0.99f)
                {
                    VisualizeTile(container.crops[i]);
                }
                else
                {
                    continue;
                }
            }

        }
    }

    private void VisualizeTile(CropTile cropTile)
    { 
        if(cropTile.renderer == null)
        {
            GameObject go = Instantiate(cropsSpritePrefab, transform);
            go.transform.position = targetTilemap.CellToWorld(cropTile.position);
            go.transform.position -= Vector3.forward * 0.1f;
            go.SetActive(false);
            cropTile.renderer = go.GetComponent<SpriteRenderer>();
        }
        cropTile.renderer.gameObject.SetActive(true);
        //Set the isGrown bool to true so that the CropTile will not be visualized again on next enter. 
        cropTile.isGrown = true;
        //Set the sprite straight to the last sprite (the well-grown plant).
        int spriteCount = cropTile.crop.sprites.Count; //This is needed to access the last sprite of the list of sprites. 
        cropTile.renderer.sprite = cropTile.crop.sprites[spriteCount- 1];
        //Set the grow timer of the tile to the maximum timeToGrow of the crop so that Complete() function in CropTile will return true.
        //This is needed for the Plow() function. 
        cropTile.growTimer = cropTile.crop.timeToGrow;
/*        //If the cropTile's grow timer is larger then the initial growth stage time of the crop, set the sprite.
        bool growing = cropTile.crop != null &&
            cropTile.growTimer >= cropTile.crop.growthStageTime[0];
        cropTile.renderer.gameObject.SetActive(growing);
        if(growing == true)
        {
            //Set the appropriate growing sprite for that grow stage.
            cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage - 1];
        }*/

    }

    //This function is defines how the plant is harvested (empty hand right clich). This is called by the CropManager in the GameManager.
    internal void PickUp(Vector3Int gridPosition)
    {
        Vector2Int position = (Vector2Int)gridPosition;

        //Get the CropTile information on the gridPosition (mouse clicked position).
        CropTile tile = container.Get(gridPosition);
        //If there is no CropTile data, meaning that the tile does not have any crop, return. 
        if(tile == null) { return; }
        //If there is a CropTile data, and the tile's grow timer is larger than the crop's time to grow, spawn the item. 
        //This prevents the CropTile being able to spawn more items even after the sprite is destroyed. 
        if(tile.Complete)
        {
            ItemSpawnManager.instance.SpawnItem(targetTilemap.CellToWorld(gridPosition),
                tile.crop.yield, tile.crop.count);

        }
        //Set the isGrown bool to false so that the tile can be randomized on next enter. 
        tile.isGrown = false;
        //For wild plant, we have a different Harvested() style. 
        tile.WildPlantHarvested();
        //Remove the tile. 
        Destroy(tile.renderer.gameObject);
    }

}
