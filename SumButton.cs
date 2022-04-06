using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SumButton : CombinationButtonBaseScript
{
    [SerializeField] int target;

    public override void ToCalculate()
    {
        if(combination[0]+combination[1]+combination[2] == target)
        {
            Win();
        }

        else
        {
            Lose();
        }

    }
}
