using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
	public Slider slider;

	private void Update()
    {
		SetMana(GameManager.instance.character.currentMana);
    }

	public void SetMaxMana(float mana)
	{
		slider.maxValue = mana;
		slider.value = mana;

	}

	public void SetMana(float mana) //setting the updated health (补蓝扣蓝)//
	{
		slider.value = mana;
	}

}
