using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZuheButton : CombinationButtonBaseScript
{
    [SerializeField] int targetA;
    [SerializeField] int targetB;

    public override void ToCalculate()
    {
        int a = combination[0];
        int b = combination[1];
        int c = combination[2];

        if (a == targetA || b == targetA || c ==targetA)
        {
            if(a == targetB || b == targetB || c == targetB)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }
        else
        {
            Lose();
        }
    }
}
