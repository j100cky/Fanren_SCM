using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionBidController : MonoBehaviour
{
    [SerializeField] AuctionHostScript actionHost;
    [SerializeField] Text basePrice;
    [SerializeField] Text currentBid;
    [SerializeField] Text minimumBid;
    int currentBidValue;


    public void UpdateItemPrices(Item item)
    {
        basePrice.text = item.purchasePrice.ToString();
        currentBidValue = item.purchasePrice;
        currentBid.text = currentBidValue.ToString();
        minimumBid.text = (item.purchasePrice / 10f).ToString();
    }

    public void UpdateCurrentBid(int value)
    {
        currentBidValue += value;
        currentBid.text = currentBidValue.ToString();
        Debug.Log("raised" + value);
    }

    public int GetCurrentBid()
    {
        return currentBidValue;
    }
}
