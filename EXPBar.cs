using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
	public Slider slider;

	public void SetMaxEXP(float maxEXP, float currentEXP)
	{
		slider.maxValue = maxEXP;
		slider.value = currentEXP;

	}

	public void SetEXP(float EXP) //setting the updated EXP//
	{
		slider.value = EXP;
	}

}
