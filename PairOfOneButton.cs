using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Determine if there is a pair of 1 in the combination.
public class PairOfOneButton : CombinationButtonBaseScript
{
    [SerializeField] int target;

    public override void ToCalculate() 
    {

        int a = combination[0];
        int b = combination[1];
        int c = combination[2];

        if(a == b && a == target)
        {
            Win();
        }
        else if(a == c && a == target)
        {
            Win();
        }
        else if(b == c && b == target)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }
}
