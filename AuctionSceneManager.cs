using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionSceneManager : MonoBehaviour
{
    [SerializeField] AuctionHostDialogPanelScript auctionHostDialogScript;
    [SerializeField] AuctionHostScript auctionHostItemListScript; //This script contains the list of items carried by the host. 
    Character character; //Will be used to reduce character cash. 
    [SerializeField] float sessionTime; //The amount of time each auction session will take. 
    [SerializeField] TimeBarScript timeBarScript; //The script that determines how the timebar should change. 
    [SerializeField] AuctionBidController bidController; //This script updates the current bid prices. 
    [SerializeField] List<GameObject> auctionParticipants; //A list of NPC that comes to the auction to compete with the player. 
    //List<Item> playerCart; //The list of items that the player successfully purchased. 
    [SerializeField] GameObject auctionItem; //This is the prefab to hold the item being auctioned. 
    private List<ItemSlot> auctionItemList; //The list of item that is passed from the AuctionHostDialogPanelScript. 
    private Item currentItem; //Keeps track on the current auction item. 
    private int currentBid; //Keeps track on the current bid of the currentItem.
    private string currentBidderName; //Keeps track on the current bidder's name.
    private Vector3 itemDisplayPosition = new Vector3(-1.15f, 4.25f, 0); 


    int itemNumber; //Used to count the ID of the item being auctioned. After each Initiate(), itemNumber must +1.

    int auctionStatus; //Determine which case the Conclude() function should go to.
                           //0 = finished an introduction of a new session, call StartCoroutine,
                           //1 = finished announcing the winner, InitializeNewSession,
                           //2 = finished the whole session, ConcludeAuction

    private void Start()
    {
        itemNumber = 0; //Starting from the first item of the list. 
        auctionStatus = 0; //Starting with auction status = 0.
        auctionItemList = auctionHostItemListScript.GetListOfItems(); //Obtain the list of items. 
        character = GameManager.instance.player.GetComponent<Character>();
        //auctionHostDialogScript.Initialize(itemNumber);
        InitializeNewAuctionItem(); //Initialize the first auction session at start. 
    }

    private void Update()
    {

    }

    private void InitializeNewAuctionItem() //This function sets up all the changes that will take place at the beginning of one session. 
    {
        currentBidderName = null; //Reset the current bidder name. 
        currentItem = auctionItemList[itemNumber].item; //Update the item in the currentItem variable. 
        auctionHostDialogScript.Initialize(currentItem); //Initialize the bottom auction speech panel. 
        bidController.UpdateItemPrices(currentItem); //Update the item prices on the top auction panel. 
        UpdateItemSprite(currentItem, itemDisplayPosition); //Change the sprite of the auction item.
        timeBarScript.SetMaxTime(sessionTime); //Reset the time bar to max. 

    }

    public void UpdateItemSprite(Item item, Vector3 position) //This function updates the item sprite to the currentItem that is being auctioned. 
    {
        auctionItem.GetComponent<SpriteRenderer>().sprite = currentItem.icon; //Update it to the current item's sprite.
        auctionItem.transform.position = position; //This step might not be necessary.
                                                                        //But can put a rigidbody component to the GO so it falls onto the podium naturally. 
    }

    public void Conclude()
    {
        if(auctionStatus == 0)
        {
            //When the session introduction is finished, begin auction session. 
            StartCoroutine(SessionCountDown()); 
        }
        else if(auctionStatus == 1)
        {
            //When finishing announcing the winner, the winner will be awarded, and a new auction session will be initialized. 
            StartCoroutine(AwardWinnerThenNewSession());
            auctionStatus = 0; //Reset the auction status to 0 so after this, coroutine will be called. 
        }
        else if(auctionStatus == 2)
        {
            StartCoroutine(AwardWinnerThenEnd());
            auctionStatus = 3; //Switch value to 3 so when the ConcludeAuctionSpeech() is called, Conclude() function here will return void.
        }
        else
        {
            return; 
        }
    }

    public void SetAuctionStatus() //This is for other scripts to set the Auction Status of the auction. 
    {

    }

    public IEnumerator SessionCountDown()
    {
        //float timeStartedLerping = 0f;
        float timeSinceStarted = 0.01f;
        float percentageComplete = timeSinceStarted / sessionTime;

        while(percentageComplete <= 1)
        {
            if(GamePauseController.instance.isPaused == true) 
            {
                Debug.Log("game is paused");
                yield return new WaitForSeconds(1);
            }
            else
            {
                currentBid = bidController.GetCurrentBid();
                timeBarScript.SetTime((sessionTime-timeSinceStarted));
                Debug.Log(percentageComplete);
                timeSinceStarted += 1;
                percentageComplete = timeSinceStarted / sessionTime;
                //Use percentage complete to determine the color of the remaining timebar. 
                NPCBid();

                yield return new WaitForSeconds(1);
                if (percentageComplete >= 1)
                {
                    timeBarScript.SetTime((sessionTime - timeSinceStarted)); //check to see if deleting this will make any changes. 
                    break;
                } 

            }

        }
        //The next dialog will be initiated after the previous session (enumerator) is finished. 
        itemNumber++;
        //AwardWinner();
        //Announcing the winner. 
        auctionHostDialogScript.AnnounceWinner(currentItem, currentBid, currentBidderName);
        if (itemNumber >= auctionItemList.Count)
        {

            auctionStatus = 2; //Set auction status to 2 so that the Conclude() function will call the appropriate functions. 
        }
        else
        {
            //Change the auction status to 1 (finished announcing the winner, start InitializeNewSession()
            auctionStatus = 1;
        }


    }


    public void RegisterAuctioneer(GameObject actor)
    {
        auctionParticipants.Add(actor);
    }



    //This is the function that determines whether the NPC will bid, how often, and how much is the cap. 
    private void NPCBid()
    {
        
        foreach(GameObject ac in auctionParticipants)
        {
            float f = Random.Range(0f, 1f);
            Actor currentActor = ac.GetComponent<NPCAuctionScript>().GetActor();
            if (currentActor.favoriteItems.Contains(currentItem) && f <= 0.9f)//Even in favorite, the NPC will still have 10% chance to not bid.
            {
                if (currentBid <= currentItem.purchasePrice * 20) 
                {
                    //Indicate a bit has been placed on the sceen. 
                    MessageManager.instance.CallMsgPanel("+" + (int)(currentItem.purchasePrice / 10), ac.transform, Color.red);
                    bidController.UpdateCurrentBid((int)(currentItem.purchasePrice / 10));
                    currentBidderName = currentActor.Name;
                    currentBid = bidController.GetCurrentBid();

                }
                else
                {
                    return;
                }

            }
        }
    }

    public void PlayerBid()
    {
        if(character.GetCashValue() < currentBid)
        {
            Debug.Log("Not enough cash to bid.");
            return; 
        }
        else
        {
            bidController.UpdateCurrentBid((int)(currentItem.purchasePrice / 10));
            currentBidderName = "player";
        }

    }

    public void AwardWinner()
    {
        if(currentBidderName == "player")
        {
            //playerCart.Add(currentItem);
            //Play the animation of obtaining the item. 
            UsingSkills.instance.summonSkill(itemDisplayPosition, 105);
            auctionItem.GetComponent<SpriteRenderer>().sprite = null; //Reset the sprite to none. 
            StartCoroutine(WaitBeforeAnim());

            character.LoseMoney(currentBid); //Reduce that amount of cash from the player. 
            GameManager.instance.inventoryContainer.Add(currentItem, 1); //Add the item into the player's inventory.
        }
        else
        {
            return;
        }
    }

    public void EndAuctionEarly()
    {
        auctionHostDialogScript.ExitPrompt();
    }

//Don't want to do the cart way now. 
/*    public void ConcludeAuction()
    {
        //Give player the items in the playerCart
        foreach(Item item in playerCart)
        {
            GameManager.instance.inventoryContainer.Add(item, 1);
        }
    }*/

    public IEnumerator WaitBeforeAnim()
    {
        Vector3 abovePlayer = new Vector3(GameManager.instance.player.transform.position.x,
                                                GameManager.instance.player.transform.position.y + 3f,
                                                GameManager.instance.player.transform.position.z);

        yield return new WaitForSeconds(1.5f);

        UsingSkills.instance.summonSkill(abovePlayer, 105);

        yield return new WaitForSeconds(0.3f);

        UpdateItemSprite(currentItem, abovePlayer);

    }

    IEnumerator AwardWinnerThenNewSession()
    {
        AwardWinner();

        yield return new WaitForSeconds(3f);

        InitializeNewAuctionItem();
    }

    IEnumerator AwardWinnerThenEnd() //Called at the last award player (status = 2).
    {
        AwardWinner();

        yield return new WaitForSeconds(3f);
        auctionItem.GetComponent<SpriteRenderer>().sprite = null; //Reset the sprite to none. 

        auctionHostDialogScript.ConcludeAuctionSpeech(); //Call the conclude auction speech in the dialog script. 
    }
}
