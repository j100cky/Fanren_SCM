using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaoziButton : CombinationButtonBaseScript
{
    [SerializeField] int target;
    public override void ToCalculate()
    {
        int a = combination[0];
        int b = combination[1];
        int c = combination[2];

        if(a == target)
        {
            if(a == b && b == c)
            {
                Win();
            }
        }

        else
        {
            Lose();
        }

    }
}
