using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashPanel : MonoBehaviour
{
    [SerializeField] Text cashValue; 

    void Update()
    {
        cashValue.text = GameManager.instance.character.GetCashValue().ToString();
    }
}
