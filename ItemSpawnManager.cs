using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
   public static ItemSpawnManager instance;

   private void Awake()
   {
   		instance = this;
   }

   [SerializeField] GameObject pickUpItemPrefab;

/*    private void Update()
    {
        if(Input.GetKeyDown("t"))
        {
            Vector3 position = new Vector3(GameManager.instance.player.transform.position.x + 5,
                GameManager.instance.player.transform.position.y,
                GameManager.instance.player.transform.position.z);
            GameObject o = Instantiate(pickUpItemPrefab, position, Quaternion.identity);
        }
    }*/

   public void SpawnItem(Vector3 spawnPosition, Item item, int count)
   {
        //Check to see if count is many.
        if(count >= 10) //If it is too many, drop one gameObject, but in the one gameobject, set the drop count to the count. 
        {
            GameObject o = Instantiate(pickUpItemPrefab, spawnPosition, Quaternion.identity);
            o.GetComponentInChildren<PickUpItem>().Set(item, count);
        }
        else //If the number is not too big, drop as many gameObject as designated by count. This saves space.
        {
            for (int i = 0; i < count; i++)
            {
                //spawnPosition.x += 3 * UnityEngine.Random.value - 1f;
                //spawnPosition.y += 3 * UnityEngine.Random.value - 1f;
                GameObject o = Instantiate(pickUpItemPrefab, spawnPosition, Quaternion.identity);
                o.GetComponentInChildren<PickUpItem>().Set(item, 1);
            }
        }


   }

}
