using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs; //Import a list of enemy prefabs that should appear in this map. 
    [SerializeField] Tilemap spawnTilemap;
    [SerializeField] List<Vector3Int> positions;
    [SerializeField] float enemySpawnPercentage = 0.01f; //Each position will generate an enemy at this percentage. Default at 1%
    [SerializeField] List<GameObject> spawnedEnemy; //A list to keep track of all the spawned enemies. 

    void Start()
    {
        spawnedEnemy = new List<GameObject>();
        RegisterTilePositions();
        SpawnEnemy();
        SceneExitDropManager.instance.SetEnemyLeft(spawnedEnemy.Count);
    }

    private void OnDestroy()
    {
        for(int i = 0; i<spawnedEnemy.Count; i++)
        {
            Destroy(spawnedEnemy[i]);
        }
    }

    //This function takes each tile in the spawnTilemap and retuens a Vector3Int value to the list of Vector3Int.
    private void RegisterTilePositions()
    {
        foreach (Vector3Int position in spawnTilemap.cellBounds.allPositionsWithin)
        {
            //Do not register empty tiles. 
            if(spawnTilemap.HasTile(position) == false)
            {
                continue;
            }
            positions.Add(position);
        }
    }

    //This function will instantiate enemy prefabs on the list of Vector3Int positions at <1% chance. 
    public void SpawnEnemy()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            //Generate a random number to determine whether a position can generate an enemy or not. 
            float randomNum = Random.Range(0f, 1f);
            //Generate a random int to determine which enemy to spawn from the list. 
            int randomMon = Random.Range(0, enemyPrefabs.Count); 
            if(randomNum >= (1f-enemySpawnPercentage))
            {
                GameObject go = Instantiate(enemyPrefabs[randomMon], positions[i], Quaternion.identity);
                spawnedEnemy.Add(go);
            }
            else
            {
                continue;
            }
        }
    }
}
