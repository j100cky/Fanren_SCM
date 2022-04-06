using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	public Gradient gradient;
	public Image imageHP;

	private void Update()
	{
		SetHealth(GameManager.instance.character.currentHealth);
	}

	public void SetMaxHealth(float health)
	{
		slider.maxValue = health;
		slider.value = health;

		imageHP.color = gradient.Evaluate(1f); //Setting the gradient to maximum.//
	}

	public void SetHealth(float health) //setting the updated health (补血扣血)//
	{
		slider.value = health;

		imageHP.color = gradient.Evaluate(slider.normalizedValue); //Setting the gradient to whatever the slider value is.//
	}

}
