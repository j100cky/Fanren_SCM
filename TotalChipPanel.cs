using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TotalChipPanel : MonoBehaviour
{
    [SerializeField] Text amount;

    void Update()
    {
        amount.text = GameManager.instance.character.chip.ToString();
    }

}
