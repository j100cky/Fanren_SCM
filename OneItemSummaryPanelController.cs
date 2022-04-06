using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneItemSummaryPanelController : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text count;
    [SerializeField] Text totalValue;

    public void Set(ItemSlot itemSlot)
    {
        icon.sprite = itemSlot.item.icon;
        count.text = itemSlot.count.ToString();
        totalValue.text = (itemSlot.item.sellingPrice * itemSlot.count).ToString();
    }
}
