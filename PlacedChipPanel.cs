using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacedChipPanel : MonoBehaviour
{
    [SerializeField] Text placedAmount;

    public void GetAmount(int amount)
    {
        placedAmount.text = amount.ToString();
    }
}
