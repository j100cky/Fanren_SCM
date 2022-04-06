using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigButton : CombinationButtonBaseScript
{
    public override void ToCalculate()
    {
        int result = 0;
        for (int i = 0; i < combination.Count; i++)
        {
            result += combination[i];
        }
        if (result > 10)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }
}
