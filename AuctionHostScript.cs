using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionHostScript : MonoBehaviour
{
    [SerializeField] List<ItemSlot> auctionItemList; //Store the items that this host will present during the entire auction meeting. 

    public List<ItemSlot> GetListOfItems()
    {
        return auctionItemList;
    }
}
