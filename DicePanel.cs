using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DicePanel : MonoBehaviour
{
    [SerializeField] List<Sprite> diceFaces; 


    public int RollDice() //The DiceGamePanel will read the return value of this one by one and determine the combination.
    {
        int point = Random.Range(1, 7); //will be 1, 2, 3, 4, 5, 6
        gameObject.GetComponent<Image>().sprite= diceFaces[point-1]; //Because the list elements started from 0.
        return point;
    }
}
