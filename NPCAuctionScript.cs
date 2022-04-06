using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAuctionScript : MonoBehaviour
{
    [SerializeField] Actor actor;
    GameObject auctionSceneManager; 
    // Start is called before the first frame update
    void Start()
    {
        //Register to the AuctionSceneManager when it is available. 
        auctionSceneManager = GameObject.Find("AuctionSceneManager");
        if (auctionSceneManager!=null&&actor!= null)
        {
            auctionSceneManager.GetComponent<AuctionSceneManager>().RegisterAuctioneer(gameObject);
        }
    }

    public Actor GetActor()
    {
        return actor;
    }


}
